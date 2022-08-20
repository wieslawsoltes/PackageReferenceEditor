using System.Runtime.Serialization;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PackageReferenceEditor;

[DataContract]
public class PackageReference : ObservableObject
{
    private string _name = string.Empty;
    private string _version = string.Empty;
    private string _fileName = string.Empty;
    private XmlDocument? _document;
    private XmlNode? _reference;
    private XmlAttribute? _versionAttribute;

    [DataMember]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [DataMember]
    public string Version
    {
        get => _version;
        set => SetProperty(ref _version, value);
    }

    [DataMember]
    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }

    [DataMember]
    public XmlDocument? Document
    {
        get => _document;
        set => SetProperty(ref _document, value);
    }

    [DataMember]
    public XmlNode? Reference
    {
        get => _reference;
        set => SetProperty(ref _reference, value);
    }

    [DataMember]
    public XmlAttribute? VersionAttribute
    {
        get => _versionAttribute;
        set => SetProperty(ref _versionAttribute, value);
    }
}
