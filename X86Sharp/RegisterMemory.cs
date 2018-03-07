using System.Runtime.InteropServices;

namespace X86Sharp
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct RegisterMemory
    {
        [FieldOffset(0)]
        public uint DWord;
        [FieldOffset(0)]
        public ushort HighWord;
        [FieldOffset(2)]
        public ushort LowWord;
        [FieldOffset(2)]
        public byte HighByte;
        [FieldOffset(3)]
        public byte LowByte;

        public static implicit operator uint(RegisterMemory register)
            => register.DWord;

        public static implicit operator RegisterMemory(uint value)
            => new RegisterMemory() { DWord = value };
    }
}
