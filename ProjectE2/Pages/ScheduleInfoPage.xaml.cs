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
        ReportCard contextReportCard;
        List<Employee> teachers = new List<Employee>();
        int currentIndex = 0;
        public ScheduleInfoPage(ReportCard reportCard)
        {
            InitializeComponent();
            contextReportCard = reportCard;
            DataContext = contextReportCard;

            foreach (var item in reportCard.ReportCardTeacher.ToList())
            {
                teachers.Add(App.DB.Employee.FirstOrDefault(x => x.Id == item.TeacherId));
            }

            if (teachers.Count == 1)
                SwitchButton.Visibility = Visibility.Hidden;

            if (teachers.Count != 0)
            {
                BorderContext.DataContext = teachers[currentIndex];
            }

            LVMarkStudent.ItemsSource = App.DB.Assessment.Where(x => 
                x.Lesson.ReportCard.Id == contextReportCard.Id &&
                x.StudentId == App.LoggedStudent.Id).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == BNext)
            {
                if (currentIndex == teachers.Count - 1)
                    currentIndex = 0;
                else currentIndex++;
            }
            else 
            {
                if (currentIndex == 0)
                    currentIndex = teachers.Count - 1;
                else currentIndex--;
            }
            BorderContext.DataContext = teachers[currentIndex];
        }

        private void DateLesson_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
               
        }
    }
}
