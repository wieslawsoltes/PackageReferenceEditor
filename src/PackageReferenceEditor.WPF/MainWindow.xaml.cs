using System;
using System.Collections.Generic;
using System.Windows;

namespace PackageReferenceEditor.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            DataContext = Updater.FindReferences(textSearchPath.Text, textSearchPattern.Text, new string[] { });
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is UpdaterResult result)
                {
                    if (groups.SelectedItem is KeyValuePair<string, List<PackageReference>> references)
                    {
                        Updater.UpdateVersions(references);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
