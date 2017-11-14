using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Serilog;

namespace PackageReferenceEditor.Avalonia
{
    public class App : Application
    {
        static void Print(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Print(ex.InnerException);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                InitializeLogging();

                var app = new App();
                AppBuilder.Configure(app)
                    .UsePlatformDetect()
                    .SetupWithoutStarting();
                app.Start();
            }
            catch (Exception ex)
            {
                Print(ex);
            }
        }

        static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Start()
        {
            var window = new MainWindow();
            window.ShowDialog();
            Run(window);
        }
    }
}
