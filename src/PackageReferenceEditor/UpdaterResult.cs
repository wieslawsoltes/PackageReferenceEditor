using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PackageReferenceEditor;

[DataContract]
public class UpdaterResult : ObservableObject
{
    private IList<XmlDocument>? _documents;
    private IList<PackageReference>? _references;
    private Dictionary<string, IList<PackageReference>>? _groupedReferences;

    [DataMember]
    public IList<XmlDocument>? Documents
    {
        get => _documents;
        set => SetProperty(ref _documents, value);
    }

    [DataMember]
    public IList<PackageReference>? References
    {
        get => _references;
        set => SetProperty(ref _references, value);
    }

    [DataMember]
    public Dictionary<string, IList<PackageReference>>? GroupedReferences
    {
        get => _groupedReferences;
        set => SetProperty(ref _groupedReferences, value);
    }

    public void Reset()
    {
        Documents?.Clear();
        References?.Clear();
        GroupedReferences?.Clear();
    }
}
