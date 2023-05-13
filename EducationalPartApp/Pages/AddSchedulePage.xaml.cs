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

        Group contextGroup;
        private int _currentIndexDay = 0;
        private ScheduleListClass _copyLesson;
        private const int _defualtIndex = -1;
        private int _lessonOneSwitch = _defualtIndex;
        private int _lessonTwoSwitch = _defualtIndex;
        private string _switchInfo = "";

        List<List<ScheduleListClass>> scheduleList = new List<List<ScheduleListClass>>(6)
        {
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ },
            new List<ScheduleListClass>(){ }
        };


        public AddSchedulePage(Group group)
        {
            InitializeComponent();
            contextGroup = group;
            GroupSelect.Text = contextGroup.StrFullName;
            for (int i = 0; i <= 5; i++) {
                for (int h = 0; h < 5; h++) {
                    scheduleList[i].Add(new ScheduleListClass()
                    {
                        schedule = new Schedule
                        {
                            ClassTimeId = h + 1,
                            Group = contextGroup,
                            DayOfTheWeekId = i + 1,
                            Semester = contextGroup.Semester
                        }
                    });
                }
            }
            RefreshSchedule(0);
        }

        private void RefreshSchedule(int i)
        {
            LVLesson.ItemsSource = scheduleList[i].ToList();
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
            _currentIndexDay = index;
            RefreshSchedule(index);
            ClearSwitch();
        }

        private bool IsCheckSchedule()
        {
            int quantityLesson = 0;
            bool isFull = true;
            int i = 0;
            foreach (var item in SPDayOfTheWeek.Children)
            {
                var bufferList = 0; 
                foreach (var item1 in scheduleList[i])
                {
                    if (item1.schedule.Discipline != null)
                    {
                        bufferList++;
                        quantityLesson++;
                    }
                }

                if (bufferList < 2)
                {
                    (item as Grid).Children.OfType<Ellipse>().FirstOrDefault().Visibility = Visibility.Visible;
                    isFull = false;
                }
                else
                {
                    (item as Grid).Children.OfType<Ellipse>().FirstOrDefault().Visibility = Visibility.Hidden;
                }
                i++;
            }
            if (quantityLesson * 2 > 36)
            {
                MessageBox.Show("Расписание превышает нагрузку");
                isFull = false;
            }
            return isFull;
        }


        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            ClearSwitch();
            //if (IsCheckSchedule() == false) return;

            for (int h = 0; h < scheduleList.Count; h++)
            {
                for (int g = 0; g < scheduleList[h].Count; g++)
                {
                    var scheduleDay = scheduleList[h][g].schedule;

                    if (scheduleDay.Discipline == null)
                        continue;

                    scheduleDay.Date = DateTime.Now;
                    App.DB.Schedule.Add(scheduleDay);
                    App.DB.SaveChanges();

                    foreach (var item in scheduleList[h][g].subgroups)
                    {
                        item.ScheduleId = scheduleDay.Id;
                        App.DB.Subgroup.Add(item);
                    }
                    App.DB.SaveChanges();
                }
            }

        }

        private void ClearSwitch()
        {
            _lessonOneSwitch = _lessonTwoSwitch = _defualtIndex;
            _switchInfo = string.Empty;
            TBSwitchInfo.Text = string.Empty;
            TBSwitchInfo.Visibility = Visibility.Collapsed;
        }

        private void MISwitchLesson_Click(object sender, RoutedEventArgs e)
        {
            if (_lessonOneSwitch == _defualtIndex)
            {
                _lessonOneSwitch = LVLesson.SelectedIndex;
                TBSwitchInfo.Visibility = Visibility.Visible;
                _switchInfo += $"Cменить {_lessonOneSwitch + 1} ";
                TBSwitchInfo.Text += _switchInfo;
            }
            else 
            {
                _lessonTwoSwitch = LVLesson.SelectedIndex;
            }

            if (_lessonTwoSwitch != _defualtIndex && _lessonOneSwitch != _defualtIndex)
            {
                var lessonSwitch = scheduleList[_currentIndexDay];

                var bufferSelectedLesson = lessonSwitch[_lessonOneSwitch];

                lessonSwitch[_lessonOneSwitch] = lessonSwitch[_lessonTwoSwitch];
                lessonSwitch[_lessonTwoSwitch] = bufferSelectedLesson;

                lessonSwitch[_lessonOneSwitch].schedule.ClassTimeId = _lessonOneSwitch + 1;
                lessonSwitch[_lessonTwoSwitch].schedule.ClassTimeId = _lessonTwoSwitch + 1;

                LVLesson.ItemsSource = null;
                LVLesson.ItemsSource = lessonSwitch;
                ClearSwitch();
            }
        }

        private void LVLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_switchInfo != "")
            {
                TBSwitchInfo.Text = _switchInfo;
                TBSwitchInfo.Text += $"⇄ {LVLesson.SelectedIndex + 1}";
            }
        }

        private void BEditLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectLesson = (sender as Button).DataContext as ScheduleListClass;
            if (selectLesson == null)
                return;
            OpenEditorPage(selectLesson);
        }

        private void MICopyLesson_Click(object sender, RoutedEventArgs e)
        {
            var copyLessonBuffer = (sender as MenuItem).DataContext as ScheduleListClass;
            if (copyLessonBuffer == null 
                || copyLessonBuffer.schedule.Discipline == null)
                return;

            _copyLesson = scheduleList[_currentIndexDay].FirstOrDefault(x => x == copyLessonBuffer);
        }

        private void MIPasteLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectLesson = LVLesson.SelectedItem as ScheduleListClass;
            if (_copyLesson == null
                && LVLesson.SelectedIndex != _defualtIndex)
                return;

            var subgroupsNow = App.DB.Subgroup.Where(
                    x => x.Schedule.DayOfTheWeekId == selectLesson.schedule.DayOfTheWeekId
                    && x.Schedule.ClassTimeId == selectLesson.schedule.ClassTimeId
                    && x.Schedule.SemesterId == _copyLesson.schedule.Group.Semester.Id
                    && x.Schedule.Date.Year == DateTime.Now.Year).ToList();

            foreach (var item in subgroupsNow.ToList())
            {
                if (_copyLesson.subgroups.FirstOrDefault(x => x.Employee == item.Employee) != null)
                {
                    MessageBox.Show($"В это время {item.Employee.FullNameShort} проводит урок у {item.Schedule.Group.StrFullName}");
                    return;
                }

                if (_copyLesson.subgroups.FirstOrDefault(x => x.Auditorium == item.Auditorium) != null)
                {
                    MessageBox.Show($"В это время каб. {item.Auditorium.Name} занят");
                    return;
                }
            }

            var buffer = scheduleList[_currentIndexDay];

            buffer[LVLesson.SelectedIndex].schedule.Discipline = _copyLesson.schedule.Discipline;
            buffer[LVLesson.SelectedIndex].subgroups = _copyLesson.subgroups;

            LVLesson.ItemsSource = null;
            LVLesson.ItemsSource = buffer.ToList();
        }

        private void MIDeleteLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectIndex = LVLesson.SelectedIndex;
            var selectLesson = LVLesson.SelectedItem as ScheduleListClass;
            if (selectIndex == _defualtIndex ||
                selectLesson.schedule.Discipline == null)
                return;


            var buffer = scheduleList[_currentIndexDay];
            buffer[selectIndex].schedule.Discipline = null;
            buffer[selectIndex].subgroups.Clear();


            LVLesson.ItemsSource = null;
            LVLesson.ItemsSource = scheduleList[_currentIndexDay].ToList();
        }

        private void MIEditLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectLesson = (sender as MenuItem).DataContext as ScheduleListClass;
            if (selectLesson == null)
                return;
            OpenEditorPage(selectLesson);
        }

        private void OpenEditorPage(ScheduleListClass sender)
        {
            ClearSwitch();
            new MainWindow().GetFrameWindow(new EditLessonPage(sender));
        }

        private void Border_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var menuItem = (sender as Border).ContextMenu.ItemContainerGenerator.Items[3] as MenuItem;
            if (_copyLesson == null)
            {
                menuItem.IsEnabled = false;
            }
            else
            {
                menuItem.IsEnabled = true;
            }
        }
    }
}
