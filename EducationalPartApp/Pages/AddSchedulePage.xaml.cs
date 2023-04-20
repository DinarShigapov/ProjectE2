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
        //List<Schedule> schedules = new List<Schedule>();
        //List<Subgroup> subgroups = new List<Subgroup>();



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
            for (int i = 0; i < 5; i++)
            {
                var scheduleMondayList = App.DB.Schedule.Where(x => x.DayOfTheWeekId == i + 1 && x.GroupId == 1).ToList();
                foreach (var item in scheduleMondayList)
                {
                    var buffer = new ScheduleListClass { schedule = item, subgroups = item.Subgroup.ToList() };
                    scheduleDayOfTheWeek[i].Add(buffer);
                }
            }
            RefreshSchedule(0);
            
        }

        private void RefreshSchedule(int i) 
        {
            DisciplineOneLesson.DataContext = null;
            TeacherOneLesson.Text = "";
            DisciplineTwoLesson.DataContext = null;
            TeacherTwoLesson.Text = "";
            DisciplineThreeLesson.DataContext = null;
            TeacherThreeLesson.Text = "";
            DisciplineFourLesson.DataContext = null;
            TeacherFourLesson.Text = "";
            DisciplineFiveLesson.DataContext = null;
            TeacherFiveLesson.Text = "";

            var context = scheduleDayOfTheWeek[i];
            for (int j = 0; j < context.Count; j++)
            {
                if (context[j] == null) continue;
                var scheduleBuffer = context[j].schedule;
                var subgroupsBuffer = context[j].subgroups;
                switch (j)
                {
                    case 0:
                        DisciplineOneLesson.DataContext = scheduleBuffer;
                        TeacherOneLesson.Text = StringTeacherList(subgroupsBuffer);
                        break;
                    case 1:
                        DisciplineTwoLesson.DataContext = scheduleBuffer;
                        TeacherTwoLesson.Text = StringTeacherList(subgroupsBuffer);
                        break;
                    case 2:
                        DisciplineThreeLesson.DataContext = scheduleBuffer;
                        TeacherThreeLesson.Text = StringTeacherList(subgroupsBuffer);
                        break;
                    case 3:
                        DisciplineFourLesson.DataContext = scheduleBuffer;
                        TeacherFourLesson.Text = StringTeacherList(subgroupsBuffer);
                        break;
                    case 4:
                        DisciplineFiveLesson.DataContext = scheduleBuffer;
                        TeacherFiveLesson.Text = StringTeacherList(subgroupsBuffer);
                        break;
                    default:
                        break;
                }
            }
        }

        private string StringTeacherList(List<Subgroup> subgroups) 
        {
            string buffer = "";
            foreach (var item in subgroups)
            {

                buffer += $"{App.DB.Employee.FirstOrDefault(x => x.Id == item.TeacherId).FullNameShort} / ";

            }
            return buffer.TrimEnd(' ', '/');
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
                if (scheduleDayOfTheWeek[i].Count == 0)
                {
                    var itemBuffer = item as Grid;
                    itemBuffer.Children.OfType<Ellipse>().FirstOrDefault().Visibility = Visibility.Visible;
                }
                i++;
            }
        }

        private void BEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window == null)
                return;

            PopUpControl popUp = window.FindName("popup") as PopUpControl;
            var popUpFrame = popUp.Content as Frame;
            //popUpFrame.Navigate(new AuthPage());
            popUp.Visibility = Visibility.Visible;
        }
    }
}
