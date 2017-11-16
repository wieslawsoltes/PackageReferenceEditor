using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PackageReferenceEditor.Avalonia.Views
{
    public class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            var patterns = this.FindControl<DropDown>("patterns");
            var textSearchPattern = this.FindControl<TextBox>("textSearchPattern");

            patterns.SelectionChanged += (sender, e) =>
            {
                textSearchPattern.Text = (patterns.SelectedItem as DropDownItem).Content as string;
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
                var dlg = new OpenFolderDialog();
                var path = await dlg.ShowAsync((Window)this.VisualRoot);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    var textSearchPath = this.FindControl<TextBox>("textSearchPath");
                    textSearchPath.Text = path;
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
                if (DataContext is UpdaterResult result)
                {
                    var textSearchPath = this.FindControl<TextBox>("textSearchPath");
                    var textSearchPattern = this.FindControl<TextBox>("textSearchPattern");
                    result.Reset();
                    result.FindReferences(
                        textSearchPath.Text,
                        textSearchPattern.Text,
                        new string[] { });
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
                if (DataContext is UpdaterResult result)
                {
                    foreach (var reference in result.CurrentReferences.Value)
                    {
                        if (reference != result.CurrentReference)
                        {
                            reference.Version = result.CurrentReference.Version;
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

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is UpdaterResult result)
                {
                    Updater.UpdateVersions(result.CurrentReferences);
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
