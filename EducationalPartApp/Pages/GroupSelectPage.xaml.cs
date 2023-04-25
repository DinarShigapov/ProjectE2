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

namespace EducationalPartApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupSelectPage.xaml
    /// </summary>
    public partial class GroupSelectPage : Page
    {
        public GroupSelectPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var frame = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            if (frame.GetType() == typeof(MainWindow))
            {
                var frameContent = (frame as MainWindow).MainFrame;
                if (frameContent.Content.GetType() == typeof(MenuPage)) 
                {
                    var menuFrame = (frameContent.Content as MenuPage).FindName("MenuFrame") as Frame;
                    menuFrame.Navigate(new AddSchedulePage());
                }
            }
            MaterialDesignThemes.Wpf.DialogHost popUp = frame.FindName("DialogHostModal") as MaterialDesignThemes.Wpf.DialogHost;
            popUp.IsOpen = false;


        }
    }
}
