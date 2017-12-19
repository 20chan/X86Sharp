using System;

namespace X86Sharp
{
    public delegate void InstructionCallback0args();
    public delegate void InstructionCallback1arg(ref uint op1);
    public delegate void InstructionCallback2args(ref uint op1, ref uint op2);
    public delegate void InstructionCallback3args(ref uint op1, ref uint op2, ref uint op3);

    [AttributeUsage(AttributeTargets.Method)]
    public class Instruction : Attribute
    {
        public InstructionType InstructionType;
        public uint OpCode;
        public Instruction(InstructionType type, uint opcode)
        {
            OpCode = opcode;
            InstructionType = type;
        }
    }
}
