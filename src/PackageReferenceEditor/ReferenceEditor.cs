using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
        private bool _isWorking;

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

        [IgnoreDataMember]
        public bool IsWorking
        {
            get => _isWorking;
            set => this.RaiseAndSetIfChanged(ref _isWorking, value);
        }

        public async Task Search()
        {
            try
            {
                IsWorking = true;
                await Task.Run(() =>
                {
                    Result.Reset();
                    CurrentReferences = default;
                    CurrentReference = default;
                    Versions = null;
                    CurrentVersion = null;
                    Result.FindReferences(SearchPath, SearchPattern, new string[] { });
                    CurrentReferences = Result.GroupedReferences.FirstOrDefault();
                    CurrentReference = CurrentReferences.Value?.FirstOrDefault();
                });
                IsWorking = false;
            }
            catch (Exception ex)
            {
                IsWorking = false;
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public async Task ConsolidateVersions()
        {
            try
            {
                IsWorking = true;
                await Task.Run(() =>
                {
                    foreach (var reference in CurrentReferences.Value)
                    {
                        if (reference != CurrentReference)
                        {
                            reference.Version = CurrentReference.Version;
                        }
                    }
                });
                IsWorking = false;
            }
            catch (Exception ex)
            {
                IsWorking = false;
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public async Task UseVersion()
        {
            try
            {
                IsWorking = true;
                await Task.Run(() =>
                {
                    if (CurrentReference != null)
                    {
                        CurrentReference.Version = CurrentVersion;
                    }
                });
                IsWorking = false;
            }
            catch (Exception ex)
            {
                IsWorking = false;
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public async Task GetVersions()
        {
            try
            {
                IsWorking = true;
                var versions = await NuGetApi.GetPackageVersions(CurrentFeed.Uri, CurrentReferences.Key);
                await Task.Run(() =>
                {
                    if (versions != null)
                    {
                        Versions = new ObservableCollection<string>(versions.Reverse());
                        CurrentVersion = Versions.FirstOrDefault();
                    }
                });
                IsWorking = false;
            }
            catch (Exception ex)
            {
                IsWorking = false;
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public async Task UpdateCurrent()
        {
            try
            {
                IsWorking = true;
                await Task.Run(() =>
                {
                    Updater.UpdateVersions(CurrentReferences, AlwaysUpdate);
                });
                IsWorking = false;
            }
            catch (Exception ex)
            {
                IsWorking = false;
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public async Task UpdateAll()
        {
            try
            {
                IsWorking = true;
                await Task.Run(() =>
                {
                    foreach (var references in Result.GroupedReferences)
                    {
                        Updater.UpdateVersions(references, AlwaysUpdate);
                    }
                });
                IsWorking = false;
            }
            catch (Exception ex)
            {
                IsWorking = false;
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }
    }
}
