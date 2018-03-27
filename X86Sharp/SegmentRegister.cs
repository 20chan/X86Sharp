using System;
using System.Collections.Generic;
using System.Text;

namespace X86Sharp
{
    public struct SegmentRegister
    {
        public uint Selector;
        public GDT GDTEntry;
    }
}
