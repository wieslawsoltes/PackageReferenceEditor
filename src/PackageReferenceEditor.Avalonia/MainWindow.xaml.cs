using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PackageReferenceEditor.Avalonia
{
    public partial class MainWindow : Window
    {
        private TextBox textSearchPath;
        private TextBox textSearchPattern;

        public MainWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();

            textSearchPath = this.FindControl<TextBox>("textSearchPath");
            textSearchPattern = this.FindControl<TextBox>("textSearchPattern");
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
                var path = await dlg.ShowAsync(this);
                if (!string.IsNullOrWhiteSpace(path))
                {
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
                    result.FindReferences(textSearchPath.Text, textSearchPattern.Text, new string[] { });
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
