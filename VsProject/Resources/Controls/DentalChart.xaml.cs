using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VsProject.Models;

namespace VsProject.Resources.Controls
{

    public partial class DentalChart : UserControl
    {
        public static readonly DependencyProperty TeethSourceProperty =
        DependencyProperty.Register(
            "TeethSource",
            typeof(ObservableCollection<ToothModel>),
            typeof(DentalChart),
            new PropertyMetadata(new ObservableCollection<ToothModel>(
                                                          Enumerable.Range(1, 32).Select(i => new ToothModel { Number = i })
                                                          )));

        public ObservableCollection<ToothModel> TeethSource
        {
            get { return (ObservableCollection<ToothModel>)GetValue(TeethSourceProperty); }
            set { SetValue(TeethSourceProperty, value); }
        }
        public DentalChart()
        {
            InitializeComponent();
        }

    }
}
