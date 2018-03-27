using System.Runtime.InteropServices;

namespace X86Sharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GDT
    {
        private ushort _limit1;
        private ushort _base1;
        private byte _base2;
        private sbyte _access;
        private sbyte _limit2AndFlags;
        private byte _base3;

        public uint Limit
        {
            get => (uint)(_limit1 + (_limit2AndFlags & 0x0f) << 0x10);
            set
            {
                _limit1 = (ushort)value;
                _limit2AndFlags = (sbyte)((value & 0x0fffffff) >> 0x10);
            }
        }

        public uint BaseAddress
        {
            get => (uint)(_base1 + (_base2 << 0x10) + (_base3 << 0x18));
            set
            {
                _base1 = (ushort)value;
                _base2 = (byte)(value >> 0x10);
                _base3 = (byte)(value >> 0x18);
            }
        }

        private bool GetAccessNthBit(int n)
            => (_access & n) == n;
        private void SetAccessNthBit(int n, bool val)
        {
            if (val) _access |= (sbyte)n;
            else _access &= (sbyte)~n;
        }

        private bool GetFlagNthBit(int n)
            => (_limit2AndFlags & n) == n;
        private void SetFlagNthBit(int n, bool val)
        {
            if (val) _limit2AndFlags |= (sbyte)n;
            else _limit2AndFlags |= (sbyte)~n;
        }

        #region Access
        public bool Accessed
        {
            get => GetAccessNthBit(0x01);
            set => SetAccessNthBit(0x01, value);
        }

        public bool IsReadable
            => IsCode && GetAccessNthBit(0x02);

        public bool IsWritable
        {
            get => !IsCode && GetAccessNthBit(0x02);
            set => SetAccessNthBit(0x02, value);
        }

        public bool Conforming
            => IsCode && GetAccessNthBit(0x04);

        public bool Expansion
            => !IsCode && GetAccessNthBit(0x04);

        public bool IsCode
        {
            get => GetAccessNthBit(0x08);
            set => SetAccessNthBit(0x08, value);
        }

        public bool IsSystemSegment
            => GetAccessNthBit(0x10);

        public byte PrevLevel
            => (byte)(_access & 0x60);

        public bool IsPresent
            => GetAccessNthBit(0x80);
        #endregion

        #region Flags
        public bool Is32Bit
        {
            get => GetFlagNthBit(0x40);
            set => SetFlagNthBit(0x40, value);
        }

        public bool Granularity
            => GetFlagNthBit(0x80);
        #endregion
    }
}
