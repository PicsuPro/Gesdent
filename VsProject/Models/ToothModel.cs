using System.Collections.Generic;
using System.Collections.ObjectModel;
using VsProject.ViewModels;

namespace VsProject.Models
{
    public class ToothModel : ViewModelBase
    {
        private int _number;
        private string? _notes;
        private bool _apicalReaction;
        private bool _decay;
        private ObservableCollection<string> _problems = new ObservableCollection<string>();
        public int Number 
        { get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        public string? Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        public bool ApicalReaction
        {
            get => _apicalReaction;
            set
            {
                _apicalReaction = value;
                OnPropertyChanged(nameof(ApicalReaction));
                OnPropertyChanged(nameof(DefaultProblem));
            }
        }
        public bool Decay
        {
            get => _decay;
            set
            {
                _decay = value;
                OnPropertyChanged(nameof(Decay));
                OnPropertyChanged(nameof(DefaultProblem));
            }
        }
        public string? DefaultProblem
        {
            get 
            {
                var problems = new List<string>();
                if (ApicalReaction)
                    problems.Add("Reaction Apical");
                if (Decay)
                    problems.Add("Carie");
                var res = string.Join(", ", problems);

                    return string.IsNullOrWhiteSpace(res) ? null : res;
            }
            
        }
        public ObservableCollection<string> Problems
        {
            get => _problems;
            set
            {
                _problems = value;
                OnPropertyChanged(nameof(Problems));
            }
            
        }
    }
}
