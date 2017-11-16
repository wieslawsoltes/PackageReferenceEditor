using System.Collections.Generic;
using System.Xml.Linq;

namespace PackageReferenceEditor
{
    public class UpdaterResult : NotifyObject
    {
        private IList<XDocument> _documents;
        private IList<PackageReference> _references;
        private Dictionary<string, IList<PackageReference>> _groupedReferences;

        public IList<XDocument> Documents
        {
            get => _documents;
            set => Update(ref _documents, value);
        }

        public IList<PackageReference> References
        {
            get => _references;
            set => Update(ref _references, value);
        }

        public Dictionary<string, IList<PackageReference>> GroupedReferences
        {
            get => _groupedReferences;
            set => Update(ref _groupedReferences, value);
        }

        public void Reset()
        {
            Documents?.Clear();
            References?.Clear();
            GroupedReferences?.Clear();
        }
    }
}
