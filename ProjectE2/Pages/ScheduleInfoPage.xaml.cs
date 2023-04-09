using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        List<Employee> teachers = new List<Employee>();
        int i = 0;
        public ScheduleInfoPage(Schedule schedule)
        {
            InitializeComponent();
            contextSchedule = schedule;
            DataContext = contextSchedule;

            foreach (var item in schedule.Subgroup.ToList())
            {
                teachers.Add(App.DB.Employee.FirstOrDefault(x => x.Id == item.TeacherId));
            }

            if (teachers.Count == 1)
                SwitchButton.Visibility = Visibility.Hidden;

            BorderContext.DataContext = teachers[i];
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == BNext)
            {
                if (i == teachers.Count - 1)
                    i = 0;
                else i++;
            }
            else 
            {
                if (i == 0)
                    i = teachers.Count - 1;
                else i--;
            }
            BorderContext.DataContext = teachers[i];
        }
    }
}
