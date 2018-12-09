using System.Collections.Generic;
using System.Runtime.Serialization;
using ReactiveUI;

namespace PackageReferenceEditor.Avalonia.ViewModels
{
    [DataContract]
    public class MainViewModel : ReactiveObject
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

        [DataMember]
        public IList<Feed> Feeds
        {
            get => _feeds;
            set => this.RaiseAndSetIfChanged(ref _feeds, value);
        }

        [DataMember]
        public Feed CurrentFeed
        {
            get => _currentFeed;
            set => this.RaiseAndSetIfChanged(ref _currentFeed, value);
        }

        [IgnoreDataMember]
        public IList<string> Versions
        {
            get => _versions;
            set => this.RaiseAndSetIfChanged(ref _versions, value);
        }

        [IgnoreDataMember]
        public string CurrentVersion
        {
            get => _currentVersion;
            set => this.RaiseAndSetIfChanged(ref _currentVersion, value);
        }

        [DataMember]
        public string SearchPath
        {
            get => _searchPath;
            set => this.RaiseAndSetIfChanged(ref _searchPath, value);
        }

        [DataMember]
        public IList<string> SearchPatterns
        {
            get => _searchPatterns;
            set => this.RaiseAndSetIfChanged(ref _searchPatterns, value);
        }

        [DataMember]
        public string SearchPattern
        {
            get => _searchPattern;
            set => this.RaiseAndSetIfChanged(ref _searchPattern, value);
        }

        [IgnoreDataMember]
        public UpdaterResult Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }

        [IgnoreDataMember]
        public KeyValuePair<string, IList<PackageReference>> CurrentReferences
        {
            get => _currentReferences;
            set => this.RaiseAndSetIfChanged(ref _currentReferences, value);
        }

        [IgnoreDataMember]
        public PackageReference CurrentReference
        {
            get => _currentReference;
            set => this.RaiseAndSetIfChanged(ref _currentReference, value);
        }

        [DataMember]
        public bool AlwaysUpdate
        {
            get => _alwaysUpdate;
            set => this.RaiseAndSetIfChanged(ref _alwaysUpdate, value);
        }
    }
}
