using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class PatientViewModel : ViewModelBase
    {
        private PatientModel _patient;
        private string _lastName = "";
        private string _firstName = "";
        private bool? _isMale;

        private bool? _isFemale;

        private string _phone = "";
        private string _phoneAlt = "";
        private string _email = "";
        private DateTime? _birthDate;
        private string _errorMessage = "";
        private bool _isNewPatient = false;

        private string _profession = "";
        private string _adresse = "";
        private string _pattern = "";
        private string _preferredDay = "";
        private string _parentName = "";

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
        public bool? IsMale
        {
            get { return _isMale; }
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
            get { return _isFemale; }
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
            get => _isNewPatient; set
            {
                _isNewPatient = value;
                OnPropertyChanged(nameof(IsNewPatient));
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
        public string Adresse
        {
            get => _adresse;
            set
            {
                _adresse = value;
                Patient.Adresse = value;
                OnPropertyChanged(nameof(Profession));
            }
        }
        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                Patient.Pattern = value;
                OnPropertyChanged(nameof(Profession));
            }
        }
        public string PreferredDay
        {
            get => _preferredDay;
            set
            {
                _preferredDay = value;
                Patient.PreferredDay = value;
                OnPropertyChanged(nameof(Profession));
            }
        }
        public string ParentName
        {
            get => _parentName;
            set
            {
                _parentName = value;
                Patient.ParentName = value;
                OnPropertyChanged(nameof(Profession));
            }
        }
        public ICommand SavePatientCommand { get; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PatientViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            SavePatientCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            Patient = UserPrincipal.PatientRepository.GetById(UserPrincipal.Current?.Id) ?? new PatientModel();
            IsNewPatient = Patient.Id == null;
#pragma warning disable CS8601 // Possible null reference assignment.
            LastName = Patient.LastName;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            FirstName = Patient.FirstName;
#pragma warning restore CS8601 // Possible null reference assignment.
            IsMale = !Patient.Sex;
            IsFemale = Patient.Sex;
#pragma warning disable CS8601 // Possible null reference assignment.
            Phone = Patient.Phone;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            PhoneAlt = Patient.PhoneAlt;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            Email = Patient.Email;
#pragma warning restore CS8601 // Possible null reference assignment.
            BirthDate = Patient.BirthDate;
#pragma warning disable CS8601 // Possible null reference assignment.
            Profession = Patient.Profession;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            Adresse = Patient.Adresse;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            Pattern = Patient.Pattern;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            PreferredDay = Patient.PreferredDay;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            ParentName = Patient.ParentName;
#pragma warning restore CS8601 // Possible null reference assignment.
        }



        private void ExecuteSaveEdit(object obj)
        {
            if (IsNewPatient)
            {
                Patient.Id = UserPrincipal.Current?.Id;
                UserPrincipal.PatientRepository.Add(Patient);
            }
            else
            {
                UserPrincipal.PatientRepository.Edit(Patient);
            }
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Patient.LastName))
                && !(string.IsNullOrWhiteSpace(Patient.FirstName))
                && Patient.Sex != null
                && !(string.IsNullOrWhiteSpace(Patient.Phone))
                && !(string.IsNullOrWhiteSpace(Patient.Email))
                && BirthDate != null;

        }

    }
}
