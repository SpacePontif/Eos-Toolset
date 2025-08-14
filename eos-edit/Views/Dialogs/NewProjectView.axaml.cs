using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Eos.Services;
using Eos.ViewModels.Base;
using Eos.ViewModels.Dialogs;
using System.Threading.Tasks;

namespace Eos.Views.Dialogs
{
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();
            DataContextChanged += NewProjectView_DataContextChanged;
        }

        private void NewProjectView_DataContextChanged(object? sender, System.EventArgs e)
        {
            if (DataContext is NewProjectViewModel vm)
                vm.OnError += Vm_OnError;
        }

        private void Vm_OnError(ViewModelBase viewModel, ViewModelErrorEventArgs args)
        {
            WindowService.ShowMessage(args.Message, "Error", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
        }

        private async void btOpenDlg_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is NewProjectViewModel vm)
            {
                if ((Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app) && (app.MainWindow != null))
                {
                    var storageProvider = app.MainWindow.StorageProvider;
                    var result = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        AllowMultiple = false
                    });
                    
                    if (result.Count > 0)
                    {
                        vm.ProjectFolder = result[0].Path.LocalPath;
                    }
                }
            }
        }
    }
}
