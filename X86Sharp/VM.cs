using System;
using System.Collections.Generic;
using System.Reflection;

namespace X86Sharp
{
    /// <summary>
    /// X86 Virtual Machine
    /// </summary>
    public partial class VM
    {
        #region Registers
        public class RegisterManager
        {
            private VM _vm;
            public ref uint EAX => ref _vm.EAX.DWord;
            public ref uint EBX => ref _vm.EBX.DWord;
            public ref uint ECX => ref _vm.ECX.DWord;
            public ref uint EDX => ref _vm.EDX.DWord;
            public ref uint ESP => ref _vm.ESP.DWord;
            public ref uint EBP => ref _vm.EBP.DWord;
            public ref uint ESI => ref _vm.ESI.DWord;
            public ref uint EDI => ref _vm.EDI.DWord;
            public ref uint EIP => ref _vm.EIP.DWord;

            public ref ushort AX => ref _vm.EAX.LowWord;
            public ref ushort BX => ref _vm.EBX.LowWord;
            public ref ushort CX => ref _vm.ECX.LowWord;
            public ref ushort DX => ref _vm.EDX.LowWord;
            public ref ushort SP => ref _vm.ESP.LowWord;
            public ref ushort BP => ref _vm.EBP.LowWord;
            public ref ushort SI => ref _vm.ESI.LowWord;
            public ref ushort DI => ref _vm.EDI.LowWord;
            public ref ushort IP => ref _vm.EIP.LowWord;

            public ref byte AH => ref _vm.EAX.HighByte;
            public ref byte AL => ref _vm.EAX.LowByte;
            public ref byte BH => ref _vm.EBX.HighByte;
            public ref byte BL => ref _vm.EBX.LowByte;
            public ref byte CH => ref _vm.ECX.HighByte;
            public ref byte CL => ref _vm.ECX.LowByte;
            public ref byte DH => ref _vm.EDX.HighByte;
            public ref byte DL => ref _vm.EDX.LowByte;

            public RegisterManager(VM vm)
            {
                _vm = vm;
            }
        }
        public RegisterManager Registers { get; private set; }
        private RegisterMemory EAX, EBX, ECX, EDX;
        private RegisterMemory ESI, EDI, ESP, EBP, EIP;
        #endregion

        #region Segments
        public class SegmentManager
        {
            private VM _vm;
            public ref ushort CS => ref _vm.CS;
            public ref ushort DS => ref _vm.DS;
            public ref ushort SS => ref _vm.SS;
            public ref ushort ES => ref _vm.ES;
            public ref ushort FS => ref _vm.FS;
            public ref ushort GS => ref _vm.GS;

            public SegmentManager(VM vm)
            {
                _vm = vm;
            }
        }
        public SegmentManager Segments { get; private set; }

        private ushort CS, DS, SS, ES, FS, GS;

        #endregion

        #region EFLAGs
        public class EflagsManager
        {
            private VM _vm;

            public ref bool CF => ref _vm.eflag.CF;
            public ref bool PF => ref _vm.eflag.PF;
            public ref bool AF => ref _vm.eflag.AF;
            public ref bool ZF => ref _vm.eflag.ZF;
            public ref bool SF => ref _vm.eflag.SF;
            public ref bool TF => ref _vm.eflag.TF;
            public ref bool IF => ref _vm.eflag.IF;
            public ref bool DF => ref _vm.eflag.DF;
            public ref bool OF => ref _vm.eflag.OF;
            public ref bool IOPL => ref _vm.eflag.IOPL;
            public ref bool IOPL2 => ref _vm.eflag.IOPL2;
            public ref bool NT => ref _vm.eflag.NT;
            public ref bool RF => ref _vm.eflag.RF;
            public ref bool VM => ref _vm.eflag.VM;
            public ref bool AC => ref _vm.eflag.AC;
            public ref bool VIF => ref _vm.eflag.VIF;
            public ref bool VIP => ref _vm.eflag.VIP;
            public ref bool ID => ref _vm.eflag.ID;

            public EflagsManager(VM vm)
            {
                _vm = vm;
            }
        }
        public EflagsManager Eflags { get; private set; }
        private EFlags eflag;
        #endregion

        #region Instructions
        public class InstructionManager
        {
            private VM _vm;

            public InstructionManager(VM vm)
            {
                _vm = vm;
            }

            public T GetInstruction<T>(uint opcode) where T : class
                => GetInstruction(opcode) as T;

            public Delegate GetInstruction(uint opcode)
            {
                if (_vm._instructions0args.ContainsKey(opcode))
                    return _vm._instructions0args[opcode];
                else if (_vm._instructions1arg.ContainsKey(opcode))
                    return _vm._instructions1arg[opcode];
                else if (_vm._instructions2args.ContainsKey(opcode))
                    return _vm._instructions2args[opcode];
                else if (_vm._instructions3args.ContainsKey(opcode))
                    return _vm._instructions3args[opcode];
                else
                    throw new KeyNotFoundException("No instruction that matches opcode");
            }

