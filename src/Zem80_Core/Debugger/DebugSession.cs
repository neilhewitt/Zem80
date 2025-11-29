using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Debugger
{
    public class DebugSession : IDisposable
    {
        private List<DebugMonitor> _monitors;
        private Processor _cpu;
        private InstructionPackage _executingPackage;

        public DateTime StartTime { get; private set; }
        public ushort StartAddress { get; private set; }

        public IReadOnlyCollection<DebugMonitor> Monitors => (IReadOnlyCollection<DebugMonitor>)_monitors.AsReadOnly();

        public DebugMonitor Monitor(ushort breakpointAddress, DebugEventTypes eventTypes, Func<DebugState, DebugResponse> handler)
        {
            if (_cpu.Debug.Breakpoints.Contains(breakpointAddress))
            {
                throw new DebuggerException("Specified breakpoint does not exist.");
            }

            lock (_monitors)
            {
                _monitors.Add(new DebugMonitor(breakpointAddress, eventTypes, handler));
                return _monitors.Last();
            }
        }

        public DebugMonitor Monitor(DebugEventTypes eventTypes, Func<DebugState, DebugResponse> handler)
        {
            lock (_monitors)
            {
                _monitors.Add(new DebugMonitor(eventTypes, handler));
                return _monitors.Last();
            }
        }

        public void RemoveMonitor(DebugMonitor monitor)
        {
            if (_monitors.Contains(monitor))
            {
                lock (_monitors)
                {
                    _monitors.Remove(monitor);
                }
            }
        }

        public void End()
        {
            _cpu.Debug.NotifySessionEnded();
            Dispose();
        }

        internal void NotifyBreakpointHit(ushort address)
        {
            HandleMonitors(DebugEventTypes.BreakpointReached, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "BreakpointHit"));
        }

        private void HandleMonitors(DebugEventTypes eventType, DebugState state)
        {
            List<DebugMonitor> monitors = new List<DebugMonitor>(_monitors.Where(m => m.EventTypes.HasFlag(eventType)));

            foreach (DebugMonitor monitor in monitors)
            {
                if (monitor.State == MonitorState.Active || monitor.Breaks(state.Address))
                {
                    DebugResponse response = monitor.Step(state.Address, state);
                    if (response == DebugResponse.Stop)
                    {
                        End();
                        return;
                    }
                }
            }
        }

        #region monitor bindings

        private void CPU_BeforeExecuteInstruction(object sender, InstructionPackage executingPackage)
        {
            _executingPackage = executingPackage;
            HandleMonitors(DebugEventTypes.BeforeInstructionExecution, new DebugState(_cpu.Registers, _cpu.Flags, executingPackage, "BeforeExecuteInstruction"));
        }

        private void CPU_AfterExecuteInstruction(object sender, ExecutionResult executionResult)
        {
            HandleMonitors(DebugEventTypes.AfterInstructionExecution, new DebugState(_cpu.Registers, executionResult, "AfterExecuteInstruction"));
        }

        private void Timing_BeforeInsertWaitCycles(object sender, int waitCycles)
        {
            HandleMonitors(DebugEventTypes.BeforeMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "BeforeInsertWaitCycles"));
        }

        private void Timing_OnOpcodeFetch(object sender, MemoryReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.BeforeMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "OpcodeFetch"));
        }

        private void Timing_OnMemoryRead(object sender, MemoryReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.BeforeMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "MemoryRead"));
        }

        private void Timing_OnMemoryWrite(object sender, MemoryReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.BeforeMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "MemoryWrite"));
        }

        private void Timing_OnStackRead(object sender, StackReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.AfterMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "StackRead"));
        }

        private void Timing_OnStackWrite(object sender, StackReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.AfterMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "StackWrite"));
        }

        private void Timing_OnPortRead(object sender, PortReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.AfterMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "PortRead"));
        }

        private void Timing_OnPortWrite(object sender, PortReadWriteCycleInfo info)
        {
            HandleMonitors(DebugEventTypes.AfterMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "PortWrite"));
        }

        private void Timing_OnInterruptAcknowledge(object sender, EventArgs e) // e will always be empty
        {
            HandleMonitors(DebugEventTypes.OnInterruptAcknowledge, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "InterruptAcknowledge"));
        }

        private void Timing_OnInternalOperation(object sender, int tStates)
        {
            HandleMonitors(DebugEventTypes.AfterMachineCycle, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "InternalOperation"));
        }

        private void Interrupts_OnMaskableInterrupt(object sender, long ticksSinceBoot)
        {
            HandleMonitors(DebugEventTypes.BeforeMaskableInterrupt, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "MaskableInterrupt"));
        }

        private void Interrupts_OnNonMaskableInterrupt(object sender, long ticksSinceBoot)
        {
            HandleMonitors(DebugEventTypes.BeforeNonMaskableInterrupt, new DebugState(_cpu.Registers, _cpu.Flags, _executingPackage, "NonMaskableInterrupt"));
        }

        #endregion

        public void Dispose()
        {
            // clean up event handlers

            _cpu.BeforeExecuteInstruction -= CPU_BeforeExecuteInstruction;
            _cpu.AfterExecuteInstruction -= CPU_AfterExecuteInstruction;

            if (_cpu.Timing is ProcessorTiming timing)
            {
                timing.BeforeInsertWaitCycles -= Timing_BeforeInsertWaitCycles;
                timing.OnOpcodeFetch -= Timing_OnOpcodeFetch;
                timing.OnMemoryRead -= Timing_OnMemoryRead;
                timing.OnMemoryWrite -= Timing_OnMemoryWrite;
                timing.OnStackRead -= Timing_OnStackRead;
                timing.OnStackWrite -= Timing_OnStackWrite;
                timing.OnPortRead -= Timing_OnPortRead;
                timing.OnPortWrite -= Timing_OnPortWrite;
                timing.OnInterruptAcknowledge -= Timing_OnInterruptAcknowledge;
                timing.OnInternalOperation -= Timing_OnInternalOperation;
            }

            if (_cpu.Interrupts is Interrupts interrupts)
            {
                interrupts.OnMaskableInterrupt -= Interrupts_OnMaskableInterrupt;
                interrupts.OnNonMaskableInterrupt -= Interrupts_OnNonMaskableInterrupt;
            }
        }

        public DebugSession(Processor cpu, InstructionPackage initialPackage) 
        {
            _cpu = cpu;
            _monitors = new List<DebugMonitor>();
            _executingPackage = initialPackage;

            _cpu.BeforeExecuteInstruction += CPU_BeforeExecuteInstruction;
            _cpu.AfterExecuteInstruction += CPU_AfterExecuteInstruction;

            if (_cpu.Timing is ProcessorTiming timing)
            {
                // add handlers for all the Timing events *using private handler methods*
                timing.BeforeInsertWaitCycles += Timing_BeforeInsertWaitCycles;
                timing.OnOpcodeFetch += Timing_OnOpcodeFetch;
                timing.OnMemoryRead += Timing_OnMemoryRead;
                timing.OnMemoryWrite += Timing_OnMemoryWrite;
                timing.OnStackRead += Timing_OnStackRead;
                timing.OnStackWrite += Timing_OnStackWrite;
                timing.OnPortRead += Timing_OnPortRead;
                timing.OnPortWrite += Timing_OnPortWrite;
                timing.OnInterruptAcknowledge += Timing_OnInterruptAcknowledge;
                timing.OnInternalOperation += Timing_OnInternalOperation;
            }

            if (_cpu.Interrupts is Interrupts interrupts)
            {
                // add handlers for any Interrupt events if needed *using private handler methods*
                interrupts.OnMaskableInterrupt += Interrupts_OnMaskableInterrupt;
                interrupts.OnNonMaskableInterrupt += Interrupts_OnNonMaskableInterrupt;
            }

            StartTime = DateTime.Now;
            StartAddress = initialPackage.InstructionAddress;
        }
    }
}
