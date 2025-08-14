using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Eos.Services;
using Eos.ViewModels.Base;
using Eos.ViewModels.Dialogs;
using System.Linq;
using System.Threading.Tasks;

namespace Eos.Views.Dialogs
{
    public partial class ImportDialogView : LanguageAwarePage
    {
        public ImportDialogView()
        {
            InitializeComponent();
            DataContextChanged += ImportDialogView_DataContextChanged;
        }

        private void ImportDialogView_DataContextChanged(object? sender, System.EventArgs e)
        {
            if (DataContext is ImportDialogViewModel vm)
                vm.OnError += Vm_OnError;
        }

        private void Vm_OnError(ViewModelBase viewModel, ViewModelErrorEventArgs args)
        {
            WindowService.ShowMessage(args.Message, "Missing Information", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
        }

        private async void btSelectFile_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is ImportDialogViewModel vm)
            {
                if ((Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app) && (app.MainWindow != null))
                {
                    var storageProvider = app.MainWindow.StorageProvider;
                    var fileTypes = new[]
                    {
                        new FilePickerFileType("Importable Files")
                        {
                            Patterns = new[] { "*.hak", "*.erf", "*.2da" }
                        },
                        new FilePickerFileType("Hak File")
                        {
                            Patterns = new[] { "*.hak" }
                        },
                        new FilePickerFileType("Encapsulated Resource File")
                        {
                            Patterns = new[] { "*.erf" }
                        },
                        new FilePickerFileType("2D Array File")
                        {
                            Patterns = new[] { "*.2da" }
                        }
                    };

                    var options = new FilePickerOpenOptions
                    {
                        AllowMultiple = true,
                        FileTypeFilter = fileTypes
                    };

                    var result = await storageProvider.OpenFilePickerAsync(options);
                    if (result.Any())
                    {
                        foreach (var file in result)
                        {
                            vm.Files.Add(file.Path.LocalPath);
                        }
                    }
                }
            }
        }

        private async void btSelectTlkFile_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is ImportDialogViewModel vm)
            {
                if ((Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app) && (app.MainWindow != null))
                {
                    var storageProvider = app.MainWindow.StorageProvider;
                    var fileTypes = new[]
                    {
                        new FilePickerFileType("Talk Table File")
                        {
                            Patterns = new[] { "*.tlk" }
                        }
                    };

                    var options = new FilePickerOpenOptions
                    {
                        AllowMultiple = false,
                        FileTypeFilter = fileTypes
                    };

                    var result = await storageProvider.OpenFilePickerAsync(options);
                    if (result.Any())
                    {
                        vm.TlkFile = result.First().Path.LocalPath;
                    }
                }
            }
        }
    }
}