            public T GetInstructionFromType<T>(InstructionType type) where T : class
                => GetInstructionFromType(type) as T;

            public Delegate GetInstructionFromType(InstructionType type)
                => _vm._instructionsFromType[type];
        }

        public InstructionManager Instructions { get; private set; }
        private Dictionary<InstructionType, Delegate> _instructionsFromType;
        private Dictionary<uint, InstructionCallback0args> _instructions0args;
        private Dictionary<uint, InstructionCallback1arg> _instructions1arg;
        private Dictionary<uint, InstructionCallback2args> _instructions2args;
        private Dictionary<uint, InstructionCallback3args> _instructions3args;
        #endregion

        #region Memory
        public class MemoryManager
        {
            private VM _vm;

            public MemoryManager(VM vm)
            {
                _vm = vm;
            }

            public Span<byte> GetValue(Address address)
                => new Span<byte>(
                    _vm._memory,
                    _vm._memory.Length - address.ActualAddress(_vm),
                    (int)address.Size);

            public ReadOnlySpan<byte> GetReadonlyValue(Address address, int size)
                => new ReadOnlySpan<byte>(
                    _vm._memory,
                    _vm._memory.Length - address.ActualAddress(_vm),
                    size);
        }
        public MemoryManager Memory { get; private set; }
        readonly int STACK_SIZE = 1024;
        byte[] _memory;
        #endregion

        public VM()
        {
            Registers = new RegisterManager(this);
            Segments = new SegmentManager(this);
            Eflags = new EflagsManager(this);
            Instructions = new InstructionManager(this);
            Memory = new MemoryManager(this);
            _instructionsFromType = new Dictionary<InstructionType, Delegate>();
            _instructions0args = new Dictionary<uint, InstructionCallback0args>();
            _instructions1arg = new Dictionary<uint, InstructionCallback1arg>();
            _instructions2args = new Dictionary<uint, InstructionCallback2args>();
            _instructions3args = new Dictionary<uint, InstructionCallback3args>();
            Reset();
            LoadInstructions();
        }

        public void Reset()
        {
            _memory = new byte[STACK_SIZE * 1024];
            EAX = 0;
            EBX = 0;
            ECX = 0;
            EDX = 0;
            ESI = 0;
            EDI = 0;
            ESP = 0;
            EBP = 0;
            EIP = 0;

            CS = 0;
            DS = 0;
            SS = (ushort)_memory.Length;
            ES = 0;
            FS = 0;
            GS = 0;

            eflag = 0;
        }

        private void LoadInstructions()
        {
            foreach (MethodInfo method in (typeof(VM)).GetMethods())
            {
                var inst = method.GetCustomAttribute<Instruction>(true);
                if (inst == null) continue;

                Delegate del;
                switch (method.GetParameters().Length)
                {
                    case 0:
                        del = Delegate.CreateDelegate(typeof(InstructionCallback0args), this, method);
                        foreach (var opcode in inst.OpCodes)
                            _instructions0args.Add(opcode, (InstructionCallback0args)del);
                        break;
                    case 1:
                        del = Delegate.CreateDelegate(typeof(InstructionCallback1arg), this, method);
                        foreach (var opcode in inst.OpCodes)
                            _instructions1arg.Add(opcode, (InstructionCallback1arg)del);
                        break;
                    case 2:
                        del = Delegate.CreateDelegate(typeof(InstructionCallback2args), this, method);
                        foreach (var opcode in inst.OpCodes)
                            _instructions2args.Add(opcode, (InstructionCallback2args)del);
                        break;
                    case 3:
                        del = Delegate.CreateDelegate(typeof(InstructionCallback3args), this, method);
                        foreach (var opcode in inst.OpCodes)
                            _instructions3args.Add(opcode, (InstructionCallback3args)del);
                        break;
                    default:
                        throw new Exception("Too many instrucment parameters");
                }
                if (!_instructionsFromType.ContainsKey(inst.InstructionType))
                    _instructionsFromType.Add(inst.InstructionType, del);
            }
        }

