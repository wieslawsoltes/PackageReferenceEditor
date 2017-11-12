using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MSBuildPropsUpdater.WPF
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

        public static UpdaterResult FindReferences(string searchPath, string searchPattern, IEnumerable<string> ignoredPaths)
        {
            var updater = new UpdaterResult()
            {
                Documents = new List<XDocument>(),
                References = new List<PackageReference>()
            };

            FindReferences(searchPath, searchPattern, ignoredPaths, updater.References, updater.Documents);

            updater.GroupedReferences = updater.References.GroupBy(x => x.Name).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.ToList());

            return updater;
        }

        public static void PrintVersions(this UpdaterResult result)
        {
            Console.WriteLine("NuGet package dependencies versions:");
            foreach (var package in result.GroupedReferences)
            {
                Console.WriteLine($"Package {package.Key} is installed:");
                foreach (var v in package.Value)
                {
                    Console.WriteLine($"{v.Version}, {v.FileName}");
                }
            }
        }

        public static void ValidateVersions(this UpdaterResult result)
        {
            Console.WriteLine("Checking installed NuGet package dependencies versions:");
            foreach (var package in result.GroupedReferences)
            {
                var packageVersion = package.Value.First().Version;
                bool isValidVersion = package.Value.All(x => x.Version == packageVersion);
                if (!isValidVersion)
                {
                    Console.WriteLine($"Error: package {package.Key} has multiple versions installed:");
                    foreach (var v in package.Value)
                    {
                        Console.WriteLine($"{v.Version}, {v.FileName}");
                    }
                    throw new Exception("Detected multiple NuGet package version installed for different projects.");
                }
            };
            Console.WriteLine("All NuGet package dependencies versions are valid.");
        }
    }
}
