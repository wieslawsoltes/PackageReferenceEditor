#addin "nuget:?package=PackageReferenceEditor&version=0.0.3"

using PackageReferenceEditor;

var target = Argument("target", "Default");

Task("PrintVersions")
    .Does(() =>
{
    Updater.FindReferences("./build", "*.props", new string[] { }).PrintVersions();
    Updater.FindReferences("./", "*.csproj", new string[] { }).PrintVersions();
});

Task("ValidateVersions")
    .Does(() =>
{
    Updater.FindReferences("./build", "*.props", new string[] { }).ValidateVersions();
    Updater.FindReferences("./", "*.csproj", new string[] { }).ValidateVersions();
});

Task("UpdateVersions")
    .Does(() =>
{
    var result = Updater.FindReferences("./build", "*.props", new string[] { });
    result.UpdateVersions("Avalonia", "0.5.2-build4248-alpha");
    result.UpdateVersions("Avalonia.Desktop", "0.5.2-build4248-alpha");
});

Task("GetVersions")
    .Does(() =>
{
    var result = Updater.FindReferences("./build", "*.props", new string[] { });
    result.ValidateVersions();
    var version = result.GroupedReferences["Newtonsoft.Json"].FirstOrDefault().Version;
    Information("Newtonsoft.Json package version: {0}", version);
});

Task("Default")
  .IsDependentOn("PrintVersions");

RunTarget(target);
