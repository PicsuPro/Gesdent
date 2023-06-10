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
    class MedicationViewModel : ViewModelBase
    {
        private ObservableCollection<MedicationModel> _medications ;
        private ICollectionView _medicationCollectionView;
        private string _search;

        public ObservableCollection<MedicationModel> Medications
        {
            get => _medications;
            set
            {
                _medications = value;
                OnPropertyChanged(nameof(_medications));
            }
        }

        public string Search
        {
            get => _search;
            set
            {
                _search= value;
                _medicationCollectionView.Refresh(); // Refresh the collection view when search text changes
                OnPropertyChanged(nameof(Search));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand EditCommand { get; }

        public MedicationViewModel()
        {
            KeepLoaded = false;
            Medications = new ObservableCollection<MedicationModel>(UserPrincipal.MedicationRepository.GetAll());
            _medicationCollectionView = CollectionViewSource.GetDefaultView(Medications);
            _medicationCollectionView.Filter = FilterBySearchText;

            AddCommand = new ViewModelCommand(Add);
            RemoveCommand = new ViewModelCommand(Remove);
            EditCommand = new ViewModelCommand(Edit);
        }

        private void Remove(object parameter)
        {
            if (parameter is MedicationModel medication)
            {
                if (DialogService.ShowYesNoDialog() == true)
                {
                    Medications.Remove(medication);
                    UserPrincipal.MedicationRepository.Remove(medication);
                }
            }
        }
        private void Edit(object parameter)
        {
            if (parameter is MedicationModel medication)
            {
                if (DialogService.Show(new MedicationEditViewModel(medication)) == true)
                {
                    UserPrincipal.MedicationRepository.Edit(medication);
                    var newMedication = UserPrincipal.MedicationRepository.GetById(medication.Id);
                    if (newMedication != null)
                    { 
                        var index = Medications.IndexOf(medication);
                        Medications.Remove(medication);
                        Medications.Insert(index, newMedication);
                    }
                }

            }
        }
        private void Add(object parameter)
        {
                var newMedication = new MedicationModel();
                if (DialogService.Show(new MedicationEditViewModel(newMedication)) == true)
                {
                    newMedication.Id = UserPrincipal.MedicationRepository.Add(newMedication);
                    Medications.Add(newMedication);
                }
        }

        private bool FilterBySearchText(object item)
        {
            if (string.IsNullOrEmpty(_search))
                return true;

            var medication = item as MedicationModel ;
            if (medication == null)
                return false;

            var fullName = $"{medication.Id} {medication.Name}".ToLower();

            var searchText = _search.ToLower();
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
