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
        static void Main(string[] args)
        {
            MainViewModel vm = null;

            try
            {
                vm = JsonConvert.DeserializeObject<MainViewModel>(
                    File.ReadAllText("settings.json"),
                    new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
#endif
            }

            if (vm == null)
            {
                var feeds = new ObservableCollection<Feed>
                {
                    new Feed("api.nuget.org", "https://api.nuget.org/v3/index.json"),
                    new Feed("avalonia-ci", "https://www.myget.org/F/avalonia-ci/api/v3/index.json")
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
                    SearchPattern = patterns.FirstOrDefault()
                };
            }

            vm.Result = new UpdaterResult()
            {
                Documents = new ObservableCollection<XDocument>(),
                References = new ObservableCollection<PackageReference>()
            };

            var app = new App();
            AppBuilder.Configure(app)
                .LogToTrace()
                .UsePlatformDetect()
                .Start<MainWindow>(() => vm);

            try
            {
                var settings = JsonConvert.SerializeObject(
                    vm, 
                    Formatting.Indented, 
                    new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                File.WriteAllText("settings.json", settings);
            }
            catch (Exception ex)
            {
#if DEBUG
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
