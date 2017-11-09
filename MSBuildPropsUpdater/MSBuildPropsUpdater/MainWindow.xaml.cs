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
            //var updater = Updater.Create(@"C:\DOWNLOADS\GitHub\", "*.props", new string[] { });
            //updater.PrintVersions();
            //updater.ValidateVersions();
            DataContext = Updater.Create(textSearchPath.Text, textSearchPattern.Text, new string[] { });
        }
    }
}
