using System.Windows;

namespace MSBuildPropsUpdater
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var updater = Updater.Create(@"C:\DOWNLOADS\GitHub\", "*.props", new string[] { });

            updater.PrintVersions();

            updater.ValidateVersions();

            DataContext = updater;
        }
    }
}
