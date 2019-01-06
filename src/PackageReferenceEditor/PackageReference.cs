using System.Runtime.Serialization;
using System.Xml;
using ReactiveUI;

namespace PackageReferenceEditor
{
    [DataContract]
    public class PackageReference : ReactiveObject
    {
        private string _name;
        private string _version;
        private string _fileName;
        private XmlDocument _document;
        private XmlNode _reference;
        private XmlAttribute _versionAttribute;

        [DataMember]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        [DataMember]
        public string Version
        {
            get => _version;
            set => this.RaiseAndSetIfChanged(ref _version, value);
        }

        [DataMember]
        public string FileName
        {
            get => _fileName;
            set => this.RaiseAndSetIfChanged(ref _fileName, value);
        }

        [DataMember]
        public XmlDocument Document
        {
            get => _document;
            set => this.RaiseAndSetIfChanged(ref _document, value);
        }

        [DataMember]
        public XmlNode Reference
        {
            get => _reference;
            set => this.RaiseAndSetIfChanged(ref _reference, value);
        }

        [DataMember]
        public XmlAttribute VersionAttribute
        {
            get => _versionAttribute;
            set => this.RaiseAndSetIfChanged(ref _versionAttribute, value);
        }
    }
}
