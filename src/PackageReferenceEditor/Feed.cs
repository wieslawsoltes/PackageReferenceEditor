using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PackageReferenceEditor;

[DataContract]
public class Feed : ObservableObject
{
    private string _name;
    private string _uri;

    [DataMember]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [DataMember]
    public string Uri
    {
        get => _uri;
        set => SetProperty(ref _uri, value);
    }

    public Feed(string name, string uri)
    {
        _name = name;
        _uri = uri;
    }
}
