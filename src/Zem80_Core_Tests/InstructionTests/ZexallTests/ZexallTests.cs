using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Zem80.Core.CPU;
using ZexNext.Core;

namespace Zem80.Core.Tests.Zexall
{
    [TestFixture]
    public class ZexallTests
    {
        private Processor _cpu;
        private TestRunner _runner;
        private bool _runZexall;
        private bool _runZexdoc;

        [OneTimeSetUp]
        public void Setup()
        {
            _cpu = new Processor();

            _runZexall = true;
            _runZexdoc = false;

            // set up the ZexNext test runner using the zipped test file (zexall.zip)
            Task.Run(() => _runner = new TestRunner(
                (address, data) => _cpu.Memory.WriteBytesAt(address, data),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\InstructionTests\\ZexallTests\\zexall.zip")
                ));
        }

        [Test]
        [TestCase("<adc,sbc> hl,<bc,de,hl,sp>")]
        [TestCase("add hl,<bc,de,hl,sp>")]
        [TestCase("add ix,<bc,de,ix,sp>")]
        [TestCase("add iy,<bc,de,iy,sp>")]
        [TestCase("aluop a,nn")]
        [TestCase("aluop a,<b,c,d,e,h,l,(hl),a>")]
        [TestCase("aluop a,<ixh,ixl,iyh,iyl>")]
        [TestCase("aluop a,(<ix,iy>+1)")]
        [TestCase("bit n,(<ix,iy>+1)")]
        //[TestCase("bit n,<b,c,d,e,h,l,(hl),a>")] - excluded because this is known bad source data
        [TestCase("cpd<r>")]
        [TestCase("cpi<r>")]
        [TestCase("<daa,cpl,scf,ccf>")]
        [TestCase("<inc,dec> a")]
        [TestCase("<inc,dec> b")]
        [TestCase("<inc,dec> bc")]
        [TestCase("<inc,dec> c")]
        [TestCase("<inc,dec> d")]
        [TestCase("<inc,dec> de")]
        [TestCase("<inc, dec> e")]
        [TestCase("<inc, dec> h")]
        [TestCase("<inc,dec> hl")]
        [TestCase("<inc,dec> ix")]
        [TestCase("<inc,dec> iy")]
        [TestCase("<inc,dec> l")]
        [TestCase("<inc,dec> (hl)")]
        [TestCase("<inc,dec> sp")]
        [TestCase("<inc,dec> (<ix,iy>+1)")]
        [TestCase("<inc,dec> ixh")]
        [TestCase("<inc,dec> ixl")]
        [TestCase("ld <bc,de>,(nnnn)")]
        [TestCase("ld hl,(nnnn)")]
        [TestCase("ld sp,(nnnn)")]
        [TestCase("ld <ix,iy>,(nnnn)")]
        [TestCase("ld (nnnn),<bd,de>")]
        [TestCase("ld (nnnn),hl")]
        [TestCase("ld (nnnn),sp")]
        [TestCase("ld (nnnn),<ix,iy>")]
        [TestCase("ld <bc,de,hl,sp>,nnnn")]
        [TestCase("ld a,<(bc),(de)>")]
        [TestCase("ld <b,c,d,e,h,l,(hl),a>,nn")]
        [TestCase("ld (<ix,iy+1),nn")]
        [TestCase("ld <b,c,d,e>,(<ix,iy>+1)")]
        [TestCase("ld <h,l>,(<ix,iy>+1)")]
        [TestCase("ld a,(<ix,iy>+1)")]
        [TestCase("ld <ixh,ixl,iyh,iyl>,nn")]
        [TestCase("ld <b,c,d,e,h,l,a>,<b,c,d,e,h,l,a>")]
        [TestCase("ld <b,c,d,e,ixy,a>,<b,c,d,e,ixy,a>")]
        [TestCase("ld a,(nnnn)")]
        [TestCase("ldd<r>")]
        [TestCase("ldi<r>")]
        [TestCase("neg")]
        [TestCase("<rrd,rld>")]
        [TestCase("<rlca,rrca,rla,rra>")]
        [TestCase("shf/rot (<ix,iy>+1)")]
        [TestCase("shf/rot <b,c,d,e,h,l,(hl),a>")]
        [TestCase("<set,res> n,<bcdehl(hl)a>")]
        [TestCase("<set,res> n,(<ix,iy>+1)")]
        [TestCase("ld (<ix,iy>+1),<b,c,d,e>")]
        [TestCase("ld (<ix,iy>+1),<h,l>")]
        [TestCase("ld (<ix,iy>+1),a")]
        [TestCase("ld (<bc,de>),a")]
        public void Zexall(string testSetName)
        {
            if (!_runZexall) return;

            // this is basically ZexNext tests running inside NUnit tests as a host

            // ensure the Visual Studio UI remains responsive while the tests are loading and compiling
            while (_runner == null) Thread.Sleep(0);

            // run the specified test set
            IEnumerable<TestResult> results = _runner.Run(testSetName, ExecuteTestCycle, false);
            Assert.That(results.All(x => x.Passed));
        }

