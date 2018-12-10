#addin "nuget:?package=PackageReferenceEditor"

using PackageReferenceEditor;
using System.Linq;

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
    Updater.FindReferences("./build", "*.props", new string[] { }).UpdateVersions("Newtonsoft.Json", "10.0.3");
});

Task("InstalledVersions")
    .Does(() =>
{
    var result = Updater.FindReferences("./build", "*.props", new string[] { });
    result.ValidateVersions();
    var version = result.GroupedReferences["Newtonsoft.Json"].FirstOrDefault().Version;
    Information("Newtonsoft.Json package version: {0}", version);
});

Task("AvailableVersions")
    .Does(() =>
{
    var versions = NuGetApi.GetPackageVersions("https://api.nuget.org/v3/index.json", "Newtonsoft.Json").Result;
    var latestVersion = versions.Reverse().FirstOrDefault();
    Information("Newtonsoft.Json package latest version: {0}", latestVersion);
}

Task("Default")
  .IsDependentOn("PrintVersions");

RunTarget(target);
