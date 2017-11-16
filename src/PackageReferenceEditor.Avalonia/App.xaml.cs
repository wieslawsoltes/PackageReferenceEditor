using System.Collections.ObjectModel;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;

namespace PackageReferenceEditor.Avalonia
{
    public class App : Application
    {
        static void Main(string[] args)
        {
            var app = new App();
            AppBuilder.Configure(app)
                .LogToTrace()
                .UsePlatformDetect()
                .Start<MainWindow>(() => new UpdaterResult()
                {
                    Documents = new ObservableCollection<XDocument>(),
                    References = new ObservableCollection<PackageReference>()
                });
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
