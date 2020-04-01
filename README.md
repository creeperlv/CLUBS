# CLUBS - **C**reeper **L**v's **U**niversal **B**uild **S**ystem

This program is designed for solving the problem that compiling the same project will use different shell script.

## Features

* Different configurations.
* Multiple tasks.
* Platform-Specific task.
* Embeded TCC for Windows.

## Compiling the project

### On Windows

You need to download latest daily-built .Net 5. Then, for normal .Net version, you just need to type `dotnet build`. To build native code version, you need to use `dotnet publish -c:Release -r:win-x64`.

### On Linux

I cannot figure out an easily way install daily-built .Net 5. However, once the higher version (higher than `preview.1`) is available, you can use the same command as it in Windows.

Before you start to build this project, you need to modify the refered project named `CLUBS.Tools.Windows.csproj` in `CLUBS.csproj` to `CLUBS.Tools.Linux.csproj`.

### On macOS

I do not possess any computer with macOS, so I cannot give out any insturction.

## License

The MIT License

## Third-Party 

### TCC

`TCC` is short for `Tiny-C Compiler`, included in Windows version for simple C compilation task.

`TCC` is licensed under `LGPL 2`

### Microsoft.DotNet.ILCompiler
`Microsoft.DotNet.ILCompiler` is used to compile IL to native code. By using this, the final binary size is sharply reduced and the launch performance is much better.

`Microsoft.DotNet.ILCompiler` is licensed under `MIT License`
