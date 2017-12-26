using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X86Sharp.Tests
{
    [TestClass]
    public class VMTests
    {
        [TestMethod, TestCategory("Instruction")]
        public void LoadInstructionsTests()
        {
            VM vm = new VM();
            try
            {
                vm.Instructions.GetInstructionFromType<InstructionCallback0args>(InstructionType.NOP)();
            }
            catch (KeyNotFoundException)
            {
                Assert.Fail("Fail to load nop instruction");
            }
        }

        [TestMethod, TestCategory("Instruction")]
        public void RegisterTests()
        {
            VM vm = new VM();
            vm.Registers.EAX = 42;
            Assert.AreEqual((uint)42, vm.Registers.EAX);

            ref var eax = ref vm.Registers.EAX;
            eax = 24;
            Assert.AreEqual((uint)24, vm.Registers.EAX);

            uint num = 42;
            var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.MOV);
            mov(ref eax, ref num);
            Assert.AreEqual((uint)42, vm.Registers.EAX);
        }

        [TestMethod, TestCategory("Memory")]
        public void MemorySpanTests()
        {
            VM vm = new VM();

            var dword0to4 = vm.Memory.GetValue(new Address(vm.Segments.SS, displacement: 0), 4);
            var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.MOV);
            ref var refptr = ref dword0to4.NonPortableCast<byte, uint>()[0];
            uint num = 0x12345678;
            mov(ref refptr, ref num);

            var res = BitConverter.ToUInt32(dword0to4.ToArray(), 0);
            Assert.AreEqual(num, res);
        }
    }
}
