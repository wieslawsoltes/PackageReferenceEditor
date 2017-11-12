using System.Windows;

namespace MSBuildPropsUpdater
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            var result = Updater.FindReferences(@"C:\DOWNLOADS\GitHub\", "*.props", new string[] { });
            result.PrintVersions();
            result.ValidateVersions();
            DataContext = Updater.FindReferences(textSearchPath.Text, textSearchPattern.Text, new string[] { });
        }
    }
}
