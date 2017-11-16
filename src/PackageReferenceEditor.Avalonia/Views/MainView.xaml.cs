using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PackageReferenceEditor.Avalonia.ViewModels;

namespace PackageReferenceEditor.Avalonia.Views
{
    public class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            var patterns = this.FindControl<DropDown>("patterns");

            patterns.SelectionChanged += (sender, e) =>
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.SearchPattern = patterns.SelectedItem as string;
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    var dlg = new OpenFolderDialog();
                    var path = await dlg.ShowAsync((Window)this.VisualRoot);
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        vm.SearchPath = path;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.Result.Reset();
                    vm.CurrentReferences = default(KeyValuePair<string, IList<PackageReference>>);
                    vm.CurrentReference = default(PackageReference);
                    vm.Versions = null;
                    vm.CurrentVersion = null;
                    vm.Result.FindReferences(
                        vm.SearchPath,
                        vm.SearchPattern,
                        new string[] { });
                    vm.CurrentReferences = vm.Result.GroupedReferences.FirstOrDefault();
                    vm.CurrentReference = vm.CurrentReferences.Value.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void buttonConsolidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    foreach (var reference in vm.CurrentReferences.Value)
                    {
                        if (reference != vm.CurrentReference)
                        {
                            reference.Version = vm.CurrentReference.Version;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async void buttonVersions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    var versions = await NuGetApi.GetPackageVersions(vm.CurrentFeed.Uri, vm.CurrentReferences.Key);
                    if (versions != null)
                    {
                        vm.Versions = new ObservableCollection<string>(versions.Reverse());
                        vm.CurrentVersion = vm.Versions.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is MainViewModel vm)
                {
                    Updater.UpdateVersions(vm.CurrentReferences);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
