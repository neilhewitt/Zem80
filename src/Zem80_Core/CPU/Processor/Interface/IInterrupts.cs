using System;

namespace Zem80.Core.CPU
{
    public interface IInterrupts
    {
        bool Enabled { get; }
        bool IFF1 { get; }
        bool IFF2 { get; }
        InterruptMode Mode { get; }

        event EventHandler<long> OnMaskableInterrupt;
        event EventHandler<long> OnNonMaskableInterrupt;

        void Disable();
        void Enable();
        void HandleAll(InstructionPackage package, Action<InstructionPackage> IM0_ExecuteInstruction);
        void RaiseMaskable(Func<byte> callback = null);
        void RaiseNonMaskable();
        void RestoreAfterNMI();
        void SetMode(InterruptMode mode);
    }
}