using System;
using System.Collections.Generic;
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

namespace VsProject.Resources.Controls
{
    /// <summary>
    /// Interaction logic for NavListBoxItem.xaml
    /// </summary>
    public partial class NavListBoxItem : ListBoxItem
    {

        public static readonly DependencyProperty NavlinkProperty =
          DependencyProperty.Register("Navlink", typeof(Uri), typeof(NavListBoxItem), new PropertyMetadata(null));
        public Uri Navlink
        {
            get { return (Uri)GetValue(NavlinkProperty); }
            set { SetValue(NavlinkProperty, value); }
        }



        public static readonly DependencyProperty IconProperty =
      DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavListBoxItem), new PropertyMetadata(null));

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

  

        public NavListBoxItem()
        {
            InitializeComponent();
        }

    }
}
