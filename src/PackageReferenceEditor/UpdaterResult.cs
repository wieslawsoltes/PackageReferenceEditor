using System.Collections.Generic;
using System.Xml.Linq;

namespace PackageReferenceEditor
{
    public class UpdaterResult
    {
        public IList<XDocument> Documents { get; set; }
        public IList<PackageReference> References { get; set; }
        public Dictionary<string, List<PackageReference>> GroupedReferences { get; set; }
    }
}