        public object ExecuteFunction(byte[] codes, Type funcType, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCode(params byte[] codes)
        {
            throw new NotImplementedException();
        }

        public object this[RegisterType reg]
        {
            get
            {
                switch (reg)
                {
                    case RegisterType.EAX:
                        return Registers.EAX;
                    case RegisterType.EBX:
                        return Registers.EBX;
                    case RegisterType.ECX:
                        return Registers.ECX;
                    case RegisterType.EDX:
                        return Registers.EDX;
                    case RegisterType.ESP:
                        return Registers.ESP;
                    case RegisterType.EBP:
                        return Registers.EBP;
                    case RegisterType.ESI:
                        return Registers.ESI;
                    case RegisterType.EDI:
                        return Registers.EDI;
                    case RegisterType.EIP:
                        return Registers.EIP;
                    case RegisterType.AX:
                        return Registers.AX;
                    case RegisterType.BX:
                        return Registers.BX;
                    case RegisterType.CX:
                        return Registers.CX;
                    case RegisterType.DX:
                        return Registers.DX;
                    case RegisterType.SP:
                        return Registers.SP;
                    case RegisterType.BP:
                        return Registers.BP;
                    case RegisterType.SI:
                        return Registers.SI;
                    case RegisterType.DI:
                        return Registers.DI;
                    case RegisterType.IP:
                        return Registers.IP;
                    case RegisterType.AH:
                        return Registers.AH;
                    case RegisterType.AL:
                        return Registers.AL;
                    case RegisterType.BH:
                        return Registers.BH;
                    case RegisterType.BL:
                        return Registers.BL;
                    case RegisterType.CH:
                        return Registers.CH;
                    case RegisterType.CL:
                        return Registers.CL;
                    case RegisterType.DH:
                        return Registers.DH;
                    case RegisterType.DL:
                        return Registers.DL;
                    default:
                        throw new IndexOutOfRangeException("No such register type");
                }
            }
            set
            {
                if (RegisterType.EAX <= reg && reg <= RegisterType.EIP)
                {
                    if (!(value is uint))
                        throw new Exception($"value of {reg} is uint, but got {value.GetType()}");
                }
                else if (RegisterType.AX <= reg && reg <= RegisterType.IP)
                {
                    if (!(value is ushort))
                        throw new Exception($"value of {reg} is uint, but got {value.GetType()}");
                }
                if (RegisterType.AX <= reg && reg <= RegisterType.DL)
                {
                    if (!(value is byte))
                        throw new Exception($"value of {reg} is uint, but got {value.GetType()}");
                }
                switch (reg)
                {
                    case RegisterType.EAX:
                        Registers.EAX = (uint)value;
                        return;
                    case RegisterType.EBX:
                        Registers.EBX = (uint)value;
                        return;
                    case RegisterType.ECX:
                        Registers.ECX = (uint)value;
                        return;
                    case RegisterType.EDX:
                        Registers.EDX = (uint)value;
                        return;
                    case RegisterType.ESP:
                        Registers.ESP = (uint)value;
                        return;
                    case RegisterType.EBP:
                        Registers.EBP = (uint)value;
                        return;
                    case RegisterType.ESI:
                        Registers.ESI = (uint)value;
                        return;
                    case RegisterType.EDI:
                        Registers.EDI = (uint)value;
                        return;
                    case RegisterType.EIP:
                        Registers.EIP = (uint)value;
                        return;
                    case RegisterType.AX:
                        Registers.AX = (ushort)value;
                        return;
                    case RegisterType.BX:
                        Registers.BX = (ushort)value;
                        return;
                    case RegisterType.CX:
                        Registers.CX = (ushort)value;
                        return;
                    case RegisterType.DX:
                        Registers.DX = (ushort)value;
                        return;
                    case RegisterType.SP:
                        Registers.SP = (ushort)value;
                        return;
                    case RegisterType.BP:
                        Registers.BP = (ushort)value;
                        return;
                    case RegisterType.SI:
                        Registers.SI = (ushort)value;
                        return;
                    case RegisterType.DI:
                        Registers.DI = (ushort)value;
                        return;
                    case RegisterType.IP:
                        Registers.IP = (ushort)value;
                        return;
                    case RegisterType.AH:
                        Registers.AH = (byte)value;
                        return;
                    case RegisterType.AL:
                        Registers.AL = (byte)value;
                        return;
                    case RegisterType.BH:
                        Registers.BH = (byte)value;
                        return;
                    case RegisterType.BL:
                        Registers.BL = (byte)value;
                        return;
                    case RegisterType.CH:
                        Registers.CH = (byte)value;
                        return;
                    case RegisterType.CL:
                        Registers.CL = (byte)value;
                        return;
                    case RegisterType.DH:
                        Registers.DH = (byte)value;
                        return;
                    case RegisterType.DL:
                        Registers.DL = (byte)value;
                        return;
                }
            }
        }

        public ushort this[SegmentType seg]
        {
            get
            {
                switch (seg)
                {
                    case SegmentType.CS:
                        return Segments.CS;
                    case SegmentType.DS:
                        return Segments.DS;
                    case SegmentType.SS:
                        return Segments.SS;
                    case SegmentType.ES:
                        return Segments.ES;
                    case SegmentType.FS:
                        return Segments.FS;
                    case SegmentType.GS:
                        return Segments.GS;
                    default:
                        throw new IndexOutOfRangeException("No such register type");
                }
            }
            private set
            {
                switch (seg)
                {
                    case SegmentType.CS:
                        Segments.CS = value;
                        break;
                    case SegmentType.DS:
                        Segments.DS = value;
                        break;
                    case SegmentType.SS:
                        Segments.SS = value;
                        break;
                    case SegmentType.ES:
                        Segments.ES = value;
                        break;
                    case SegmentType.FS:
                        Segments.FS = value;
                        break;
                    case SegmentType.GS:
                        Segments.GS = value;
                        break;
                }
            }
        }
    }
}
