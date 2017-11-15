using System.Collections.Generic;
using System.Xml.Linq;

namespace PackageReferenceEditor
{
    public class UpdaterResult : NotifyObject
    {
        private IList<XDocument> _documents;
        private IList<PackageReference> _references;
        private Dictionary<string, List<PackageReference>> _groupedReferences;
        private KeyValuePair<string, List<PackageReference>> _currentReferences;
        private PackageReference _currentReference;

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

        public Dictionary<string, List<PackageReference>> GroupedReferences
        {
            get => _groupedReferences;
            set => Update(ref _groupedReferences, value);
        }

        public KeyValuePair<string, List<PackageReference>> CurrentReferences
        {
            get => _currentReferences;
            set => Update(ref _currentReferences, value);
        }

        public PackageReference CurrentReference
        {
            get => _currentReference;
            set => Update(ref _currentReference, value);
        }

        public void Reset()
        {
            Documents?.Clear();
            References?.Clear();
            GroupedReferences?.Clear();
            CurrentReferences = default(KeyValuePair<string, IList<PackageReference>>);
            CurrentReference = default(PackageReference);
        }
    }
}
