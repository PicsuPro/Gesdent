using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;
using static System.Net.Mime.MediaTypeNames;
using static VsProject.ViewModels.PatientEditViewModel;

namespace VsProject.ViewModels
{
    public class PatientEditViewModel : ViewModelBase
    {

        public class SortedObservableCollection<T> : ObservableCollection<T>
        {
            public SortedObservableCollection()
            {
            }

            public SortedObservableCollection(IEnumerable<T> collection) : base(collection)
            {
            }

            public SortedObservableCollection(List<T> list) : base(list)
            {
            }

            protected override void InsertItem(int index, T item)
            {
                int sortedIndex = FindSortedIndex(item);
                base.InsertItem(sortedIndex, item);
            }

            protected override void SetItem(int index, T item)
            {
                int sortedIndex = FindSortedIndex(item);
                base.SetItem(sortedIndex, item);
            }

            private int FindSortedIndex(T item)
            {
                IComparable indexComparable = (item as ToothProblemModel)?.ToothNumber;
                if (indexComparable == null)
                    return Count;

                for (int i = 0; i < Count; i++)
                {
                    IComparable existingIndexComparable = (this[i] as ToothProblemModel)?.ToothNumber;
                    if (indexComparable.CompareTo(existingIndexComparable) < 0)
                        return i;
                }

                return Count;
            }
        }


        public class ToothProblemModel : ViewModelBase
        {
            private int _toothNumber = 0;
            private string? _toothProblem;
            private bool _isDefault = false;
            public int ToothNumber
            {
                get => _toothNumber;
                set
                {
                    _toothNumber = value;
                    OnPropertyChanged(nameof(ToothNumber));
                }
            }
            public string? ToothProblem
            {
                get => _toothProblem;
                set
                {
                    _toothProblem = value;
                    OnPropertyChanged(nameof(ToothProblem));
                }
            }
            public bool IsDefault
            {
                get => _isDefault;
                set
                {
                    _isDefault = value;
                    OnPropertyChanged(nameof(IsDefault));
                }
            }
        }
        private int _currentPanelIndex = 0;
        public int CurrentPanelIndex 
        { get => _currentPanelIndex;
            set
            {
                _currentPanelIndex = value;
                OnPropertyChanged(nameof(CurrentPanelIndex));
            }
        } 

        private PatientModel _patient;
        private string _lastName = "";
        private string _firstName = "";
        private string _surname = "";
        private bool? _isMale;

        private bool? _isFemale;

        private string _phone = "";
        private string _phoneAlt = "";
        private string _email = "";
        private DateOnly _birthDate;
        private string _errorMessage = "";
        private bool _isNewPatient = false;
        private bool _isEditing = false;

        private string _profession = "";
        private string _adress = "";
        private string _motive = "";
        private string _orientedBy = "";
        private string _preferredDay = "";
        private string _parentName = "";
        private ToothModel? _selectedTooth;
        private string? _selectedToothProblem;

        private PatientRecordModel _patientRecord;
        private ObservableCollection<ToothModel> _teeth;
        
        private List<PatientRecordModel> _patientRecordHistory;
        private List<ObservableCollection<ToothModel>> _teethHistory;
        private int _historyIndex = 0;

