namespace X86Sharp
{
    public partial class VM
    {
        [Instruction(InstructionType.Mov, 0x88)]
        public void Mov(ref uint dest, ref uint source)
        {
            dest = source;
        }
    }
}
