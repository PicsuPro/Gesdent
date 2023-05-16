using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VsProject.ViewModels
{
    public class PatientEditViewModel : ViewModelBase
    {
    

        private PatientModel _patient;
        private string _lastName = "";
        private string _firstName = "";
        private string _surname = "";
        private bool? _isMale;

        private bool? _isFemale;

        private string _phone = "";
        private string _phoneAlt = "";
        private string _email = "";
        private DateTime? _birthDate;
        private string _errorMessage = "";
        private bool _isNewPatient = false;
        private bool _isEditing = false;

        private string _profession = "";
        private string _adress = "";
        private string _motive = "";
        private string _orientedBy = "";
        private string _preferredDay = "";
        private string _parentName = "";

        private PatientRecordModel _patientRecord;


        private ObservableCollection<ToothModel> _teeth;

        public PatientModel Patient
        {
            get => _patient;
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                Patient.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                Patient.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                Patient.Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public bool? IsMale
        {
            get => _isMale; 
            set
            {
                if (_isMale != value)
                {
                    _isMale = value;
                    Patient.Sex = !value;
                    OnPropertyChanged(nameof(IsMale));
                }
            }
        }
        public bool? IsFemale
        {
            get => _isFemale; 
            set
            {
                if (_isFemale != value)
                {
                    _isFemale = value;
                    Patient.Sex = value;
                    OnPropertyChanged(nameof(IsFemale));
                }
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                Patient.Phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string PhoneAlt
        {
            get => _phoneAlt;
            set
            {
                _phoneAlt = value;
                Patient.PhoneAlt = value;
                OnPropertyChanged(nameof(PhoneAlt));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                Patient.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                Patient.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public string Profession
        {
            get => _profession;
            set
            {
                _profession = value;
                Patient.Profession = value;
                OnPropertyChanged(nameof(Profession));
            }
        }
        public string Adress
        {
            get => _adress;
            set
            {
                _adress = value;
                Patient.Adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }
       public string Motive
        {
            get => _motive;
            set
            {
                _motive = value;
                Patient.Motive = value;
                OnPropertyChanged(nameof(Motive));
            }
        }
       public string OrientedBy
        {
            get => _orientedBy;
            set
            {
                _orientedBy = value;
                Patient.OrientedBy = value;
                OnPropertyChanged(nameof(OrientedBy));
            }
        }
   
        public string PreferredDay
        {
            get => _preferredDay;
            set
            {
                _preferredDay = value;
                Patient.PreferredDay = value;
                OnPropertyChanged(nameof(PreferredDay));
            }
        }
        public string ParentName
        {
            get => _parentName;
            set
            {
                _parentName = value;
                Patient.ParentName = value;
                OnPropertyChanged(nameof(ParentName));
            }
        }

        public PatientRecordModel PatientRecord
        {
            get => _patientRecord;
            set
            {
                _patientRecord = value;
                OnPropertyChanged(nameof(PatientRecord));
            }
        }

        public ObservableCollection<ToothModel> Teeth
        {
            get => _teeth;
            set
            {
                _teeth = value;
                OnPropertyChanged(nameof(Teeth));
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
        public bool IsNewPatient 
        {
            get => _isNewPatient;
            set
            {
                _isNewPatient = value;
                OnPropertyChanged(nameof(IsNewPatient));
            } 
        }

        public bool IsEditing
        {
            get => _isEditing; set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public ICommand SaveEditCommand { get; }
        

        public PatientEditViewModel()
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            Patient = new PatientModel();
            PatientRecord = new PatientRecordModel();
            Teeth = new ObservableCollection<ToothModel>(
                                                        Enumerable.Range(1, 32).Select(i => new ToothModel { Number = i })
                                                        );
            IsNewPatient =  true;
        }
        public PatientEditViewModel(PatientModel patient, PatientRecordModel patientRecord, IEnumerable<ToothModel> teeth)
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            Patient = patient;
            PatientRecord = patientRecord;
            PatientRecord.PatientId = Patient.Id;
            Teeth = new ObservableCollection<ToothModel>(teeth);
            LastName = Patient.LastName;
            FirstName = Patient.FirstName;
            Surname = Patient.Surname;
            IsMale = !Patient.Sex;
            IsFemale = Patient.Sex;
            Phone = Patient.Phone;
            PhoneAlt = Patient.PhoneAlt;
            Email = Patient.Email;
            BirthDate = Patient.BirthDate;
            Profession = Patient.Profession;
            Adress = Patient.Adress;
            Motive = Patient.Motive;
            OrientedBy = Patient.OrientedBy;
            Adress = Patient.Adress;
            PreferredDay = Patient.PreferredDay;
            ParentName = Patient.ParentName;

            IsNewPatient = Patient.Id == null;
        }


        private void ExecuteSaveEdit(object obj)
        {
            if (IsNewPatient)
            {
                Patient.Id = UserPrincipal.PatientRepository.Add(Patient);
                PatientRecord.PatientId = Patient.Id;
                UserPrincipal.PatientRecordRepository.Add(PatientRecord);
                UserPrincipal.ToothRepository.AddAll(Teeth.ToList(), PatientRecord.PatientId);
            }
            else
            {
                UserPrincipal.PatientRepository.Edit(Patient);
                //UserPrincipal.PatientRecordRepository.Edit();
                UserPrincipal.ToothRepository.EditAll(Teeth.ToList(), Patient.Id);
            }
            IsEditing = false;
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Patient.LastName))
                && !(string.IsNullOrWhiteSpace(Patient.FirstName))
                && Patient.Sex != null
                && !(string.IsNullOrWhiteSpace(Patient.Phone))
                && !(string.IsNullOrWhiteSpace(Patient.Email))
                && !(string.IsNullOrWhiteSpace(Patient.Profession))
                && !(string.IsNullOrWhiteSpace(Patient.Adress))
                && BirthDate != null
                && IsEditing;

        }

    }
}
