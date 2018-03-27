using System;
using System.Collections.Generic;
using System.Text;

namespace X86Sharp
{
    public class Operand
    {
        private OperandType _type { get; }

        private uint _value;

        public ref uint Value => ref _value;

        private OperandSize _size { get; }

        private Operand(OperandType type, uint value, OperandSize size)
        {
            _type = type;
            _value = value;
            _size = size;
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

        public int SignedValue
        {
            get
            {
                switch(_size)
                {
                    case OperandSize.BYTE:
                        return (sbyte)_value;
                    case OperandSize.WORD:
                        return (short)_value;
                    case OperandSize.DWORD:
                        return (int)_value;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch(_size)
                {
                    case OperandSize.BYTE:
                        _value = (byte)value;
                        break;
                    case OperandSize.WORD:
                        _value = (ushort)value;
                        break;
                }
            }
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
