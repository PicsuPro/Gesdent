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
    public partial class NavTabItem : TabItem
    {

        public static readonly DependencyProperty ViewModelProperty =
          DependencyProperty.Register("ViewModel", typeof(Type), typeof(NavTabItem), new PropertyMetadata(default));
        public Type ViewModel
        {
            get { return (Type)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }        



        public NavTabItem()
        {
            InitializeComponent();
        }

    }
}
