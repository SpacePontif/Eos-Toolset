using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;
using Eos.Models;
using Eos.ViewModels.Base;
using Eos.Repositories;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;

namespace Eos.ViewModels
{
    public class SpellViewModel : DataDetailViewModel<Spell>
    {
        public SpellViewModel() : base()
        {
            OpenSpellbookCommand = ReactiveCommand.Create<SpellbookEntryInfo>(OpenSpellbook);
        }

        public SpellViewModel(Spell spell) : base(spell)
        {
            OpenSpellbookCommand = ReactiveCommand.Create<SpellbookEntryInfo>(OpenSpellbook);
        }

        protected override string GetHeader()
        {
            return Data.Name;
        }

        protected override ISolidColorBrush GetEntityColor()
        {
            return new SolidColorBrush(Color.FromArgb(100, 193, 104, 171));
        }

        private void OpenSpellbook(SpellbookEntryInfo spellbookEntry)
        {
            if (MasterRepository.Spellbooks != null && !string.IsNullOrEmpty(spellbookEntry.SpellbookName))
            {
                var spellbook = MasterRepository.Spellbooks.FirstOrDefault(sb => sb?.Name == spellbookEntry.SpellbookName);
                
                if (spellbook != null)
                {
                    // Open the spellbook detail view first
                    MessageDispatcher.Send(MessageType.OpenDetail, spellbook, true);
                    
                    // Set the selected tab to the appropriate spell level
                    var targetTabIndex = spellbookEntry.Level + 1;
                    var spellbookViewModel = GetSpellbookViewModel(spellbook);
                    if (spellbookViewModel != null)
                        spellbookViewModel.SelectedTabIndex = targetTabIndex;
                }
            }
        }

        private static SpellbookViewModel? GetSpellbookViewModel(Spellbook spellbook)
        {
            // Access the MainWindowViewModel through the Application's main window
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime app && 
                app.MainWindow?.DataContext is MainWindowViewModel mainWindowVM)
            {
                return mainWindowVM.DetailViewList.OfType<SpellbookViewModel>()
                    .FirstOrDefault(vm => vm.Data?.Name == spellbook.Name);
            }
            return null;
        }

        public ReactiveCommand<SpellbookEntryInfo, Unit> OpenSpellbookCommand { get; private set; }
    }
}
