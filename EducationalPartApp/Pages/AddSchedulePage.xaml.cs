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
using EducationalPartApp.Model;
using EducationalPartApp.Codes;

namespace EducationalPartApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddSchedulePage.xaml
    /// </summary>
    public partial class AddSchedulePage : Page
    {


        List<List<ScheduleListClass>> scheduleDayOfTheWeek = new List<List<ScheduleListClass>>(6)
        {
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ }
        };

        public AddSchedulePage()
        {
            InitializeComponent();
            for (int i = 0; i <= 5; i++)
            {
                for (int h = 0; h < 5; h++)
                {
                    scheduleDayOfTheWeek[i].Add(new ScheduleListClass() { schedule = new Schedule { ClassTimeId = h + 1 } });
                }
            }
            RefreshSchedule(0);
        }

        private void RefreshSchedule(int i) 
        {
            LVLesson.ItemsSource = scheduleDayOfTheWeek[i].ToList();
        }


        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index;
            switch ((sender as RadioButton).Content.ToString())
            {
                case "ПН":
                    index = 0;
                    break;
                case "ВТ":
                    index = 1;
                    break;
                case "СР":
                    index = 2;
                    break;
                case "ЧТ":
                    index = 3;
                    break;
                case "ПТ":
                    index = 4;
                    break;
                case "СБ":
                    index = 5;
                    break;
                default:
                    index = 0;
                    break;
            }
            RefreshSchedule(index);
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {

            int i = 0;
            foreach (var item in SPDayOfTheWeek.Children)
            {
                if (scheduleDayOfTheWeek[i].All(x => x.schedule.Discipline == null) == true)
                {
                    var itemBuffer = item as Grid;
                    itemBuffer.Children.OfType<Ellipse>().FirstOrDefault().Visibility = Visibility.Visible;
                }
                else 
                {
                    (item as Grid).Children.OfType<Ellipse>().FirstOrDefault().Visibility = Visibility.Hidden;
                }
                i++;
            }

        }


        private void BEditTeacher_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BEditLesson_Click(object sender, RoutedEventArgs e)
        {
            var dsds = (sender as Button).DataContext as ScheduleListClass;
            var window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);

            window.GetFrameWindow(new EditLessonPage(dsds));
        }
    }
}
