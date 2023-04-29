using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class PatientListViewModel : ViewModelBase
    {

        private ObservableCollection<PatientModel> _patients;

        public ObservableCollection<PatientModel> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }
        public ICommand AddPatientCommand { get; }
        public ICommand EditPatientCommand { get; }
        public ICommand RemovePatientCommand { get; }

        public PatientListViewModel()
        {

            Patients = new ObservableCollection<PatientModel>(UserPrincipal.PatientRepository.GetAll());

           // AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand);
           // EditPatientCommand = new ViewModelCommand((patient) => ExecuteEditPatientCommand((PatientModel)patient));
           // RemovePatientCommand = new ViewModelCommand((user) => ExecuteRemovePatientCommand((PatientModel)patient));
        }



        //private void ExecuteAddPatientCommand(object obj)
        //{
        //    var newPatient = new PatientModel();

        //    {
        //        UserPrincipal.PatientRepository.Add(newPatient);
        //        Patients.Add(UserPrincipal.PatientRepository.GetByPatientname(newPatient.PatientName));
        //    }


        //}

        //private void ExecuteEditPatientCommand(PatientModel user)
        //{
        //        PatientPrincipal.PatientRepository.Edit(user);
        //        var index = Patients.IndexOf(user);
        //        Patients.Remove(user);
        //        Patients.Insert(index, PatientPrincipal.PatientRepository.GetById((Guid)user.Id));

        //}


        //private void ExecuteRemovePatientCommand(PatientModel patient)
        //{
        //    UserPrincipal.PatientRepository.Remove(patient);
        //    Patients.Remove(patient);
        //}

      
    }
}

