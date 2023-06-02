using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VsProject.Services;

namespace VsProject.Resources.Controls
{
    /// <summary>
    /// Interaction logic for NavListBoxItem.xaml
    /// </summary>
    public partial class NavListBoxItem : ListBoxItem
    {

        public static readonly DependencyProperty ViewModelProperty =
          DependencyProperty.Register("ViewModel", typeof(Type), typeof(NavListBoxItem), new PropertyMetadata(default));
        public Type ViewModel
        {
            get { return (Type)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public Type? ParentType => NavService.GetParentTypeFromViewModelType(ViewModel); 
        



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
        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavService.Navigate(ViewModel);
        }
    }
}
