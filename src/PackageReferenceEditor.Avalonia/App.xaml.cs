using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Serilog;

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
                    Documents = new List<XDocument>(),
                    References = new List<PackageReference>()
                });
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
