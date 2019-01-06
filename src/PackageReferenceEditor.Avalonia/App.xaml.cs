using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;

namespace PackageReferenceEditor.Avalonia
{
    public class App : Application
    {
        static void Main(string[] args)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            string settingPath = "settings.json";
            ReferenceEditor editor = default;

            try
            {
                if (File.Exists(settingPath))
                {
                    string json = File.ReadAllText(settingPath);
                    editor = JsonConvert.DeserializeObject<ReferenceEditor>(json, jsonSettings);
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }

            if (editor == null)
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

                editor = new ReferenceEditor()
                {
                    Feeds = feeds,
                    CurrentFeed = feeds.FirstOrDefault(),
                    SearchPath = @"C:\DOWNLOADS\GitHub",
                    SearchPatterns = patterns,
                    SearchPattern = patterns.FirstOrDefault(),
                    AlwaysUpdate = false
                };
            }

            editor.Result = new UpdaterResult()
            {
                Documents = new ObservableCollection<XmlDocument>(),
                References = new ObservableCollection<PackageReference>()
            };

            BuildAvaloniaApp().Start<MainWindow>(() => editor);

            try
            {
                var json = JsonConvert.SerializeObject(editor, Newtonsoft.Json.Formatting.Indented, jsonSettings);
                File.WriteAllText(settingPath, json);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToDebug();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
