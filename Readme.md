# X86Sharp [![Build status](https://ci.appveyor.com/api/projects/status/dgsql1bmt4ur9kci?svg=true)](https://ci.appveyor.com/project/phillyai/x86sharp) [![Test status](http://flauschig.ch/batch.php?type=tests&account=phillyai&slug=X86Sharp)](https://ci.appveyor.com/project/phillyai/x86sharp)

X86 VM written in C#, works on .Net Core 2.0

Separated from project [AssemblySharp](https://github.com/phillyai/AssemblySharp)

## Usage

```csharp
VM vm = new VM();

Address addr = new Address(vm.Segments.SS, displacement: 0);
Span<byte> dword0to4 = vm.Memory.GetValue(addr, size: 4);
var mov = vm.Instruction.MOV;
ref uint refptr = ref dword0to4.NonPortableCast<byte, uint>()[0];
uint num = 0x12345678;

mov(ref refptr, ref num);
vm.Instruction.PUSH(ref vm.Registers.EAX);
```

## [LICENSE](/LICENSE)

The MIT License (MIT) Copyright (c) 2017 phillyai