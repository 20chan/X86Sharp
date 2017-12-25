# X86Sharp

[![Build](https://img.shields.io/appveyor/ci/phillyai/X86Sharp/master.svg)](https://ci.appveyor.com/project/phillyai/x86sharp)
[![Tests](https://img.shields.io/appveyor/tests/phillyai/X86Sharp/master.svg)](https://ci.appveyor.com/project/phillyai/x86sharp/build/tests)

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