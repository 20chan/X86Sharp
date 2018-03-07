using System;

namespace X86Sharp
{
    public delegate void InstructionCallback0args();
    public delegate void InstructionCallback1arg(Operand op1);
    public delegate void InstructionCallback2args(Operand op1, Operand op2);
    public delegate void InstructionCallback3args(Operand op1, Operand op2, Operand op3);

    [AttributeUsage(AttributeTargets.Method)]
    public class Instruction : Attribute
    {
        public InstructionType InstructionType;
        public uint[] OpCodes;
        public Instruction(InstructionType type, params uint[] opcodes)
        {
            OpCodes = opcodes;
            InstructionType = type;
        }
    }
}
