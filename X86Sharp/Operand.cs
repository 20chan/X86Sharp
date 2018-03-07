using System;
using System.Collections.Generic;
using System.Text;

namespace X86Sharp
{
    public struct Operand
    {
        private OperandType OperandType { get; }

        private uint? Value { get; }
        private OperandSize Size { get; }

        private Operand(OperandType type, uint value, OperandSize size)
        {
            OperandType = type;
            Value = value;
            Size = size;
        }

        public static implicit operator Operand(Address addr)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Operand(RegisterType reg)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Operand(SegmentType seg)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Operand(uint dword)
        {
            return new Operand(OperandType.Immediate, dword, OperandSize.DWORD);
        }

        public static implicit operator Operand(ushort word)
        {
            return new Operand(OperandType.Immediate, word, OperandSize.WORD);
        }

        public static implicit operator Operand(byte @byte)
        {
            return new Operand(OperandType.Immediate, @byte, OperandSize.BYTE);
        }
    }

    public enum OperandType
    {
        Register,
        Segment,
        Immediate,
        RegMem,
        Memory,
    }

    public enum OperandSize
    {
        DWORD,
        WORD,
        BYTE,
    }
}
