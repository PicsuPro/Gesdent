using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class StaffEditViewModel : ViewModelBase
    {
        private StaffModel _staff;
        private string? _lastName = "";
        private string? _firstName = "";
        private bool? _isMale;
        
        private bool? _isFemale;
        
        private string? _phone = "";
        private string? _phoneAlt = "";
        private string? _email = "";
        private DateTime? _birthDate;
        private string? _errorMessage = "";
        private bool _isNewStaff = false;
        private bool _isEditing = false;

        public StaffModel Staff
        {
            get => _staff;
            set
            {
                _staff = value;
                OnPropertyChanged(nameof(Staff));
            }
        }
        public string? LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                Staff.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string? FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                Staff.FirstName = value;
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
                    Staff.Sex = !value;
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
                    Staff.Sex = value;
                    OnPropertyChanged(nameof(IsFemale));
                }
            }
        }
        public string? Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                Staff.Phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string? PhoneAlt
        {
            get => _phoneAlt;
            set
            {
                _phoneAlt = value;
                Staff.PhoneAlt = value;
                OnPropertyChanged(nameof(PhoneAlt));
            }
        }


        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                Staff.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                Staff.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }


        public string? ErrorMessage
        {
            get => _errorMessage; set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        public bool IsNewStaff
        {
            get => _isNewStaff; set
            {
                _isNewStaff = value;
                OnPropertyChanged(nameof(IsNewStaff));
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

        public StaffEditViewModel()
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            Staff = UserPrincipal.StaffRepository.GetByUserId(UserPrincipal.Current?.Id) ?? new StaffModel();
            IsNewStaff = Staff.UserId == null;
            LastName = Staff.LastName;
            FirstName = Staff.FirstName;
            IsMale = !Staff.Sex;
            IsFemale = Staff.Sex;
            Phone = Staff.Phone;
            PhoneAlt = Staff.PhoneAlt;
            Email = Staff.Email;
            BirthDate = Staff.BirthDate;
        }



        private void ExecuteSaveEdit(object obj)
        {
            if(IsNewStaff)
            {
                Staff.UserId = UserPrincipal.Current?.Id;
                UserPrincipal.StaffRepository.Add(Staff);
            }else
            {
                UserPrincipal.StaffRepository.Edit(Staff);
            }
            IsEditing = false;
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Staff.LastName))
                && !(string.IsNullOrWhiteSpace(Staff.FirstName))
                && Staff.Sex != null
                && !(string.IsNullOrWhiteSpace(Staff.Phone))
                && !(string.IsNullOrWhiteSpace(Staff.Email))
                && BirthDate != null
                && IsEditing;

        }

    }
}
