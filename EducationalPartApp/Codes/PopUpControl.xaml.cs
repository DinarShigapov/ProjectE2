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

namespace EducationalPartApp.Codes
{
    /// <summary>
    /// Логика взаимодействия для PopUpControl.xaml
    /// </summary>
    public partial class PopUpControl : ContentControl
    {
        public PopUpControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
    "Text", typeof(string), typeof(PopUpControl), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window == null)
                return;
            PopUpControl popUp = window.FindName("popup") as PopUpControl;

            popUp.Visibility = Visibility.Collapsed;
        }
    }
}
