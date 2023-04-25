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
    /// Логика взаимодействия для ScheduleListPage.xaml
    /// </summary>
    public partial class ScheduleListPage : Page
    {
        public ScheduleListPage()
        {
            InitializeComponent();
        }

        private void BCreate_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.GetFrameWindow(new GroupSelectPage(), 300, 200);
        }
    }
}
