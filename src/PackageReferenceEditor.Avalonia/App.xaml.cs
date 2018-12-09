using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using PackageReferenceEditor.Avalonia.ViewModels;

namespace PackageReferenceEditor.Avalonia
{
    public class App : Application
    {
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToDebug();

        static void Main(string[] args)
        {
            MainViewModel vm = null;

            try
            {
                vm = JsonConvert.DeserializeObject<MainViewModel>(
                    File.ReadAllText("settings.json"),
                    new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
            catch (Exception)
            {
#if _DEBUG
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
#endif
            }

            if (vm == null)
            {
                var feeds = new ObservableCollection<Feed>
                {
                    new Feed("api.nuget.org", "https://api.nuget.org/v3/index.json"),
                    new Feed("dotnet-core", "https://dotnet.myget.org/F/dotnet-core/api/v3/index.json"),
                    new Feed("avalonia-ci", "https://www.myget.org/F/avalonia-ci/api/v3/index.json"),
                    new Feed("xamlbehaviors-nightly", "https://www.myget.org/F/xamlbehaviors-nightly/api/v3/index.json"),
                    new Feed("panandzoom-nightly", "https://www.myget.org/F/panandzoom-nightly/api/v3/index.json"),
                    new Feed("dock-nightly", "https://www.myget.org/F/dock-nightly/api/v3/index.json"),
                    new Feed("portable-xaml", "https://ci.appveyor.com/nuget/portable-xaml")
                };

                var patterns = new ObservableCollection<string>
                {
                    "*.props",
                    "*.csproj"
                };

                vm = new MainViewModel()
                {
                    Feeds = feeds,
                    CurrentFeed = feeds.FirstOrDefault(),
                    SearchPath = @"C:\DOWNLOADS\GitHub",
                    SearchPatterns = patterns,
                    SearchPattern = patterns.FirstOrDefault(),
                    AlwaysUpdate = false
                };
            }

            vm.Result = new UpdaterResult()
            {
                Documents = new ObservableCollection<XDocument>(),
                References = new ObservableCollection<PackageReference>()
            };

            BuildAvaloniaApp().Start<MainWindow>(() => vm);

            try
            {
                var settings = JsonConvert.SerializeObject(
                    vm, 
                    Formatting.Indented, 
                    new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                File.WriteAllText("settings.json", settings);
            }
            catch (Exception)
            {
#if _DEBUG
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
#endif
            }
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
