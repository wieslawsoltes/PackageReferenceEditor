dotnet restore src/PackageReferenceEditor
dotnet build src/PackageReferenceEditor/PackageReferenceEditor.csproj -c Release -f netstandard2.0
dotnet pack src/PackageReferenceEditor/PackageReferenceEditor.csproj -c Release

dotnet restore src/PackageReferenceEditor.Avalonia
dotnet build src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0

dotnet publish src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0 -r win7-x64 -o bin/win7-x64
dotnet publish src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0 -r ubuntu.14.04-x64 -o bin/ubuntu.14.04-x64
dotnet publish src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0 -r osx.10.12-x64 -o bin/osx.10.12-x64
