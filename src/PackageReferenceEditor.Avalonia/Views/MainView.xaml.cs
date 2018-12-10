using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PackageReferenceEditor.Avalonia.Views
{
    public class MainView : UserControl
    {
        private readonly DropDown _dropDownPatterns;
        private readonly Button _buttonBrowse;

        public MainView()
        {
            InitializeComponent();
            _dropDownPatterns = this.FindControl<DropDown>("dropDownPatterns");
            _dropDownPatterns.SelectionChanged += patterns_SelectionChanged;
            _buttonBrowse = this.FindControl<Button>("buttonBrowse");
            _buttonBrowse.Click += buttonBrowse_Click;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void patterns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DataContext is ReferenceEditor vm)
                {
                    vm.SearchPattern = _dropDownPatterns.SelectedItem as string;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        private async void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is ReferenceEditor vm)
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