        private SortedObservableCollection<ToothProblemModel> _problems = new SortedObservableCollection<ToothProblemModel>();
        private ToothProblemModel _problem = new ToothProblemModel();
        public string[] Numbers { get; } = Enumerable.Range(0, 33)
                                            .Select(n => n == 0 ? "None" : n.ToString())
                                            .ToArray();


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
        public DateOnly BirthDate
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
                OnPropertyChanged(nameof(Diagnostic));
                OnPropertyChanged(nameof(Plan));
                OnPropertyChanged(nameof(Notes));
                OnPropertyChanged(nameof(IsInHistory));

            }
        }


        public string? Diagnostic
        {
            get => PatientRecord.Diagnostic;
            set
            {
                PatientRecord.Diagnostic = value;
                OnPropertyChanged(nameof(Diagnostic));
                OnPropertyChanged(nameof(PatientRecord));
            }
        }
        public string? Plan
        {
            get => PatientRecord.TreatmentPlan;
            set
            {
                PatientRecord.TreatmentPlan = value;
                OnPropertyChanged(nameof(Plan));
                OnPropertyChanged(nameof(PatientRecord));
            }
        }
        public string? Notes
        {
            get => PatientRecord.Notes;
            set
            {
                PatientRecord.Notes = value;
                OnPropertyChanged(nameof(Notes));
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
                OnPropertyChanged(nameof(IsInHistory));

            }
        }
        public List<PatientRecordModel> PatientRecordHistory
        {
            get => _patientRecordHistory;
            set
            {
                _patientRecordHistory = value;
                OnPropertyChanged(nameof(PatientRecordHistory));
                OnPropertyChanged(nameof(IsInHistory));

            }
        }
        public List<ObservableCollection<ToothModel>> TeethHistory
        {
            get => _teethHistory;
            set
            {
                _teethHistory = value;
                OnPropertyChanged(nameof(TeethHistory));
                OnPropertyChanged(nameof(IsInHistory));

            }
        }
        public int HistoryIndex
        {
            get => _historyIndex;
            set
            {
                _historyIndex = value;
                OnPropertyChanged(nameof(HistoryIndex));
                OnPropertyChanged(nameof(IsInHistory));
            }
        }

        public bool IsInHistory => _historyIndex > 0;

        public ToothModel? SelectedTooth
        {
            get => _selectedTooth;
            set
            {
                var t = _selectedTooth;
                _selectedTooth = value;
                if (t != null)
                {
                    Problems = new SortedObservableCollection<ToothProblemModel>(Problems.Where(p => p.ToothNumber != t.Number || p.IsDefault == false));

                    if (t.ApicalReaction || t.Decay)
                    {

                        Problems.Add(new ToothProblemModel
                        {
                            ToothNumber = t.Number,
                            ToothProblem = t.DefaultProblem,
                            IsDefault = true
                        });
                    }
                }
                OnPropertyChanged(nameof(SelectedToothProblems));
                OnPropertyChanged(nameof(SelectedTooth));
            }
        }
        public ObservableCollection<string> SelectedToothProblems
        {
            get => SelectedTooth?.Problems?? new ObservableCollection<string>();
            set
            {
                if (SelectedTooth != null)
                {
                    SelectedTooth.Problems = value;
                }
                OnPropertyChanged(nameof(SelectedToothProblems));
            }
        }

        public SortedObservableCollection<ToothProblemModel> Problems 
        { 
            get => _problems;
            set
            {
                _problems = value;
                ObservableCollection<string> addedProblems = new ObservableCollection<string>(value
                                                            .Where(p => p.ToothNumber == 0 && !string.IsNullOrWhiteSpace(p.ToothProblem))
                                                            .Select(p => p.ToothProblem ?? ""));
                PatientRecord.Problems = addedProblems;
                OnPropertyChanged(nameof(Problems));
            }
        }

        public ToothProblemModel Problem
        {
            get => _problem;
            set
            {
                _problem = value;
                OnPropertyChanged(nameof(Problem));
            }
        }
        public int ToothNumber
        {
            get => Problem.ToothNumber;
            set
            {
                Problem.ToothNumber = value;
                OnPropertyChanged(nameof(ToothNumber));
            }
        }
        public string? ToothProblem
        {
            get => Problem.ToothProblem;
            set
            {
                Problem.ToothProblem = value;
                OnPropertyChanged(nameof(ToothProblem));
            }
        }
        public string? SelectedToothProblem
        {
            get => _selectedToothProblem;
            set
            {
                _selectedToothProblem = value;
                OnPropertyChanged(nameof(SelectedToothProblem));
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
        public bool IsNotNewPatient => !IsNewPatient;
        
        public bool IsEditing
        {
            get => _isEditing; set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(IsInHistory));

            }
        }

        public ICommand SaveEditCommand { get; }
        public ICommand AppointmentCommand { get; }
        public ICommand OrdonnanceCommand { get; }
        public ICommand RemovePatientCommand { get; }
        public ICommand RemoveProblemCommand { get; }
        public ICommand AddProblemCommand { get; }
        public ICommand GoToToothCommand { get; }
        public ICommand RemoveProblemSelectedToothCommand { get; }
        public ICommand AddProblemSelectedToothCommand { get; }
        public ICommand GoBackInHistory { get; }
        public ICommand GoForwardInHistory { get; }


        public PatientEditViewModel()
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            AppointmentCommand = new ViewModelCommand(ExecuteAddAppointment, (_) => { return IsNotNewPatient; });
            OrdonnanceCommand = new ViewModelCommand(ExecuteAddOrdonnance, (_) => { return IsNotNewPatient; });
            RemovePatientCommand = new ViewModelCommand(ExecuteRemovePatient, (_) => { return IsNotNewPatient; });
            RemoveProblemCommand = new ViewModelCommand(ExecuteRemoveProblem, (_) => { return IsEditing; });
            AddProblemCommand = new ViewModelCommand(ExecuteAddProblem, CanExecuteAddProblem);
            GoToToothCommand = new ViewModelCommand(ExecuteGoToTooth);
            RemoveProblemSelectedToothCommand = new ViewModelCommand(ExecuteRemoveProblemSelectedTooth, (_) => { return SelectedTooth != null &&  IsEditing; });
            AddProblemSelectedToothCommand = new ViewModelCommand(ExecuteAddProblemSelectedTooth, CanExecuteAddProblemSelectedTooth);
            GoBackInHistory = new ViewModelCommand(ExecuteGoBackInHistory, (_) => { return IsNotNewPatient && PatientRecordHistory != null && HistoryIndex < PatientRecordHistory.Count - 1; });
            GoForwardInHistory = new ViewModelCommand(ExecuteGoForwardInHistory, (_) => { return IsNotNewPatient && PatientRecordHistory != null && IsInHistory; });

            IsNewPatient = true;
            IsEditing = true;

            Patient = new PatientModel();
            PatientRecord = new PatientRecordModel();
            Teeth = new ObservableCollection<ToothModel>(Enumerable.Range(1, 32).Select(i => new ToothModel { Number = i }));
        }

      
        public PatientEditViewModel(PatientModel patient, PatientRecordModel patientRecord, IEnumerable<ToothModel> teeth)
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            AppointmentCommand = new ViewModelCommand(ExecuteAddAppointment, (_) => { return IsNotNewPatient; });
            OrdonnanceCommand = new ViewModelCommand(ExecuteAddOrdonnance, (_) => { return IsNotNewPatient; });
            RemovePatientCommand = new ViewModelCommand(ExecuteRemovePatient, (_) => { return IsNotNewPatient; });
            RemoveProblemCommand = new ViewModelCommand(ExecuteRemoveProblem, (_) => { return IsEditing; });
            AddProblemCommand = new ViewModelCommand(ExecuteAddProblem, CanExecuteAddProblem);
            GoToToothCommand = new ViewModelCommand(ExecuteGoToTooth);
            RemoveProblemSelectedToothCommand = new ViewModelCommand(ExecuteRemoveProblemSelectedTooth, (_) => { return SelectedTooth != null && IsEditing; });
            AddProblemSelectedToothCommand = new ViewModelCommand(ExecuteAddProblemSelectedTooth, CanExecuteAddProblemSelectedTooth);
            GoBackInHistory = new ViewModelCommand(ExecuteGoBackInHistory, (_) => { return IsNotNewPatient && PatientRecordHistory != null && HistoryIndex < PatientRecordHistory.Count - 1; });
            GoForwardInHistory = new ViewModelCommand(ExecuteGoForwardInHistory, (_) => { return IsNotNewPatient && PatientRecordHistory != null && IsInHistory; });
            Patient = patient;
           


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

            PatientRecord = patientRecord;
            PatientRecord.PatientId = Patient.Id;
            Teeth = new ObservableCollection<ToothModel>(teeth);
            PatientRecordHistory = (List<PatientRecordModel>)UserPrincipal.PatientRecordRepository.GetAllFromHistory(Patient.Id);
            List<IEnumerable<ToothModel>> toothModelLists = (List<IEnumerable<ToothModel>>)UserPrincipal.ToothRepository.GetAllFromHistory((int)PatientRecord.PatientId);
            TeethHistory = toothModelLists
                            .Select(list => new ObservableCollection<ToothModel>(list))
                            .ToList();


            foreach (var tooth in Teeth)
            {
                foreach (var problem in tooth.Problems)
                {
                    var toothProblem = new ToothProblemModel
                    {
                        ToothNumber = tooth.Number,
                        ToothProblem = problem,
                        IsDefault = false
                    };
                    Problems.Add(toothProblem);
                }
            }
            var toothDefaultProblems = Teeth
            .Where(t => t.ApicalReaction || t.Decay)
            .Select(t => new ToothProblemModel
            {
                ToothNumber = t.Number,
                ToothProblem = t.DefaultProblem,
                IsDefault = true
            }) ;

            foreach (var problem in toothDefaultProblems)
            {
                Problems.Add(problem);
            }

            
            if(PatientRecord.Problems == null)
            {
                PatientRecord.Problems = new ObservableCollection<string>();
            }
            foreach (var problem in PatientRecord.Problems)
            {
                var toothProblem = new ToothProblemModel
                {
                    ToothNumber = 0,
                    ToothProblem = problem,
                    IsDefault = false
                };
                Problems.Add(toothProblem);
            }

        }
        private void ExecuteGoBackInHistory(object obj)
        {
            HistoryIndex++;
            NavigateInHistory(HistoryIndex);
            IsEditing = false;

        }

        private void NavigateInHistory(int historyIndex)
        {
            PatientRecord = PatientRecordHistory[historyIndex];
            PatientRecord.PatientId = Patient.Id;
            Teeth = new ObservableCollection<ToothModel>(TeethHistory[historyIndex]);

            Problems.Clear();


            var toothDefaultProblems = Teeth
            .Where(t => t.ApicalReaction || t.Decay)
            .Select(t => new ToothProblemModel
            {
                ToothNumber = t.Number,
                ToothProblem = t.DefaultProblem,
                IsDefault = true
            });

            foreach (var problem in toothDefaultProblems)
            {
                Problems.Add(problem);
            }

            foreach (var tooth in Teeth)
            {
                foreach (var problem in tooth.Problems)
                {
                    var toothProblem = new ToothProblemModel
                    {
                        ToothNumber = tooth.Number,
                        ToothProblem = problem,
                        IsDefault = false
                    };
                    Problems.Add(toothProblem);
                }
            }
            if (PatientRecord.Problems == null)
            {
                PatientRecord.Problems = new ObservableCollection<string>();
            }
            foreach (var problem in PatientRecord.Problems)
            {
                var toothProblem = new ToothProblemModel
                {
                    ToothNumber = 0,
                    ToothProblem = problem,
                    IsDefault = false
                };
                Problems.Add(toothProblem);
            }
        }

        private void ExecuteGoForwardInHistory(object obj)
        {
            HistoryIndex--;
            NavigateInHistory(HistoryIndex);
            IsEditing = false;

        }




        private bool CanExecuteAddProblemSelectedTooth(object obj)
        {
            return (SelectedTooth != null  &&  IsEditing && !string.IsNullOrWhiteSpace(SelectedToothProblem));
        }

        private void ExecuteAddProblemSelectedTooth(object obj)
        {
            SelectedTooth?.Problems.Add(SelectedToothProblem);
            Problems.Add(new ToothProblemModel { IsDefault = false , ToothNumber = SelectedTooth.Number, ToothProblem = SelectedToothProblem });
            SelectedToothProblem = "";

        }

        private void ExecuteRemoveProblemSelectedTooth(object obj)
        {
            if (obj is string toothProblem)
            {
                SelectedTooth?.Problems.Remove(toothProblem);
                var item = Problems.FirstOrDefault(p => p.ToothNumber == SelectedTooth.Number && p.ToothProblem == toothProblem);
                if(item !=null)
                    Problems.Remove(item);

            }else
            {
                MessageBox.Show(obj.GetType().Name);
            }
        }



        private void ExecuteRemoveProblem(object obj)
        {
            if (obj is ToothProblemModel toothProblem)
            {
                Problems.Remove(toothProblem);
                if (Problem.ToothNumber != 0)
                    Teeth[toothProblem.ToothNumber - 1].Problems.Remove(toothProblem.ToothProblem ?? "");
                else
                {
                    PatientRecord.Problems?.Remove(toothProblem.ToothProblem ?? "");
                }
            }
        }

        private void ExecuteGoToTooth(object obj)
        {
            if (obj is int toothNumber)
            {
                SelectedTooth = Teeth[toothNumber - 1];
            }
        }


        private void ExecuteAddProblem(object obj)
        {
            Problems.Add(Problem);
            if(Problem.ToothNumber != 0)
                Teeth[Problem.ToothNumber - 1].Problems.Add(Problem.ToothProblem?? "");
            else
            {
                PatientRecord.Problems?.Add(Problem.ToothProblem ?? "");
            }
            Problem = new ToothProblemModel();
            Problem.ToothNumber = 0;
            Problem.ToothProblem = "";
        }
         private bool CanExecuteAddProblem(object obj)
        {
            return (IsEditing && !string.IsNullOrEmpty(Problem.ToothProblem));
        }

        private void ExecuteRemovePatient(object obj)
        {
            if (DialogService.ShowYesNoDialog() == true)
            {
                UserPrincipal.PatientRepository.Remove(Patient);
                Patient = new PatientModel();
                PatientRecord = new PatientRecordModel();
                Teeth = new ObservableCollection<ToothModel>(Enumerable.Range(1, 32).Select(i => new ToothModel { Number = i }));
                LastName = "";
                FirstName = "";
                Surname = "";
                IsMale = null;
                IsFemale = null;
                Phone = "";
                PhoneAlt = "";
                Email = "";
                BirthDate = new DateOnly();
                Profession = "";
                Adress = "";
                Motive = "";
                OrientedBy = "";
                Adress = "";
                PreferredDay = "";
                ParentName = "";
                IsNewPatient = true;
                IsEditing = true;
            }
        }

        private void ExecuteAddOrdonnance(object obj)
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            if (DialogService.Show(new OrdonnanceViewModel(Patient)) == true)
            {
            }
        }

        private void ExecuteAddAppointment(object obj)
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            var appointment = new AppointmentModel() { PatientId = Patient.Id, Date = now, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(13, 0) };
            if (DialogService.Show(new AppointmentEditViewModel(new AppointmentViewModel(appointment))) == true)
            {
                UserPrincipal.AppointmentRepository.Add(appointment);
            }
        }

       

        private void ExecuteSaveEdit(object obj)
        {
            if (DialogService.ShowYesNoDialog() == true)
            {
                
                if (IsNewPatient)
                {
                    Patient.Id = UserPrincipal.PatientRepository.Add(Patient);
                    PatientRecord.PatientId = Patient.Id;
                    UserPrincipal.PatientRecordRepository.Add(PatientRecord);
                    UserPrincipal.ToothRepository.AddAll(Teeth.ToList(), PatientRecord.PatientId);
                    IsNewPatient = false;
                }
                else
                {
                    UserPrincipal.PatientRepository.Edit(Patient);
                    UserPrincipal.PatientRecordRepository.Edit(PatientRecord);
                    if (Teeth != null)
                        UserPrincipal.ToothRepository.EditAll(Teeth.ToList(), Patient.Id);
                }
                PatientRecordHistory = (List<PatientRecordModel>)UserPrincipal.PatientRecordRepository.GetAllFromHistory(Patient.Id);
                List<IEnumerable<ToothModel>> toothModelLists = (List<IEnumerable<ToothModel>>)UserPrincipal.ToothRepository.GetAllFromHistory((int)PatientRecord.PatientId);
                TeethHistory = toothModelLists
                                .Select(list => new ObservableCollection<ToothModel>(list))
                                .ToList();

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
                && IsEditing
                && !IsInHistory;

        }

    }
}
