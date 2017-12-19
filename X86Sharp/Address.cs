using System;
using System.Collections.Generic;
using System.Text;

namespace X86Sharp
{
    public struct Address
    {
        public readonly ushort Segment;
        public readonly uint Base;
        public readonly uint Index;
        public readonly int Scale;
        public readonly int Displacement;

        public Address(int displacement) : this(0, 0, 0, 1, displacement) { }
        public Address(uint @base) : this(0, @base, 0, 1, 0) { }
        public Address(uint @base, int displacement) : this(0, @base, 0, 1, displacement) { }
        public Address(uint index, int scale, int displacement) : this(0, 0, index, scale, displacement) { }
        public Address(uint @base, uint index, int displacement) : this(0, @base, index, 1, displacement) { }

        public Address(ushort seg, uint @base, uint index, int scale, int displacement)
        {
            Segment = seg;
            Base = @base;
            Index = index;
            Scale = scale;
            Displacement = displacement;
        }
    }
}
