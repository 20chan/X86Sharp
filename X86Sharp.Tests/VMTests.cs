using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static X86Sharp.RegisterType;
using static X86Sharp.SegmentType;

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
            Assert.AreEqual(vm[EAX], vm.Registers.EAX);

            vm[EAX] = (uint)24;
            Assert.AreEqual((uint)24, vm[EAX]);
            Assert.AreEqual(vm[EAX], vm.Registers.EAX);

            uint num = 42;
            var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.MOV);
            mov(EAX, num);
            Assert.AreEqual((uint)42, vm.Registers.EAX);
        }

        [TestMethod, TestCategory("Memory")]
        public void MemorySpanTests()
        {
            VM vm = new VM();
            var mov = vm.Instructions.GetInstructionFromType<InstructionCallback2args>(InstructionType.MOV);

            var addr = new Address(SS, 4, displacement: 0);
            uint num = 0x12345678;
            mov(addr, num);
            
            var res = BitConverter.ToUInt32(vm.Memory.GetMemory(addr).ToArray(), 0);
            Assert.AreEqual(num, res);
        }

        [TestMethod, TestCategory("Memory")]
        public void PushPopTests()
        {
            VM vm = new VM();

            var dword0to4 = vm.Memory.GetMemory(new Address(SS, 4, vm.Registers.ESP));
            uint num = 0x01020304;
            var push = vm.Instructions.GetInstructionFromType<InstructionCallback1arg>(InstructionType.PUSH);
            push(num);

            CollectionAssert.AreEqual(new byte[] { 0x04, 0x03, 0x02, 0x01 }, dword0to4.ToArray());

            var pop = vm.Instructions.GetInstructionFromType<InstructionCallback1arg>(InstructionType.POP);
            pop(EAX);
            Assert.AreEqual(num, vm[EAX]);
        }
    }
}
