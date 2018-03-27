namespace X86Sharp
{
    public partial class VM
    {
        [Instruction(InstructionType.ADD, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
            0x8000, 0x8100, 0x8300)]
        public void Add(Operand dest, Operand source)
        {
            dest.Value += source.Value;

            Eflags.SF = (dest.Value & 0x80000000) == 0x80000000;
            Eflags.ZF = dest.Value == 0;
            Eflags.CF = dest.Value < source.Value;
            Eflags.PF = (((((byte)dest.Value * 0x0101010101010101UL) & 0x8040201008040201UL) % 0x1FF) & 1) == 0;

            Eflags.OF = false;
            try
            {
                var res = checked(dest.Value + source.Value);
            }
            catch (System.OverflowException)
            {
                Eflags.OF = true;
            }
        }
    }
}
