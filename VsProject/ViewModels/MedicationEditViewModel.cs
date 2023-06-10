using System;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Repositories;

namespace VsProject.ViewModels
{
    public class MedicationEditViewModel : ViewModelBase
    {
        private MedicationModel _medication = new MedicationModel();
        private string _errorMessage = "";
        private bool _isNewUser = false;

       
         public MedicationModel Medication
        {
            get => _medication;
            set
            {
                _medication = value;
                OnPropertyChanged(nameof(Medication));
            }
        }
        
         public string? Name
        {
            get => _medication.Name;
            set
            {
                _medication.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

     
        public string ErrorMessage
        {
            get => _errorMessage; set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsNewUser
        {
            get => _isNewUser; set
            {
                _isNewUser = value;
                OnPropertyChanged(nameof(IsNewUser));
                OnPropertyChanged(nameof(IsNotNewUser));
            }
        }
        public bool IsNotNewUser => !_isNewUser;



        public ICommand SaveEditCommand { get; }
        public MedicationEditViewModel()
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = true;
        }
        public MedicationEditViewModel(MedicationModel medication)
        {
            Medication = medication;
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = medication == null;
        }


        private void ExecuteSaveEdit(object obj)
        {
            End();
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Medication.Name));
        }

    }
}

