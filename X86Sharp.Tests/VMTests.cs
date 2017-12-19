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
            Assert.AreEqual((uint)42, vm.Registers.EAX);

            ref var eax = ref vm.Registers.EAX;
            eax = 24;
            Assert.AreEqual((uint)24, vm.Registers.EAX);

            uint num = 42;
            var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.Mov);
            mov(ref eax, ref num);
            Assert.AreEqual((uint)42, vm.Registers.EAX);
        }
    }
}
