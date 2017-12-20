namespace X86Sharp
{
    public struct Address
    {
        public readonly ushort Segment;
        public readonly uint Base;
        public readonly uint Index;
        public readonly int Scale;
        public readonly int Displacement;

        public int ActualAddress
            => (int)(Segment + Base + Index * Scale + Displacement);

        public Address(ushort seg, uint @base = 0, uint index = 0, int scale = 0, int displacement = 0)
        {
            Segment = seg;
            Base = @base;
            Index = index;
            Scale = scale;
            Displacement = displacement;
        }
    }
}
