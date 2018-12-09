using System;
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
            this.FindControl<DropDown>("patterns").SelectionChanged += patterns_SelectionChanged;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void patterns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SearchPattern = this.FindControl<DropDown>("patterns").SelectedItem as string;
            }
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
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }
    }
}
