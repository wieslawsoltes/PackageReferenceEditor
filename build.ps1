dotnet restore src/PackageReferenceEditor
dotnet build src/PackageReferenceEditor/PackageReferenceEditor.csproj -c Release -f netstandard2.0
dotnet pack src/PackageReferenceEditor/PackageReferenceEditor.csproj -c Release

dotnet restore src/PackageReferenceEditor.Avalonia
dotnet build src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0

dotnet publish src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0 -r win7-x64 -o bin/win7-x64
Copy-Item "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Redist\MSVC\14.15.26706\x64\Microsoft.VC141.CRT\msvcp140.dll" src/PackageReferenceEditor.Avalonia/bin/win7-x64
Copy-Item "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Redist\MSVC\14.15.26706\x64\Microsoft.VC141.CRT\vcruntime140.dll" src/PackageReferenceEditor.Avalonia/bin/win7-x64
& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\14.15.26726\bin\HostX86\x86\editbin.exe" /subsystem:windows src/PackageReferenceEditor.Avalonia/bin/win7-x64/PackageReferenceEditor.Avalonia.exe

dotnet publish src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0 -r ubuntu.14.04-x64 -o bin/ubuntu.14.04-x64
dotnet publish src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0 -r osx.10.12-x64 -o bin/osx.10.12-x64
