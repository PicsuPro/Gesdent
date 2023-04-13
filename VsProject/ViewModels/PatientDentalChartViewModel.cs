using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class PatientDentalChartViewModel : ViewModelBase
    {
        private ObservableCollection<ToothModel> _teeth = new ObservableCollection<ToothModel>(
                                                          Enumerable.Range(1, 32).Select(i => new ToothModel { Index = i })
                                                          );

        public ObservableCollection<ToothModel> Teeth
        {
            get => _teeth;
            set
            {
                _teeth = value;
                OnPropertyChanged(nameof(Teeth));
            }
        }
        public PatientDentalChartViewModel() {}

        public PatientDentalChartViewModel(ObservableCollection<ToothModel> teeth)
        {
            Teeth = teeth;
        }
    }
    
}
