using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PackageReferenceEditor
{
    public static class Updater
    {
        public static string NormalizePath(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
        }

        public static void FindReferences(string searchPath, string searchPattern, IEnumerable<string> ignoredPaths, IList<PackageReference> references, IList<XDocument> documents)
        {
            Directory.EnumerateFiles(searchPath, searchPattern, SearchOption.AllDirectories).ToList().ForEach(
                fileName =>
                {
                    if (!ignoredPaths.Any(i => NormalizePath(fileName).Contains(NormalizePath(i))))
                    {
                        FindReferences(fileName, references, documents);
                    }
                });
        }

        public static void FindReferences(string fileName, IList<PackageReference> references, IList<XDocument> documents)
        {
            var document = XDocument.Load(fileName);
            documents.Add(document);
            foreach (var reference in document.Descendants().Where(x => x.Name.LocalName == "PackageReference"))
            {
                var name = reference.Attribute("Include").Value;
                var versionAttribute = reference.Attribute("Version");
                var version = versionAttribute != null ? versionAttribute.Value : reference.Elements().First(x => x.Name.LocalName == "Version").Value;
                var pr = new PackageReference()
                {
                    Name = name,
                    Version = version,
                    FileName = fileName,
                    Document = document,
                    Reference = reference,
                    VersionAttribute = versionAttribute
                };
                references.Add(pr);
            }
        }

        public static UpdaterResult FindReferences(this UpdaterResult updater, string searchPath, string searchPattern, IEnumerable<string> ignoredPaths)
        {
            FindReferences(searchPath, searchPattern, ignoredPaths, updater.References, updater.Documents);

            updater.GroupedReferences = updater.References
                .GroupBy(x => x.Name)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => (IList<PackageReference>)new ObservableCollection<PackageReference>(x));

            return updater;
        }

        public static UpdaterResult FindReferences(string searchPath, string searchPattern, IEnumerable<string> ignoredPaths)
        {
            return new UpdaterResult()
            {
                Documents = new ObservableCollection<XDocument>(),
                References = new ObservableCollection<PackageReference>()
            }
            .FindReferences(searchPath, searchPattern, ignoredPaths);
        }

        public static void PrintVersions(this UpdaterResult result)
        {
            Logger.Log("NuGet package dependencies versions:");
            foreach (var package in result.GroupedReferences)
            {
                Logger.Log($"Package {package.Key} is installed:");
                foreach (var v in package.Value)
                {
                    Logger.Log($"{v.Version}, {v.FileName}");
                }
            }
        }

        public static void ValidateVersions(this UpdaterResult result)
        {
            Logger.Log("Checking installed NuGet package dependencies versions:");
            foreach (var package in result.GroupedReferences)
            {
                var packageVersion = package.Value.First().Version;
                bool isValidVersion = package.Value.All(x => x.Version == packageVersion);
                if (!isValidVersion)
                {
                    Logger.Log($"Error: package {package.Key} has multiple versions installed:");
                    foreach (var v in package.Value)
                    {
                        Logger.Log($"{v.Version}, {v.FileName}");
                    }
                    throw new Exception("Detected multiple NuGet package version installed for different projects.");
                }
            };
            Logger.Log("All NuGet package dependencies versions are valid.");
        }

        public static void UpdateVersions(this UpdaterResult result, string name, string version)
        {
            Logger.Log("Updating NuGet package dependencies versions:");
            foreach (var v in result.GroupedReferences[name])
            {
                if (v.VersionAttribute != null)
                {
                    if (version != v.VersionAttribute.Value)
                    {
                        Logger.Log($"Name: {name}, old: {v.VersionAttribute.Value}, new: {version}, file: {v.FileName}");
                        v.VersionAttribute.Value = version;
                        v.Document.Save(v.FileName);
                    }
                }
                else
                {
                    var versionElement = v.Reference.Elements().First(x => x.Name.LocalName == "Version");
                    if (versionElement != null)
                    {
                        if (version != versionElement.Value)
                        {
                            Logger.Log($"Name: {name}, old: {versionElement.Value}, new: {version}, file: {v.FileName}");
                            versionElement.Value = version;
                            v.Document.Save(v.FileName);
                        }
                    }
                }
            }
        }

        public static void UpdateVersions(KeyValuePair<string, IList<PackageReference>> package, bool alwaysUpdate = false)
        {
            Logger.Log($"Updating NuGet package dependencies versions for {package.Key}:");
            foreach (var v in package.Value)
            {
                if (v.VersionAttribute != null)
                {
                    if (v.Version != v.VersionAttribute.Value || alwaysUpdate == true)
                    {
                        Logger.Log($"Name: {package.Key}, old: {v.VersionAttribute.Value}, new: {v.Version}, file: {v.FileName}");
                        v.VersionAttribute.Value = v.Version;
                        v.Document.Save(v.FileName);
                    }
                }
                else
                {
                    var versionElement = v.Reference.Elements().First(x => x.Name.LocalName == "Version");
                    if (versionElement != null)
                    {
                        if (v.Version != versionElement.Value || alwaysUpdate == true)
                        {
                            Logger.Log($"Name: {package.Key}, old: {versionElement.Value}, new: {v.Version}, file: {v.FileName}");
                            versionElement.Value = v.Version;
                            v.Document.Save(v.FileName);
                        }
                    }
                }
            }
        }
    }
}
