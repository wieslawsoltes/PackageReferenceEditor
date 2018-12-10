using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using ReactiveUI;

namespace PackageReferenceEditor
{
    [DataContract]
    public class ReferenceEditor : ReactiveObject
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

        public void Search()
        {
            try
            {
                Result.Reset();
                CurrentReferences = default;
                CurrentReference = default;
                Versions = null;
                CurrentVersion = null;
                Result.FindReferences(SearchPath, SearchPattern, new string[] { });
                CurrentReferences = Result.GroupedReferences.FirstOrDefault();
                CurrentReference = CurrentReferences.Value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public void ConsolidateVersions()
        {
            try
            {
                foreach (var reference in CurrentReferences.Value)
                {
                    if (reference != CurrentReference)
                    {
                        reference.Version = CurrentReference.Version;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public void UseVersion()
        {
            try
            {
                if (CurrentReference != null)
                {
                    CurrentReference.Version = CurrentVersion;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public async void GetVersions()
        {
            try
            {
                var versions = await NuGetApi.GetPackageVersions(CurrentFeed.Uri, CurrentReferences.Key);
                if (versions != null)
                {
                    Versions = new ObservableCollection<string>(versions.Reverse());
                    CurrentVersion = Versions.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public void UpdateCurrent()
        {
            try
            {
                Updater.UpdateVersions(CurrentReferences, AlwaysUpdate);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public void UpdateAll()
        {
            try
            {
                foreach (var references in Result.GroupedReferences)
                {
                    Updater.UpdateVersions(references, AlwaysUpdate);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }
    }
}
