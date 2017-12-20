using System;

namespace X86Sharp
{
    public partial class VM
    {
        [Instruction(InstructionType.Mov, 0x88)]
        public void Mov(ref uint dest, ref uint source)
        {
            dest = source;
        }

        [Instruction(InstructionType.Push, 0x06, 0x0e, 0x16, 0x1e, 0x68, 0x6a,
            0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57,
            0x0fa0, 0x0fa8, 0xff06)]
        public void Push(ref uint dest)
        {
            Registers.ESP -= 4;
            var bytes = BitConverter.GetBytes(dest).AsSpan();
            var mem = Memory.GetValue(new Address(Segments.SS, Registers.ESP, 0, 1, 0), 4);
            bytes.CopyTo(mem);
        }
    }
}
