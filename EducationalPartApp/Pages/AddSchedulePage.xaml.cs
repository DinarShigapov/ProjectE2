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
        private Schedule _copyLesson;
        private int _currentIndexDay = 0;
        private const int _defualtIndex = -1;
        private int _lessonOneSwitch = _defualtIndex;
        private int _lessonTwoSwitch = _defualtIndex;
        private string _switchInfo = "";
        private bool _isCreated;
        private Dictionary<Discipline, List<Employee>> _disciplineTeacher = new Dictionary<Discipline, List<Employee>>(); 

        List<List<Schedule>> scheduleList = new List<List<Schedule>>(6)
        {
            new List<Schedule>(){ },
            new List<Schedule>(){ },
            new List<Schedule>(){ },
            new List<Schedule>(){ },
            new List<Schedule>(){ },
            new List<Schedule>(){ }
        };


        public AddSchedulePage(Group group, bool IsCreated)
        {
            InitializeComponent();
            contextGroup = group;
            _isCreated = IsCreated;
            GroupSelect.Text = contextGroup.StrFullName;
            if (IsCreated)
            {
                FillList(group);
            }
            else
            {
                FillList();
            }
            GetDisciplne();
            RefreshSchedule(0);
        }

        private void GetDisciplne()
        {
            var disciplineList = App.DB.Curriculum.Where(x =>
                x.SpecializationId == contextGroup.Qualification.SpecializationId &&
                x.SemesterId == contextGroup.Semester.Id).Select(x => x.Discipline).ToList();

            foreach (var item in disciplineList)
            {
                _disciplineTeacher.Add(item, new List<Employee>());
            }
        }

        private void FillList() 
        {
            for (int i = 0; i <= 5; i++)
            {
                for (int h = 0; h < App.DB.ClassTime.Count(); h++)
                {
                    scheduleList[i].Add(new Schedule()
                    {
                        ClassTimeId = h + 1,
                        Group = contextGroup,
                        DayOfTheWeekId = i + 1,
                        Semester = contextGroup.Semester
                    });
                }
            }
        }

        private void FillList(Group group)
        {
            for (int i = 0; i <= 5; i++)
            {
                for (int h = 0; h < App.DB.ClassTime.Count(); h++)
                {
                    var schedule = App.DB.Schedule.FirstOrDefault(x => 
                        x.ClassTimeId == h + 1 &&
                        x.Group.Id == group.Id &&
                        x.DayOfTheWeekId == i + 1 &&
                        x.SemesterId == group.SemesterId);
                    if (schedule != null)
                    {
                        scheduleList[i].Add(schedule);
                    }
                    else
                    {
                        scheduleList[i].Add(new Schedule()
                        {
                            ClassTimeId = h + 1,
                            Group = contextGroup,
                            DayOfTheWeekId = i + 1,
                            Semester = contextGroup.Semester
                        });
                    }
                }
            }
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
                default:
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
                var ellipseVisibility = (item as Grid).Children.OfType<Ellipse>().FirstOrDefault().Visibility;
                foreach (var item1 in scheduleList[i])
                {
                    if (item1.Discipline != null)
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
            if (IsCheckSchedule() == false) return;

            for (int h = 0; h < scheduleList.Count; h++)
            {
                for (int g = 0; g < scheduleList[h].Count; g++)
                {
                    var scheduleDay = scheduleList[h][g];

                    if (scheduleDay.Discipline == null)
                        continue;
                    if (scheduleDay.Id == 0)
                    {
                        scheduleDay.Date = DateTime.Now;
                        App.DB.Schedule.Add(scheduleDay);
                    }
                }
            }
            try
            {
                App.DB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (!_isCreated)
            {
                AddReportCard();
            }
        }


        private void AddReportCard() 
        {

            foreach (var item in _disciplineTeacher)
            {
                List<ReportCardTeacher> reportCards =
                    item.Value.Select(x => new ReportCardTeacher{ Employee = x}).ToList();

                App.DB.ReportCard.Add(new ReportCard
                {
                    Group = contextGroup,
                    Discipline = item.Key,
                    DateOfCreation = DateTime.Now,
                    ReportCardTeacher = reportCards,
                    Semester = contextGroup.Semester,
                    IsActive = true
                });
            }
            App.DB.SaveChanges();
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
                _switchInfo += $"Cменить {_lessonOneSwitch + 1} ⇄";
                TBSwitchInfo.Text += _switchInfo;
            }
            else 
            {
                _lessonTwoSwitch = LVLesson.SelectedIndex;
            }

            if (_lessonTwoSwitch != _defualtIndex && _lessonOneSwitch != _defualtIndex)
            {
                var lessonSwitch = scheduleList[_currentIndexDay];

                lessonSwitch[_lessonOneSwitch].ClassTimeId = _lessonTwoSwitch + 1;
                lessonSwitch[_lessonTwoSwitch].ClassTimeId = _lessonOneSwitch + 1;

                scheduleList[_currentIndexDay] = lessonSwitch.OrderBy(x => x.ClassTimeId).ToList();

                LVLesson.ItemsSource = null;
                LVLesson.ItemsSource = scheduleList[_currentIndexDay];
                ClearSwitch();
            }
        }

        private void LVLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_switchInfo != "")
            {
                TBSwitchInfo.Text = _switchInfo;
                TBSwitchInfo.Text += $" {LVLesson.SelectedIndex + 1}";
            }
        }

        private void BEditLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectLesson = (sender as Button).DataContext as Schedule;
            if (selectLesson == null)
                return;
            OpenEditorPage(selectLesson);
        }

        // Context Menu ListView
        private void MIEditLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectLesson = (sender as MenuItem).DataContext as Schedule;
            if (selectLesson == null)
                return;
            OpenEditorPage(selectLesson);
        }
        private void MICopyLesson_Click(object sender, RoutedEventArgs e)
        {
            var copyLessonBuffer = (sender as MenuItem).DataContext as Schedule;
            if (copyLessonBuffer == null 
                || copyLessonBuffer.Discipline == null)
                return;

            _copyLesson = copyLessonBuffer.Clone();
        }
        private void MIPasteLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectLesson = LVLesson.SelectedItem as Schedule;
            if (_copyLesson == null
                && LVLesson.SelectedIndex != _defualtIndex)
                return;

            var subgroupsNow = App.DB.Subgroup.Where(
                    x => x.Schedule.DayOfTheWeekId == selectLesson.DayOfTheWeekId
                    && x.Schedule.ClassTimeId == selectLesson.ClassTimeId
                    && x.Schedule.SemesterId == _copyLesson.Group.Semester.Id
                    && x.Schedule.Date.Year == DateTime.Now.Year).ToList();

            foreach (var item in subgroupsNow.ToList())
            {
                if (_copyLesson.Subgroup.FirstOrDefault(x => x.Employee == item.Employee) != null)
                {
                    MessageBox.Show($"В это время {item.Employee.FullNameShort} проводит урок у {item.Schedule.Group.StrFullName}");
                    return;
                }

                if (_copyLesson.Subgroup.FirstOrDefault(x => x.Auditorium == item.Auditorium) != null)
                {
                    MessageBox.Show($"В это время каб. {item.Auditorium.Name} занят");
                    return;
                }
            }

            var buffer = scheduleList[_currentIndexDay];

            var copyBuffer = _copyLesson.Clone();

            buffer[LVLesson.SelectedIndex].Discipline = copyBuffer.Discipline;
            buffer[LVLesson.SelectedIndex].Subgroup = copyBuffer.Subgroup;

            LVLesson.ItemsSource = null;
            LVLesson.ItemsSource = buffer.ToList();
        }

        private void MIDeleteLesson_Click(object sender, RoutedEventArgs e)
        {
            var selectIndex = LVLesson.SelectedIndex;
            var selectLesson = LVLesson.SelectedItem as Schedule;
            if (selectIndex == _defualtIndex ||
                selectLesson.Discipline == null)
                return;


            var buffer = scheduleList[_currentIndexDay];
            buffer[selectIndex].Discipline = null;
            buffer[selectIndex].Subgroup.Clear();


            LVLesson.ItemsSource = null;
            LVLesson.ItemsSource = scheduleList[_currentIndexDay].ToList();
        }


        private void OpenEditorPage(Schedule sender)
        {
            ClearSwitch();
            new MainWindow().GetFrameWindow(new EditLessonPage(sender, _disciplineTeacher));
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

        private void MIRadioLesson_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            window.GetFrameWindow(new SelectLessonPage(scheduleList));
        }
    }
}
