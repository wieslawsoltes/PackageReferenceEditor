using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PackageReferenceEditor;

[DataContract]
public class ReferenceEditor : ObservableObject
{
    private IList<Feed>? _feeds;
    private Feed? _currentFeed;
    private IList<string?>? _versions;
    private string? _currentVersion;
    private string? _searchPath;
    private string? _searchPattern;
    private IList<string>? _searchPatterns;
    private UpdaterResult? _result;
    private KeyValuePair<string, IList<PackageReference>> _currentReferences;
    private PackageReference? _currentReference;
    private bool _alwaysUpdate;
    private bool _isWorking;

    [DataMember]
    public IList<Feed>? Feeds
    {
        get => _feeds;
        set => SetProperty(ref _feeds, value);
    }

    [DataMember]
    public Feed? CurrentFeed
    {
        get => _currentFeed;
        set => SetProperty(ref _currentFeed, value);
    }

    [IgnoreDataMember]
    public IList<string?>? Versions
    {
        get => _versions;
        set => SetProperty(ref _versions, value);
    }

    [IgnoreDataMember]
    public string? CurrentVersion
    {
        get => _currentVersion;
        set => SetProperty(ref _currentVersion, value);
    }

    [DataMember]
    public string? SearchPath
    {
        get => _searchPath;
        set => SetProperty(ref _searchPath, value);
    }

    [DataMember]
    public IList<string>? SearchPatterns
    {
        get => _searchPatterns;
        set => SetProperty(ref _searchPatterns, value);
    }

    [DataMember]
    public string? SearchPattern
    {
        get => _searchPattern;
        set => SetProperty(ref _searchPattern, value);
    }

    [IgnoreDataMember]
    public UpdaterResult? Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    [IgnoreDataMember]
    public KeyValuePair<string, IList<PackageReference>> CurrentReferences
    {
        get => _currentReferences;
        set => SetProperty(ref _currentReferences, value);
    }

    [IgnoreDataMember]
    public PackageReference? CurrentReference
    {
        get => _currentReference;
        set => SetProperty(ref _currentReference, value);
    }

    [DataMember]
    public bool AlwaysUpdate
    {
        get => _alwaysUpdate;
        set => SetProperty(ref _alwaysUpdate, value);
    }

    [IgnoreDataMember]
    public bool IsWorking
    {
        get => _isWorking;
        set => SetProperty(ref _isWorking, value);
    }

    public async Task Search()
    {
        try
        {
            IsWorking = true;
            await Task.Run(() =>
            {
                if (Result != null)
                {
                    Result.Reset();
                    CurrentReferences = default;
                    CurrentReference = default;
                    Versions = null;
                    CurrentVersion = null;
                    if (SearchPath != null && SearchPattern != null)
                    {
                        Result.FindReferences(SearchPath, SearchPattern, new string[] { });
                        CurrentReferences = Result.GroupedReferences.FirstOrDefault();
                        CurrentReference = CurrentReferences.Value.FirstOrDefault();
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

    public async Task ConsolidateVersions()
    {
        try
        {
            IsWorking = true;
            await Task.Run(() =>
            {
                foreach (var reference in CurrentReferences.Value)
                {
                    if (CurrentReference != null && reference != CurrentReference)
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
                if (CurrentReference != null && CurrentVersion != null)
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
            if (CurrentFeed != null)
            {
                IsWorking = true;
                var versions = await NuGetApi.GetPackageVersions(CurrentFeed.Uri, CurrentReferences.Key);
                await Task.Run(() =>
                {
                    if (versions.Count > 0)
                    {
                        Versions = new ObservableCollection<string?>(versions.Reverse());
                        CurrentVersion = Versions.FirstOrDefault();
                    }
                });
                IsWorking = false; 
            }
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
            if (Result != null)
            {
                IsWorking = true;
                await Task.Run(() =>
                {
                    if (Result.GroupedReferences != null)
                    {
                        foreach (var references in Result.GroupedReferences)
                        {
                            Updater.UpdateVersions(references, AlwaysUpdate);
                        } 
                    }
                });
                IsWorking = false; 
            }
        }
        catch (Exception ex)
        {
            IsWorking = false;
            Logger.Log(ex.Message);
            Logger.Log(ex.StackTrace);
        }
    }
}
