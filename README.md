# PackageReferenceEditor

[![Build status](https://ci.appveyor.com/api/projects/status/v654ae52b4bh7y5g/branch/master?svg=true)](https://ci.appveyor.com/project/wieslawsoltes/msbuildpropsupdater/branch/master)
[![Build Status](https://travis-ci.org/wieslawsoltes/MSBuildPropsUpdater.svg?branch=master)](https://travis-ci.org/wieslawsoltes/MSBuildPropsUpdater)
[![CircleCI](https://circleci.com/gh/wieslawsoltes/MSBuildPropsUpdater/tree/master.svg?style=svg)](https://circleci.com/gh/wieslawsoltes/MSBuildPropsUpdater/tree/master)

[![NuGet](https://img.shields.io/nuget/v/PackageReferenceEditor.svg)](https://www.nuget.org/packages/PackageReferenceEditor)

MSBuild, csproj and props package reference editor.

## NuGet

PackageReferenceEditor is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/PackageReferenceEditor/) and install the package like this:

`Install-Package PackageReferenceEditor`

### NuGet Packages

* [PackageReferenceEditor](https://www.nuget.org/packages/PackageReferenceEditor/).

### Package Sources

* https://api.nuget.org/v3/index.json

## Example Usage with Cake build scripts

### Print package versions
```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.1"

using PackageReferenceEditor;

var result = Updater.FindReferences(@"C:\GitHub\", "*.props", new string[] { });

result.PrintVersions();		
```

### Validate package versions

```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.1"

using PackageReferenceEditor;

var result = Updater.FindReferences(@"C:\GitHub\", "*.props", new string[] { });
	
result.ValidateVersions();
```

### Update package version

```C#
#addin "nuget:?package=PackageReferenceEditor&version=0.0.1"

using PackageReferenceEditor;

var result = Updater.FindReferences(@"C:\GitHub\", "*.props", new string[] { });

result.UpdateVersions("Newtonsoft.Json", "10.0.3");
```

Same code can be used in C# programs by removing `#addin` directive and installing package from NuGet.

## Resources

* [GitHub source code repository.](https://github.com/wieslawsoltes/PackageReferenceEditor)

## License

PackageReferenceEditor is licensed under the [MIT license](LICENSE.TXT).
