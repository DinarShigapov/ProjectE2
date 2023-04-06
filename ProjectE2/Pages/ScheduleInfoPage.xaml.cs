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
using ProjectE2.Model;
using ProjectE2.Pages;

namespace ProjectE2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ScheduleInfoPage.xaml
    /// </summary>
    public partial class ScheduleInfoPage : Page
    {
        Schedule contextSchedule;
        public ScheduleInfoPage(Schedule schedule)
        {
            InitializeComponent();
            contextSchedule = schedule;
            DataContext = contextSchedule;

        }
    }
}
