using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PackageReferenceEditor.Avalonia
{
    public partial class MainWindow : Window
    {
        private Button buttonBrowse;
        private Button buttonSearch;
        private Button buttonUpdate;
        private TextBox textSearchPath;
        private TextBox textSearchPattern;
        private ListBox groups;
        private ListBox references;

        public MainWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();

            buttonBrowse = this.FindControl<Button>("buttonBrowse");
            buttonSearch = this.FindControl<Button>("buttonSearch");
            buttonUpdate = this.FindControl<Button>("buttonUpdate");
            textSearchPath = this.FindControl<TextBox>("textSearchPath");
            textSearchPattern = this.FindControl<TextBox>("textSearchPattern");
            groups = this.FindControl<ListBox>("groups");
            references = this.FindControl<ListBox>("references");

            buttonBrowse.Click += buttonBrowse_Click;
            buttonSearch.Click += buttonSearch_Click;
            buttonUpdate.Click += buttonUpdate_Click;
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
                DataContext = Updater.FindReferences(textSearchPath.Text, textSearchPattern.Text, new string[] { });
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
                    if (groups.SelectedItem is KeyValuePair<string, List<PackageReference>> references)
                    {
                        Updater.UpdateVersions(references);
                    }
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
