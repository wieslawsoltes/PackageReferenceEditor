using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PackageReferenceEditor.Views;

public class MainView : UserControl
{
    private readonly ComboBox _comboBoxPatterns;
    private readonly Button _buttonBrowse;

    public MainView()
    {
        InitializeComponent();
        _comboBoxPatterns = this.FindControl<ComboBox>("comboBoxPatterns");
        _comboBoxPatterns.SelectionChanged += patterns_SelectionChanged;
        _buttonBrowse = this.FindControl<Button>("buttonBrowse");
        _buttonBrowse.Click += buttonBrowse_Click;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void patterns_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (DataContext is ReferenceEditor vm)
            {
                vm.SearchPattern = _comboBoxPatterns.SelectedItem as string;
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex.Message);
            if (ex.StackTrace != null)
            {
                Logger.Log(ex.StackTrace);
            }
        }
    }

    private async void buttonBrowse_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ReferenceEditor vm)
            {
                var dlg = new OpenFolderDialog();
                var window = (Window?)this.VisualRoot;
                if (window is null)
                {
                    return;
                }
                var path = await dlg.ShowAsync(window);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    vm.SearchPath = path;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex.Message);
            if (ex.StackTrace != null)
            {
                Logger.Log(ex.StackTrace);
            }
        }
    }
}
