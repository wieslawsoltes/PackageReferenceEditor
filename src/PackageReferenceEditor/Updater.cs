using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace PackageReferenceEditor
{
    public static class Updater
    {
        public static void FindReferences(string fileName, IList<PackageReference> references, XmlDocument document)
        {
            XmlNodeList packageReferences = default;

            if (document.DocumentElement.Attributes["xmlns"] != null)
            {
                string xmlns = document.DocumentElement.Attributes["xmlns"].Value;
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
                namespaceManager.AddNamespace("msbuild", xmlns);
                packageReferences = document.SelectNodes("//msbuild:Project//msbuild:ItemGroup//msbuild:PackageReference", namespaceManager);
            }
            else
            {
                packageReferences = document.SelectNodes("//Project//ItemGroup//PackageReference");
            }

            foreach (XmlNode packageReference in packageReferences)
            {
                var include = default(string);
                var version = default(string);

                var includeAttribute = packageReference.Attributes["Include"];
                if (includeAttribute != null)
                {
                    include = includeAttribute.Value;
                }
                else
                {
                    var includes = packageReference.SelectNodes("descendant::Include");
                    if (includes.Count > 0)
                    {
                        include = includes[0].InnerText;
                    }
                    else
                    {
                        Logger.Log($"Missing package reference Include, file: {fileName}");
                        continue;
                    }
                }

                var versionAttribute = packageReference.Attributes["Version"];
                if (versionAttribute != null)
                {
                    version = versionAttribute.Value;
                }
                else
                {
                    var versions = packageReference.SelectNodes("descendant::Version");
                    if (versions.Count > 0)
                    {
                        version = versions[0].InnerText;
                    }
                    else
                    {
                        Logger.Log($"Missing package reference Version for {include}, file: {fileName}");
                        continue;
                    }
                }

                var pr = new PackageReference()
                {
                    Name = include,
                    Version = version,
                    FileName = fileName,
                    Document = document,
                    Reference = packageReference,
                    VersionAttribute = versionAttribute
                };

                references.Add(pr);
            }
        }

        public static void FindReferences(string fileName, IList<PackageReference> references, IList<XmlDocument> documents)
        {
            var document = new XmlDocument()
            {
                PreserveWhitespace = true
            };

            document.Load(fileName);
            documents.Add(document);

            FindReferences(fileName, references, document);
        }

        public static void FindReferences(string searchPath, string searchPattern, IEnumerable<string> ignoredPaths, IList<PackageReference> references, IList<XmlDocument> documents)
        {
            Directory.EnumerateFiles(searchPath, searchPattern, SearchOption.AllDirectories).ToList().ForEach(
                fileName =>
                {
                    if (!ignoredPaths.Any(i => NormalizePath(fileName).Contains(NormalizePath(i))))
                    {
                        FindReferences(fileName, references, documents);
                    }
                });

            string NormalizePath(string path)
            {
                return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
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
                Documents = new ObservableCollection<XmlDocument>(),
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
            foreach (var pr in result.GroupedReferences[name])
            {
                if (pr.VersionAttribute != null)
                {
                    if (version != pr.VersionAttribute.Value)
                    {
                        Logger.Log($"Name: {name}, old: {pr.VersionAttribute.Value}, new: {version}, file: {pr.FileName}");
                        pr.VersionAttribute.Value = version;
                        pr.Document.Save(pr.FileName);
                    }
                }
                else
                {
                    var versions = pr.Reference.SelectNodes("descendant::Version");
                    if (versions.Count > 0)
                    {
                        var versionElement = versions[0];
                        if (version != versionElement.InnerText)
                        {
                            Logger.Log($"Name: {name}, old: {versionElement.InnerText}, new: {version}, file: {pr.FileName}");
                            versionElement.InnerText = version;
                            pr.Document.Save(pr.FileName);
                        }
                    }
                }
            }
        }

        public static void UpdateVersions(KeyValuePair<string, IList<PackageReference>> package, bool alwaysUpdate = false)
        {
            Logger.Log($"Updating NuGet package dependencies versions for {package.Key}:");
            foreach (var pr in package.Value)
            {
                if (pr.VersionAttribute != null)
                {
                    if (pr.Version != pr.VersionAttribute.Value || alwaysUpdate == true)
                    {
                        Logger.Log($"Name: {package.Key}, old: {pr.VersionAttribute.Value}, new: {pr.Version}, file: {pr.FileName}");
                        pr.VersionAttribute.Value = pr.Version;
                        pr.Document.Save(pr.FileName);
                    }
                }
                else
                {
                    var versions = pr.Reference.SelectNodes("descendant::Version");
                    if (versions.Count > 0)
                    {
                        var versionElement = versions[0];
                        if (pr.Version != versionElement.InnerText || alwaysUpdate == true)
                        {
                            Logger.Log($"Name: {package.Key}, old: {versionElement.InnerText}, new: {pr.Version}, file: {pr.FileName}");
                            versionElement.InnerText = pr.Version;
                            pr.Document.Save(pr.FileName);
                        }
                    }
                }
            }
        }
    }
}
