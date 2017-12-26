using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace X86Sharp.Tests
{
    [TestClass]
    public class InstructionsTests
    {
        [TestMethod, TestCategory("Instruction")]
        public void TestAllInstructionLoaded()
        {
            VM vm = new VM();
            foreach (InstructionType type in Enum.GetValues(typeof(InstructionType)))
            {
                vm.Instructions.GetInstructionFromType(type);
            }
        }
    }
}
