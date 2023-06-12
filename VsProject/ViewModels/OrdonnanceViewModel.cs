using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Data;

namespace VsProject.ViewModels
{
    public class OrdonnanceViewModel : ViewModelBase
    {
        public class OrdonnanceMedicationModel
        {
            public string Name { get; set; } = "";
            public string Dosage { get; set; } = "";
            public string Frequency { get; set; } = "";
        }
        private ObservableCollection<OrdonnanceMedicationModel> _medications = new ObservableCollection<OrdonnanceMedicationModel>();
        private ObservableCollection<OrdonnanceMedicationModel> _medicationsAdded = new ObservableCollection<OrdonnanceMedicationModel>();
        private string _patientName;
        private int _age;
        private string _notes;


        private ICollectionView _medicationCollectionView;
        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                _medicationCollectionView.Refresh(); // Refresh the collection view when search text changes
                OnPropertyChanged(nameof(Search));
            }
        }

        public string PatientName
        {
            get => _patientName;
            set
            {
                _patientName = value;
                OnPropertyChanged(nameof(PatientName));
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }


        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }



        public ObservableCollection<OrdonnanceMedicationModel> Medications
        {
            get => _medications;
            set
            {
                _medications = value;
                OnPropertyChanged(nameof(Medications));
            }
        }
        public ObservableCollection<OrdonnanceMedicationModel> MedicationsAdded
        {
            get => _medicationsAdded;
            set
            {
                _medicationsAdded = value;
                OnPropertyChanged(nameof(MedicationsAdded));
            }
        }

        public ICommand AddMedicationCommand { get; }
        public ICommand RemoveMedicationCommand { get; }
        public ICommand GeneratePdfCommand { get; }

        public OrdonnanceViewModel()
        {
            
            var meds = new ObservableCollection<MedicationModel>(UserPrincipal.MedicationRepository.GetAll());
            foreach(var med in meds)
            {
                Medications.Add(new OrdonnanceMedicationModel() { Name= med.Name });
            }

            _medicationCollectionView = CollectionViewSource.GetDefaultView(Medications);
            _medicationCollectionView.Filter = FilterBySearchText;
            Notes = "";
            AddMedicationCommand = new ViewModelCommand(AddMedication);
            RemoveMedicationCommand = new ViewModelCommand(RemoveMedication);
            GeneratePdfCommand = new ViewModelCommand(GeneratePdf);
        }
        
        public OrdonnanceViewModel(PatientModel patient)
        {
            var meds = new ObservableCollection<MedicationModel>(UserPrincipal.MedicationRepository.GetAll());
            foreach (var med in meds)
            {
                Medications.Add(new OrdonnanceMedicationModel() { Name = med.Name });
            }
            _medicationCollectionView = CollectionViewSource.GetDefaultView(Medications);
            _medicationCollectionView.Filter = FilterBySearchText;
            PatientName = patient.LastName + " " + patient.FirstName;
            int age = DateTime.Today.Year - patient.BirthDate.Year;

            // Check if the birthday has already occurred this year
            if (DateTime.Today < patient.BirthDate.ToDateTime(new TimeOnly()).AddYears(age))
            {
                age--;
            }
            Age = age;
            Notes = "";
            AddMedicationCommand = new ViewModelCommand(AddMedication, CanAddMedication);
            RemoveMedicationCommand = new ViewModelCommand(RemoveMedication);
            GeneratePdfCommand = new ViewModelCommand(GeneratePdf, CanGeneratePdf);
        }


        private void AddMedication(object parameter)
        {
            if (parameter is OrdonnanceMedicationModel medication)
            {
                MedicationsAdded.Add(new OrdonnanceMedicationModel { Name = medication.Name , Dosage = medication.Dosage , Frequency = medication.Frequency});
            }
        }
        private bool CanAddMedication(object parameter)
        {
            if (parameter is OrdonnanceMedicationModel medication)
            {
                return !(string.IsNullOrEmpty(medication.Name) || string.IsNullOrEmpty(medication.Dosage));
            }
            return false;
        }

        private void RemoveMedication(object parameter)
        {
            if (parameter is OrdonnanceMedicationModel medication)
            {
                MedicationsAdded.Remove(medication);
            }
        }

        private void GeneratePdf(object parameter)
        {
            // Create a new PDF document
            var document = new PdfSharp.Pdf.PdfDocument();

            // Create a new page
            var page = document.AddPage();

            // Create a PDF graphics object for drawing
            using (var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
            {
                // Set the font and size
                var font = new PdfSharp.Drawing.XFont("Calibri", 14, PdfSharp.Drawing.XFontStyle.Regular);
                var boldFont = new PdfSharp.Drawing.XFont("Calibri", 14, PdfSharp.Drawing.XFontStyle.Bold);

                // Get the page dimensions
                var pageWidth = page.Width.Point;
                var pageHeight = page.Height.Point;

                // Calculate the center coordinates
                var centerX = pageWidth / 2;
                var centerY = pageHeight / 2;

                // Calculate the text dimensions
                var lineHeight = font.GetHeight();
                var textWidth = graphics.MeasureString("Nom:   " + PatientName + "                                      Age:   " + Age.ToString() + " ans" + PatientName, font).Width;

                // Calculate the total height of the text
                var totalTextHeight = MedicationsAdded.Count * lineHeight;

                // Calculate the available space for vertical centering
                var availableSpace = pageHeight - totalTextHeight;

                // Calculate the initial text position for vertical centering
                var textX = centerX - (centerX / 1.5);
                var textY = centerY - (centerY / 2);

                // Draw the patient name
                graphics.DrawString ("Nom:   " + PatientName + "                                   Age:   " + Age.ToString() + " ans", boldFont, PdfSharp.Drawing.XBrushes.Black, textX, textY);
                textY += lineHeight * 6;

                // Draw the medications added
                foreach (var medication in MedicationsAdded)
                {
                    var ntextX = textX;
                    graphics.DrawString($"- {medication.Name}", font, PdfSharp.Drawing.XBrushes.Black, ntextX, textY);
                    var ntextWidth = graphics.MeasureString($"- {medication.Name}", font).Width;
                    ntextX += ntextWidth + textX / 4;
                    graphics.DrawString($"{medication.Dosage} :", font, PdfSharp.Drawing.XBrushes.Black, ntextX, textY);
                    ntextWidth = graphics.MeasureString($"{medication.Dosage} :", font).Width;
                    if (!string.IsNullOrWhiteSpace(medication.Frequency))
                    {
                        ntextX += ntextWidth + textX / 4;
                        graphics.DrawString($"{medication.Frequency}", font, PdfSharp.Drawing.XBrushes.Black, ntextX, textY);
                    }
                    textY += lineHeight * 1.2;

                }
                textY += lineHeight * 10;
                if (!string.IsNullOrEmpty(Notes))
                {
                    // Wrap and draw the notes
                    var notesX = textX;
                    var notesWidth = pageWidth - notesX * 1.5;
                    var notesY = textY;
                    var notesHeight = pageHeight - notesY;
                    var notesfont = new PdfSharp.Drawing.XFont("Calibri", 12, PdfSharp.Drawing.XFontStyle.Regular);
                    var notesRect = new PdfSharp.Drawing.XRect(notesX, notesY, notesWidth, notesHeight);
                    var notesFormatter = new PdfSharp.Drawing.Layout.XTextFormatter(graphics);
                    notesFormatter.DrawString("Notes: \n" + Notes, notesfont, PdfSharp.Drawing.XBrushes.Black, notesRect);
                }

            }

            // Save the PDF to a file
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                document.Save(saveFileDialog.FileName);
            }
        }
        private bool CanGeneratePdf(object parameter)
        {
            return MedicationsAdded.Count > 0;
        }

            private bool FilterBySearchText(object item)
        {
            if (string.IsNullOrEmpty(_search))
                return true;

            var medication = item as OrdonnanceMedicationModel;
            if (medication == null)
                return false;

            var fullName = $"{medication.Name}".ToLower();

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
