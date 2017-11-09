using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MSBuildPropsUpdater
{
    public class Updater
    {
        public IList<XDocument> Documents { get; private set; }
        public IList<PackageReference> References { get; private set; }
        public IEnumerable<IGrouping<string, PackageReference>> GroupedReferences { get; private set; }

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
                });
        }

        public static Updater Create(string searchPath, string searchPattern, IEnumerable<string> ignoredPaths)
        {
            var updater = new Updater()
            {
                Documents = new List<XDocument>(),
                References = new List<PackageReference>()
            };

            FindReferences(searchPath, searchPattern, ignoredPaths, updater.References, updater.Documents);

            updater.GroupedReferences = updater.References.GroupBy(x => x.Name);

            return updater;
        }

        public void PrintVersions()
        {
            Debug.WriteLine("NuGet package dependencies versions:");
            foreach (var package in GroupedReferences)
            {
                Debug.WriteLine($"Package {package.Key} is installed:");
                foreach (var v in package)
                {
                    Debug.WriteLine($"{v.Version}, {v.FileName}");
                }
            }
        }

        public void ValidateVersions()
        {
            Debug.WriteLine("Checking installed NuGet package dependencies versions:");
            foreach (var package in GroupedReferences)
            {
                var packageVersion = package.First().Version;
                bool isValidVersion = package.All(x => x.Version == packageVersion);
                if (!isValidVersion)
                {
                    Debug.WriteLine($"Error: package {package.Key} has multiple versions installed:");
                    foreach (var v in package)
                    {
                        Debug.WriteLine($"{v.Version}, {v.FileName}");
                    }
                    throw new Exception("Detected multiple NuGet package version installed for different projects.");
                }
            };
            Debug.WriteLine("All NuGet package dependencies versions are valid.");
        }
    }
}
