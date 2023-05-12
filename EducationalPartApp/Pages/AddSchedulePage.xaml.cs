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
        private int _currentIndexDayOfTheWeek = 0;
        private ScheduleListClass _copyLesson; 
        private const int _defualtIndex = -1;
        private int _lessonOneSwitch = _defualtIndex;
        private int _lessonTwoSwitch = _defualtIndex;
        private string _switchInfo = "";

        List<List<ScheduleListClass>> scheduleDayOfTheWeek = new List<List<ScheduleListClass>>(6)
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
                    scheduleDayOfTheWeek[i].Add(new ScheduleListClass() 
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
            _currentIndexDayOfTheWeek = index;
            RefreshSchedule(index);
            ClearSwitch();
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            ClearSwitch();
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
            if (copyLessonBuffer == null)
                return;

            _copyLesson = copyLessonBuffer;

        }

        private void MIPasteLesson_Click(object sender, RoutedEventArgs e)
        {
            if (_copyLesson == null && LVLesson.SelectedIndex != -1)
                return;
             scheduleDayOfTheWeek[_currentIndexDayOfTheWeek][LVLesson.SelectedIndex] = (ScheduleListClass)_copyLesson.Clone();
            scheduleDayOfTheWeek[_currentIndexDayOfTheWeek][LVLesson.SelectedIndex].schedule.ClassTimeId = LVLesson.SelectedIndex + 1;

            LVLesson.ItemsSource = null;
            LVLesson.ItemsSource = scheduleDayOfTheWeek[_currentIndexDayOfTheWeek].ToList();

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


        private void ClearSwitch()
        {
            _lessonOneSwitch = _lessonTwoSwitch = _defualtIndex;
            _switchInfo = string.Empty;
            TBSwitchInfo.Text = string.Empty;
            TBSwitchInfo.Visibility = Visibility.Collapsed;
        }

        private void MISwitchLesson_Click(object sender, RoutedEventArgs e)
        {
            if (_lessonOneSwitch == -1)
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
                var lessonSwitch = scheduleDayOfTheWeek[5];

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

    }
}
