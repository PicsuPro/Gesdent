using System.Windows.Input;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand AddMedicationCommand { get; }
        public ICommand SettingsCommand { get; }

        public MainViewModel()
        {
            SettingsCommand = new ViewModelCommand(ExecuteSettingsCommand);
            AddMedicationCommand = new ViewModelCommand(ExecuteAddMedicationCommand);

        }

        private void ExecuteSettingsCommand(object obj)
        {
            DialogService.Show(new SettingsViewModel());
        }
        private void ExecuteAddMedicationCommand(object obj)
        {
            DialogService.Show(new MedicationViewModel());
        }
    }
}
