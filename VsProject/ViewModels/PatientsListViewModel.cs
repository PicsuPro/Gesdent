using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;

namespace VsProject.ViewModels
{
    class PatientsListViewModel : ViewModelBase
    {
        private ObservableCollection<PatientModel> _patients;
        private ICollectionView _patientCollectionView;
        private string _searchPatient;

        public ObservableCollection<PatientModel> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        public string SearchPatient
        {
            get => _searchPatient;
            set
            {
                _searchPatient = value;
                _patientCollectionView.Refresh(); // Refresh the collection view when search text changes
                OnPropertyChanged(nameof(SearchPatient));
            }
        }

        public ICommand EditPatientCommand { get; }
        public ICommand RemovePatientCommand { get; }

        public PatientsListViewModel()
        {

            KeepLoaded = false;
            Patients = new ObservableCollection<PatientModel>(UserPrincipal.PatientRepository.GetAll());
            _patientCollectionView = CollectionViewSource.GetDefaultView(Patients);
            _patientCollectionView.Filter = FilterBySearchText;

            RemovePatientCommand = new ViewModelCommand(RemovePatient);
            EditPatientCommand = new ViewModelCommand(EditPatient);
        }

        private void RemovePatient(object parameter)
        {
            if (parameter is PatientModel patient)
            {
                if (DialogService.ShowYesNoDialog() == true)
                {
                    Patients.Remove(patient);
                    UserPrincipal.PatientRepository.Remove(patient);
                }
            }
        }
        private void EditPatient(object parameter)
        {
            if(parameter is PatientModel patient)
            {
                var viewModel = new PatientEditViewModel(patient);
                NavService.Navigate(viewModel);

            }
        }

        private bool FilterBySearchText(object item)
        {
            if (string.IsNullOrEmpty(_searchPatient))
                return true;

            var patient = item as PatientModel;
            if (patient == null)
                return false;

            var fullName = $"{patient.LastName} {patient.FirstName} {patient.Surname} {patient.Id} {patient.BirthDate} {patient.Phone} {patient.PhoneAlt} {patient.PhoneAlt}".ToLower();

            var searchText = _searchPatient.ToLower();
            var searchParts = searchText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            foreach (var part in searchParts)
            {
                if (!fullName.Contains(part))
                { 
                        return false; 
                }
            }
            return true;
        }




    }
}
