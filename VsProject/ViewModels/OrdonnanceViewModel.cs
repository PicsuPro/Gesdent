using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class OrdonnanceViewModel : ViewModelBase
    {
        private OrdonnanceModel _ordonnance;
        private ObservableCollection<MedicationModel> _medications;
        private int? _age;

        public int? Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }


        public OrdonnanceModel Ordonnance
        {
            get => _ordonnance;
            set
            {
                _ordonnance = value;
                OnPropertyChanged(nameof(Ordonnance));
            }
        }

        public ObservableCollection<MedicationModel> Medications
        {
            get => _medications;
            set
            {
                _medications = value;
                OnPropertyChanged(nameof(Medications));
            }
        }

        public ICommand AddMedicationCommand { get; }
        public ICommand RemoveMedicationCommand { get; }
        public ICommand GeneratePdfCommand { get; }

        public OrdonnanceViewModel()
        {
            Ordonnance = new OrdonnanceModel();
            Medications = new ObservableCollection<MedicationModel>();

            AddMedicationCommand = new ViewModelCommand(AddMedication);
            RemoveMedicationCommand = new ViewModelCommand(RemoveMedication);
            GeneratePdfCommand = new ViewModelCommand(GeneratePdf);
        }

        private void AddMedication(object parameter)
        {
            Medications.Add(new MedicationModel());
        }

        private void RemoveMedication(object parameter)
        {
            if (parameter is MedicationModel medication)
            {
                if (DialogService.ShowYesNoDialog() == true)
                {
                    Medications.Remove(medication);
                }
            }
        }
        private void SavePdfToFile(string pdfContent, string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = Path.Combine(desktopPath, $"{fileName}.pdf");
            File.WriteAllText(filePath, pdfContent);
        }


        private void GeneratePdf(object parameter)
        {
            try
            {
                // Prompt the user to enter a file name
                string fileName = Microsoft.VisualBasic.Interaction.InputBox("Enter the file name:", "Save PDF", $"{Ordonnance.PatientName}");

                // Generate the PDF using the ordonnance and medications data
                string pdfContent = GeneratePdfContent();
                SavePdfToFile(pdfContent, fileName);
            }
            catch (Exception ex)
            {
                // Output the exception message for debugging
                Console.WriteLine(ex.Message);
            }
        }



        private string GeneratePdfContent()
        {
            StringBuilder contentBuilder = new StringBuilder();

            // Add patient information
            contentBuilder.AppendLine("Patient Information:");
            contentBuilder.AppendLine($"Name: {Ordonnance.PatientName}");
            contentBuilder.AppendLine($"Age: {Ordonnance.Age}");
            contentBuilder.AppendLine($"Notes: {Ordonnance.Notes}");
            contentBuilder.AppendLine();

            // Add medication information
            contentBuilder.AppendLine("Medications:");
            foreach (MedicationModel medication in Medications)
            {
                contentBuilder.AppendLine($"Name: {medication.Name}");
                contentBuilder.AppendLine($"Dosage: {medication.Dosage}");
                contentBuilder.AppendLine($"Frequency: {medication.Frequency}");
                contentBuilder.AppendLine();
            }

            return contentBuilder.ToString();
        }


        private void SavePdfToFile(string pdfContent)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = Path.Combine(desktopPath, "ordonnance.pdf");
            File.WriteAllText(filePath, pdfContent);
        }
    }
}
