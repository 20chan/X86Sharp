namespace X86Sharp
{
    public struct Address
    {
        public readonly SegmentType Segment;
        public readonly uint Base;
        public readonly uint Index;
        public readonly int Scale;
        public readonly int Displacement;
        public readonly uint Size;

        public int ActualAddress(VM vm)
            => (int)(vm[Segment]+ Base + Index * Scale + Displacement);

        public Address(SegmentType seg, uint size, uint @base = 0, uint index = 0, int scale = 0, int displacement = 0)
        {
            Segment = seg;
            Base = @base;
            Index = index;
            Scale = scale;
            Displacement = displacement;
            Size = size;
        }
    }
}
