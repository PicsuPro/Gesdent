using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
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
        public ICommand AppointmentCommand { get; }
        public ICommand OrdonnanceCommand { get; }
        public ICommand AddPatientCommand { get; }

        public PatientsListViewModel()
        {

            KeepLoaded = false;
            Patients = new ObservableCollection<PatientModel>(UserPrincipal.PatientRepository.GetAll());
            _patientCollectionView = CollectionViewSource.GetDefaultView(Patients);
            _patientCollectionView.Filter = FilterBySearchText;

            RemovePatientCommand = new ViewModelCommand(RemovePatient);
            EditPatientCommand = new ViewModelCommand(EditPatient);
            AppointmentCommand = new ViewModelCommand(AddAppointmentForPatient);
            OrdonnanceCommand = new ViewModelCommand(CreateOrdonnance);
            AddPatientCommand = new ViewModelCommand(AddPatient);
        }

        private void AddPatient(object obj)
        {
            NavService.Navigate(new PatientEditViewModel());
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
                var patientRecord = UserPrincipal.PatientRecordRepository.GetByPatientId(patient.Id);
                var Teeth = UserPrincipal.ToothRepository.GetAll((int)patient.Id);
                var viewModel = new PatientEditViewModel(patient, patientRecord,Teeth);
                NavService.Navigate(viewModel);

            }
        }
        private void AddAppointmentForPatient(object parameter)
        {
            if (parameter is PatientModel patient)
            {
                DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                var appointment = new AppointmentModel() {PatientId = patient.Id , Date = now, StartTime = new TimeOnly(12,0), EndTime = new TimeOnly(13,0)};
                if (DialogService.Show(new AppointmentEditViewModel(new AppointmentViewModel(appointment))) == true)
                {
                    UserPrincipal.AppointmentRepository.Add(appointment);
                }
            }
        }
        private void CreateOrdonnance(object parameter)
        {
            if (parameter is PatientModel patient)
            {
                DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                if (DialogService.Show(new OrdonnanceViewModel(patient)) == true)
                {
                }
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
