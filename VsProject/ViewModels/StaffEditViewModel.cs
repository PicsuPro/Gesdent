using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.ViewModels
{
    public class StaffEditViewModel : ViewModelBase
    {
        public StaffEditViewModel() 
        {
            _myDate = DateTime.Now;
        }
        private DateTime _myDate = DateTime.Now;


        public DateTime MyDate
        {
            get { return _myDate; }
            set
            {
                _myDate = value;
                OnPropertyChanged(nameof(MyDate));
            }
        }
    }
}
