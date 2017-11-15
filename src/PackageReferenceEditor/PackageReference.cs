using System.Xml.Linq;

namespace PackageReferenceEditor
{
    public class PackageReference : NotifyObject
    {
        private string _name;
        private string _version;
        private string _fileName;
        private XDocument _document;
        private XElement _reference;
        private XAttribute _versionAttribute;

        public string Name
        {
            get => _name;
            set => Update(ref _name, value);
        }

        public string Version
        {
            get => _version;
            set => Update(ref _version, value);
        }

        public string FileName
        {
            get => _fileName;
            set => Update(ref _fileName, value);
        }

        public XDocument Document
        {
            get => _document;
            set => Update(ref _document, value);
        }

        public XElement Reference
        {
            get => _reference;
            set => Update(ref _reference, value);
        }

        public XAttribute VersionAttribute
        {
            get => _versionAttribute;
            set => Update(ref _versionAttribute, value);
        }
    }
}
