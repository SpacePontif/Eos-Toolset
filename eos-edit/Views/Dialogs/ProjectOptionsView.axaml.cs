using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Eos.Repositories;
using Eos.ViewModels.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eos.Views.Dialogs
{
    public partial class ProjectOptionsView : UserControl
    {
        public ProjectOptionsView()
        {
            InitializeComponent();
        }

        private async void btOpenDlg_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProjectOptionsViewModel vm)
            {
                if ((Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app) && (app.MainWindow != null))
                {
                    var target = (String?)((Button)sender)?.Tag;

                    var storageProvider = app.MainWindow.StorageProvider;
                    string? suggestedStartLocation = null;
                    
                    switch (target)
                    {
                        case "BACKUP":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.BackupFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "2DA":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.Export.TwoDAFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "SSF":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.Export.SsfFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "HAK":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.Export.HakFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "ERF":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.Export.ErfFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "TLK":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.Export.TlkFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "INC":
                            suggestedStartLocation = Path.GetFullPath(vm.SettingsCopy.Export.IncludeFolder, MasterRepository.Project.ProjectFolder);
                            break;
                        case "EXT":
                            suggestedStartLocation = Path.GetFullPath(vm.ExternalPathToAdd, MasterRepository.Project.ProjectFolder);
                            break;
                    }

                    var options = new FolderPickerOpenOptions
                    {
                        AllowMultiple = false
                    };

                    if (!string.IsNullOrEmpty(suggestedStartLocation) && Directory.Exists(suggestedStartLocation))
                    {
                        options.SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(suggestedStartLocation);
                    }

                    var result = await storageProvider.OpenFolderPickerAsync(options);
                    
                    if (result.Count > 0)
                    {
                        var selectedPath = result[0].Path.LocalPath;
                        var resultPath = Path.GetRelativePath(MasterRepository.Project.ProjectFolder, selectedPath);
                        if (resultPath.Contains($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..")) // 3+ back? Just use the absolute path
                            resultPath = selectedPath;
                        switch (target)
                        {
                            case "BACKUP":
                                vm.SettingsCopy.BackupFolder = resultPath;
                                break;
                            case "2DA":
                                vm.SettingsCopy.Export.TwoDAFolder = resultPath;
                                break;
                            case "SSF":
                                vm.SettingsCopy.Export.SsfFolder = resultPath;
                                break;
                            case "HAK":
                                vm.SettingsCopy.Export.HakFolder = resultPath;
                                break;
                            case "ERF":
                                vm.SettingsCopy.Export.ErfFolder = resultPath;
                                break;
                            case "TLK":
                                vm.SettingsCopy.Export.TlkFolder = resultPath;
                                break;
                            case "INC":
                                vm.SettingsCopy.Export.IncludeFolder = resultPath;
                                break;
                            case "EXT":
                                vm.ExternalPathToAdd = resultPath;
                                break;
                        }
                    }
                }
            }
        }

        private async void btOpenTlk_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProjectOptionsViewModel vm)
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
                    if (result.Count > 0)
                    {
                        vm.SettingsCopy.Export.BaseTlkFile = result[0].Path.LocalPath;
                    }
                }
            }
        }
    }
}
