using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using VsProject.Models;

namespace VsProject.ViewModels
{
    class ListViewModel : ViewModelBase
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

        public ICommand RemovePatientCommand { get; }

        public ListViewModel()
        {

            Patients = new ObservableCollection<PatientModel>(UserPrincipal.PatientRepository.GetAll());
            _patientCollectionView = CollectionViewSource.GetDefaultView(Patients);
            _patientCollectionView.Filter = FilterBySearchText;

            RemovePatientCommand = new ViewModelCommand(RemovePatient);
        }

        private void RemovePatient(object parameter)
        {
            var patient = parameter as PatientModel;
            if (patient != null)
            {
                Patients.Remove(patient);
                UserPrincipal.PatientRepository.Remove(patient);
            }
        }

        private bool FilterBySearchText(object item)
        {
            if (string.IsNullOrEmpty(_searchPatient))
                return true;

            var patient = item as PatientModel;
            if (patient == null)
                return false;

            var fullName = $"{patient.LastName} {patient.FirstName} {patient.Surname}".ToLower();
            return fullName.Contains(_searchPatient.ToLower());
        }

    }
}
