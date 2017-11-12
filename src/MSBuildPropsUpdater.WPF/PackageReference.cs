using System.Xml.Linq;

namespace MSBuildPropsUpdater.WPF
{
    public class PackageReference
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string FileName { get; set; }
        public XDocument Document { get; set; }
        public XElement Reference { get; set; }
        public XAttribute VersionAttribute { get; set; }
    }
}
