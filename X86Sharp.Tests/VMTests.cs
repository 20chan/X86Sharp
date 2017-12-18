using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X86Sharp.Tests
{
    [TestClass]
    public class VMTests
    {
        [TestCategory("Instruction")]
        [TestMethod]
        public void LoadInstructionsTests()
        {
            VM vm = new VM();
            try
            {
                vm.Instructions.GetInstructionFromType<InstructionCallback0args>(InstructionType.Nop)();
            }
            catch (KeyNotFoundException)
            {
                Assert.Fail("Fail to load nop instruction");
            }
        }

        [TestCategory("Instruction")]
        [TestMethod]
        public void RegisterTests()
        {
            VM vm = new VM();
            vm.Registers.EAX = 42;
            Assert.AreEqual((uint)42, (uint)vm.Registers.EAX);

            ref var eax = ref vm.Registers.EAX;
            eax = 24;
            Assert.AreEqual((uint)24, (uint)vm.Registers.EAX);

            unsafe
            {
                ref var eaxop = ref Unsafe.AsRef<IOperand>(Unsafe.AsPointer(ref eax));
                var dword = (dwordop)42;
                ref var numop = ref Unsafe.AsRef<IOperand>(Unsafe.AsPointer(ref dword));
                var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.Mov);
                mov(ref eaxop, ref numop);
                Assert.AreEqual((uint)42, (uint)vm.Registers.EAX);
            }
        }
    }
}
