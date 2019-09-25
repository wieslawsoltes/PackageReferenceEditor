using System.Runtime.Serialization;
using ReactiveUI;

namespace PackageReferenceEditor
{
    [DataContract]
    public class Feed : ReactiveObject
    {
        private string _name;
        private string _uri;

        [DataMember]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        [DataMember]
        public string Uri
        {
            get => _uri;
            set => this.RaiseAndSetIfChanged(ref _uri, value);
        }

        public Feed(string name, string uri)
        {
            _name = name;
            _uri = uri;
        }
    }
}
