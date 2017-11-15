#addin "nuget:?package=PackageReferenceEditor&version=0.0.3"

using PackageReferenceEditor;

var target = Argument("target", "Default");

Task("PrintVersions")
    .Does(() =>
{
    var result = Updater.FindReferences(@"C:\DOWNLOADS\GitHub", "*.props", new string[] { });
    result.PrintVersions();	
});

Task("ValidateVersions")
    .Does(() =>
{
    var result = Updater.FindReferences(@"C:\DOWNLOADS\GitHub", "*.props", new string[] { });	
    result.ValidateVersions();
});

Task("UpdateVersions")
    .Does(() =>
{
    var result = Updater.FindReferences(@"C:\DOWNLOADS\GitHub", "*.props", new string[] { });
    result.UpdateVersions("Avalonia", "0.5.2-build4248-alpha");
    result.UpdateVersions("Avalonia.Desktop", "0.5.2-build4248-alpha");
});

Task("Default")
  .IsDependentOn("PrintVersions");

RunTarget(target);