        [Test]
        [TestCase("<adc,sbc> hl,<bc,de,hl,sp>")]
        [TestCase("add hl,<bc,de,hl,sp>")]
        [TestCase("add ix,<bc,de,ix,sp>")]
        [TestCase("add iy,<bc,de,iy,sp>")]
        [TestCase("aluop a,nn")]
        [TestCase("aluop a,<b,c,d,e,h,l,(hl),a>")]
        [TestCase("aluop a,<ixh,ixl,iyh,iyl>")]
        [TestCase("aluop a,(<ix,iy>+1)")]
        [TestCase("bit n,(<ix,iy>+1)")]
        [TestCase("bit n,<b,c,d,e,h,l,(hl),a>")]
        [TestCase("cpd<r>")]
        [TestCase("cpi<r>")]
        [TestCase("<daa,cpl,scf,ccf>")]
        [TestCase("<inc,dec> a")]
        [TestCase("<inc,dec> b")]
        [TestCase("<inc,dec> bc")]
        [TestCase("<inc,dec> c")]
        [TestCase("<inc,dec> d")]
        [TestCase("<inc,dec> de")]
        [TestCase("<inc, dec> e")]
        [TestCase("<inc, dec> h")]
        [TestCase("<inc,dec> hl")]
        [TestCase("<inc,dec> ix")]
        [TestCase("<inc,dec> iy")]
        [TestCase("<inc,dec> l")]
        [TestCase("<inc,dec> (hl)")]
        [TestCase("<inc,dec> sp")]
        [TestCase("<inc,dec> (<ix,iy>+1)")]
        [TestCase("<inc,dec> ixh")]
        [TestCase("<inc,dec> ixl")]
        [TestCase("ld <bc,de>,(nnnn)")]
        [TestCase("ld hl,(nnnn)")]
        [TestCase("ld sp,(nnnn)")]
        [TestCase("ld <ix,iy>,(nnnn)")]
        [TestCase("ld (nnnn),<bd,de>")]
        [TestCase("ld (nnnn),hl")]
        [TestCase("ld (nnnn),sp")]
        [TestCase("ld (nnnn),<ix,iy>")]
        [TestCase("ld <bc,de,hl,sp>,nnnn")]
        [TestCase("ld a,<(bc),(de)>")]
        [TestCase("ld <b,c,d,e,h,l,(hl),a>,nn")]
        [TestCase("ld (<ix,iy+1),nn")]
        [TestCase("ld <b,c,d,e>,(<ix,iy>+1)")]
        [TestCase("ld <h,l>,(<ix,iy>+1)")]
        [TestCase("ld a,(<ix,iy>+1)")]
        [TestCase("ld <ixh,ixl,iyh,iyl>,nn")]
        [TestCase("ld <b,c,d,e,h,l,a>,<b,c,d,e,h,l,a>")]
        [TestCase("ld <b,c,d,e,ixy,a>,<b,c,d,e,ixy,a>")]
        [TestCase("ld a,(nnnn)")]
        [TestCase("ldd<r>")]
        [TestCase("ldi<r>")]
        [TestCase("neg")]
        [TestCase("<rrd,rld>")]
        [TestCase("<rlca,rrca,rla,rra>")]
        [TestCase("shf/rot (<ix,iy>+1)")]
        [TestCase("shf/rot <b,c,d,e,h,l,(hl),a>")]
        [TestCase("<set,res> n,<bcdehl(hl)a>")]
        [TestCase("<set,res> n,(<ix,iy>+1)")]
        [TestCase("ld (<ix,iy>+1),<b,c,d,e>")]
        [TestCase("ld (<ix,iy>+1),<h,l>")]
        [TestCase("ld (<ix,iy>+1),a")]
        [TestCase("ld (<bc,de>),a")]
        public void Zexdoc(string testSetName)
        {
            if (!_runZexdoc) return;

            // this is basically ZexNext tests running inside NUnit tests as a host

            // ensure the Visual Studio UI remains responsive while the tests are loading and compiling
            while (_runner == null) Thread.Sleep(0);

            // run the specified test set
            IEnumerable<TestResult> results = _runner.Run(testSetName, ExecuteTestCycle, true);
            Assert.That(results.All(x => x.Passed));
        }

        private TestState ExecuteTestCycle(TestState input)
        {
            _cpu.Registers.AF = input.AF;
            _cpu.Registers.BC = input.BC;
            _cpu.Registers.DE = input.DE;
            _cpu.Registers.HL = input.HL;
            _cpu.Registers.IX = input.IX;
            _cpu.Registers.IY = input.IY;
            _cpu.Registers.SP = input.SP;
            _cpu.Registers.PC = 0x1D42;
            _cpu.Memory.WriteBytesAt(input.DataAddress, input.Data);

            _cpu.Debug.ExecuteDirect(input.Opcode);

            TestState afterExecution = new TestState(
            input.Opcode,
            input.Mnemonic,
            _cpu.Memory.ReadBytesAt(input.DataAddress, 16),
            _cpu.Registers.AF,
            _cpu.Registers.BC,
            _cpu.Registers.DE,
            _cpu.Registers.HL,
            _cpu.Registers.IX,
            _cpu.Registers.IY,
            _cpu.Registers.SP,
            _cpu.Registers.PC
            );

            return afterExecution;
        }
    }
}
