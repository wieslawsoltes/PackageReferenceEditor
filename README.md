# PackageReferenceEditor

[![Build status](https://ci.appveyor.com/api/projects/status/42j9f6aylrv36ufs/branch/master?svg=true)](https://ci.appveyor.com/project/wieslawsoltes/packagereferenceeditor/branch/master)
[![Build Status](https://travis-ci.org/wieslawsoltes/PackageReferenceEditor.svg?branch=0.0.1)](https://travis-ci.org/wieslawsoltes/PackageReferenceEditor)
[![CircleCI](https://circleci.com/gh/wieslawsoltes/PackageReferenceEditor/tree/master.svg?style=svg)](https://circleci.com/gh/wieslawsoltes/PackageReferenceEditor/tree/master)

[![NuGet](https://img.shields.io/nuget/v/PackageReferenceEditor.svg)](https://www.nuget.org/packages/PackageReferenceEditor)

MSBuild, csproj and props package reference editor.

## Screenshots

![](images/Avalonia.png)

## NuGet

PackageReferenceEditor is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/PackageReferenceEditor/) and install the package like this:

`Install-Package PackageReferenceEditor`

### NuGet Packages

* [PackageReferenceEditor](https://www.nuget.org/packages/PackageReferenceEditor/).

### Package Sources

* https://api.nuget.org/v3/index.json

## Example Usage with Cake build scripts

Same code can be used in C# programs by removing `#addin` directive and installing package from NuGet.

### Print package versions
```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.3"

using PackageReferenceEditor;

Updater.FindReferences("./build", "*.props", new string[] { }).PrintVersions();
Updater.FindReferences("./", "*.csproj", new string[] { }).PrintVersions();	
```

### Validate package versions

```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.3"

using PackageReferenceEditor;

Updater.FindReferences("./build", "*.props", new string[] { }).ValidateVersions();
Updater.FindReferences("./", "*.csproj", new string[] { }).ValidateVersions();
```

### Update package version

```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.3"

using PackageReferenceEditor;

Updater.FindReferences("./build", "*.props", new string[] { }).UpdateVersions("Newtonsoft.Json", "10.0.3");
```

### Get package versions
```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.3"

using PackageReferenceEditor;

var result = Updater.FindReferences("./build", "*.props", new string[] { });
result.ValidateVersions();
var version = result.GroupedReferences["Newtonsoft.Json"].FirstOrDefault().Version;
Information("Newtonsoft.Json package version: {0}", version);
```

## Resources

* [GitHub source code repository.](https://github.com/wieslawsoltes/PackageReferenceEditor)

## License

PackageReferenceEditor is licensed under the [MIT license](LICENSE.TXT).
