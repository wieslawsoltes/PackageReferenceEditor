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

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext = Updater.FindReferences(textSearchPath.Text, textSearchPattern.Text, new string[] { });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
