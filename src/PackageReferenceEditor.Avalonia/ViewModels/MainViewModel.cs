using System.Collections.Generic;

namespace PackageReferenceEditor.Avalonia.ViewModels
{
    public class MainViewModel : NotifyObject
    {
        private IList<Feed> _feeds;
        private Feed _currentFeed;
        private IList<string> _versions;
        private string _currentVersion;
        private string _searchPath;
        private string _searchPattern;
        private IList<string> _searchPatterns;
        private UpdaterResult _result;
        private KeyValuePair<string, IList<PackageReference>> _currentReferences;
        private PackageReference _currentReference;
        private bool _alwaysUpdate;

        public IList<Feed> Feeds
        {
            get => _feeds;
            set => Update(ref _feeds, value);
        }

        public Feed CurrentFeed
        {
            get => _currentFeed;
            set => Update(ref _currentFeed, value);
        }

        public IList<string> Versions
        {
            get => _versions;
            set => Update(ref _versions, value);
        }

        public string CurrentVersion
        {
            get => _currentVersion;
            set => Update(ref _currentVersion, value);
        }

        public string SearchPath
        {
            get => _searchPath;
            set => Update(ref _searchPath, value);
        }

        public IList<string> SearchPatterns
        {
            get => _searchPatterns;
            set => Update(ref _searchPatterns, value);
        }

        public string SearchPattern
        {
            get => _searchPattern;
            set => Update(ref _searchPattern, value);
        }

        public UpdaterResult Result
        {
            get => _result;
            set => Update(ref _result, value);
        }

        public KeyValuePair<string, IList<PackageReference>> CurrentReferences
        {
            get => _currentReferences;
            set => Update(ref _currentReferences, value);
        }

        public PackageReference CurrentReference
        {
            get => _currentReference;
            set => Update(ref _currentReference, value);
        }

        public bool AlwaysUpdate
        {
            get => _alwaysUpdate;
            set => Update(ref _alwaysUpdate, value);
        }

        public bool ShouldSerializeFeeds() => true;

        public bool ShouldSerializeCurrentFeed() => true;

        public bool ShouldSerializeVersions() => false;

        public bool ShouldSerializeCurrentVersion() => false;

        public bool ShouldSerializeSearchPath() => true;

        public bool ShouldSerializeSearchPatterns() => true;

        public bool ShouldSerializeSearchPattern() => true;

        public bool ShouldSerializeResult() => false;

        public bool ShouldSerializeCurrentReferences() => false;

        public bool ShouldSerializeCurrentReference() => false;

        public bool ShouldSerializeAlwaysUpdate() => true;
    }
}
