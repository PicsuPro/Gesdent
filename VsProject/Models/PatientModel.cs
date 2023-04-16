using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace VsProject.Models
{
    internal class PatientModel : INotifyPropertyChanged
    {
        private int id;
        private string firstName;
        private string lastName;
        private int age;
        private string gender;
        private string phoneNumber;
        private string emailAddress;
        public ObservableCollection<PatientModel> Patients { get; set; }


        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        [Required]
        [StringLength(50)]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        [Required]
        [StringLength(50)]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        [Required]
        [Range(0, 120)]
        public int Age
        {
            get { return age; }
            set { age = value; OnPropertyChanged(nameof(Age)); }
        }

        [Required]
        [StringLength(10)]
        public string Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        [Required]
        [StringLength(15)]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        [Required]
        [EmailAddress]
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; OnPropertyChanged(nameof(EmailAddress)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
