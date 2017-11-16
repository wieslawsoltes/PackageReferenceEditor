
namespace PackageReferenceEditor
{
    public class Feed : NotifyObject
    {
        private string _name;
        private string _uri;

        public string Name
        {
            get => _name;
            set => Update(ref _name, value);
        }

        public string Uri
        {
            get => _uri;
            set => Update(ref _uri, value);
        }

        public Feed(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }
    }
}
