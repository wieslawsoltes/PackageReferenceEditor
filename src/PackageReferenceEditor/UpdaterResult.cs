using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using ReactiveUI;

namespace PackageReferenceEditor
{
    [DataContract]
    public class UpdaterResult : ReactiveObject
    {
        private IList<XDocument> _documents;
        private IList<PackageReference> _references;
        private Dictionary<string, IList<PackageReference>> _groupedReferences;

        [DataMember]
        public IList<XDocument> Documents
        {
            get => _documents;
            set => this.RaiseAndSetIfChanged(ref _documents, value);
        }

        [DataMember]
        public IList<PackageReference> References
        {
            get => _references;
            set => this.RaiseAndSetIfChanged(ref _references, value);
        }

        [DataMember]
        public Dictionary<string, IList<PackageReference>> GroupedReferences
        {
            get => _groupedReferences;
            set => this.RaiseAndSetIfChanged(ref _groupedReferences, value);
        }

        public void Reset()
        {
            Documents?.Clear();
            References?.Clear();
            GroupedReferences?.Clear();
        }
    }
}
