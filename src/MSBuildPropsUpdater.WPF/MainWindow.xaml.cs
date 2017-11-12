using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace MSBuildPropsUpdater.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            //var result = Updater.FindReferences(@"C:\DOWNLOADS\GitHub\", "*.props", new string[] { });
            //result.PrintVersions();
            //result.ValidateVersions();

            DataContext = Updater.FindReferences(textSearchPath.Text, textSearchPattern.Text, new string[] { });
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (groups.SelectedItem is KeyValuePair<string, List<PackageReference>> references)
            {
                foreach (var v in references.Value)
                {
                    try
                    {
                        if (v.VersionAttribute != null)
                        {
                            if (v.Version != v.VersionAttribute.Value)
                            {
                                Debug.WriteLine($"Old: {v.VersionAttribute.Value}, New: {v.Version}, File: {v.FileName}");
                                v.VersionAttribute.Value = v.Version;
                                v.Document.Save(v.FileName);
                            }
                        }
                        else
                        {
                            var version = v.Reference.Elements().First(x => x.Name.LocalName == "Version");
                            if (version != null)
                            {
                                if (v.Version != v.VersionAttribute.Value)
                                {
                                    Debug.WriteLine($"Old: {version.Value}, New: {v.Version}, File: {v.FileName}");
                                    version.Value = v.Version;
                                    v.Document.Save(v.FileName);
                                }
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
    }
}
