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
using TeacherApp.Model;

namespace TeacherApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShowLessonPage.xaml
    /// </summary>
    public partial class ShowLessonPage : Page
    {
        private List<StudentGrade> _studentGrades = new List<StudentGrade>();
        private Lesson _currentLesson;
        private ReportCard _selectedReportCard;
        private StudentGrade _selectedStudent;

        public ShowLessonPage(ReportCard selectedReportCard)
        {
            InitializeComponent();
            _selectedReportCard = selectedReportCard;
            RefreshGroupSelect(_selectedReportCard);
            CBMark.ItemsSource = App.DB.RaitingSystem.ToList();
        }

        private void RefreshGroupSelect(ReportCard reportCard)
        {
            var dayOfTheWeek = App.DB.Schedule.Where(
                x => x.GroupId == reportCard.GroupId &&
                x.DisciplineId == reportCard.DisciplineId &&
                x.Subgroup.Any(z => z.TeacherId == App.LoggedTeacher.Id)).ToList();

            int MonthNow = DateTime.Now.Month;
            int YearNow = DateTime.Now.Year;

            CBDateLesson.ItemsSource = DaySchedule(
                GetDayLesson(dayOfTheWeek),
                new DateTime(YearNow, MonthNow, 1),
                new DateTime(YearNow, MonthNow,
                DateTime.DaysInMonth(YearNow, MonthNow)));
        }

        private List<DayOfWeek> GetDayLesson(List<Schedule> schedules)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            foreach (var day in schedules)
            {
                DayOfWeek dayBuffer = new DayOfWeek();
                switch (day.DayOfTheWeek.Name.ToString())
                {
                    case "Понедельник":
                        dayBuffer = DayOfWeek.Monday;
                        break;
                    case "Вторник":
                        dayBuffer = DayOfWeek.Tuesday;
                        break;
                    case "Среда":
                        dayBuffer = DayOfWeek.Wednesday;
                        break;
                    case "Четверг":
                        dayBuffer = DayOfWeek.Thursday;
                        break;
                    case "Пятница":
                        dayBuffer = DayOfWeek.Friday;
                        break;
                    case "Суббота":
                        dayBuffer = DayOfWeek.Saturday;
                        break;
                }
                days.Add(dayBuffer);
            }
            return days;
        }

        private void LVStudent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectStudent = LVStudent.SelectedItem as StudentGrade;
            if (selectStudent == null) return;
            GStudentInfo.DataContext = selectStudent;
            CBMark.SelectedItem = selectStudent.Raiting;
            ChBStudent.IsChecked = !selectStudent.IsAttend;
        }
        private void CBDateLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentLesson = null;
            _studentGrades.Clear();
            DateTime selectLessonDate = Convert.ToDateTime((dynamic)CBDateLesson.SelectedValue);

            if (selectLessonDate != null && selectLessonDate != default)
            {

                if (selectLessonDate > DateTime.Now)
                {
                    MessageBox.Show("Данный урок не был еще проведен");
                    CBDateLesson.SelectedIndex = -1;
                    return;
                }
                else
                {
                    var checkedLesson = App.DB.Lesson.Where(x => x.ReportCard.Id == _selectedReportCard.Id && x.IsConducted == true);
                    Lesson lessonBuffer = new Lesson();
                    foreach (var item in checkedLesson)
                    {
                        if (item.DateOfTheLesson.Date == selectLessonDate.Date
                            && item.IsConducted == true)
                        {
                            lessonBuffer = item;
                        }
                    }
                    if (lessonBuffer.DateOfTheLesson.Date != default)
                    {
                        _currentLesson = lessonBuffer;
                        TBLessonTopic.DataContext = _currentLesson;
                        foreach (var item in _currentLesson.ReportCard.Group.Student)
                        {
                            var mark = App.DB.Assessment.FirstOrDefault(x =>
                                x.Student.Id == item.Id &&
                                x.Lesson.Id == _currentLesson.Id);
                            var attendance = App.DB.Attendance.FirstOrDefault(x =>
                                x.Student.Id == item.Id &&
                                x.Lesson.Id == _currentLesson.Id);

                            var studentList = new StudentGrade()
                            {
                                Student = item,
                                IsAttend = attendance == null ? true : false,
                                Raiting = mark.RaitingSystem
                                
                            };
                            _studentGrades.Add(studentList);


                        }
                        LVStudent.ItemsSource = _studentGrades.ToList();
                        BAccept.Content = "Сохранить";
                    }
                    else
                    {
                        Lesson lesson = new Lesson
                        {
                            ReportCard = _selectedReportCard,
                            DateOfTheLesson = selectLessonDate
                        };
                        _currentLesson = lesson;
                        foreach (var item in lesson.ReportCard.Group.Student)
                        {
                            _studentGrades.Add(new StudentGrade()
                            {
                                Student = item,
                                IsAttend = true
                            });
                        }
                        LVStudent.ItemsSource = _studentGrades.ToList();
                        TBLessonTopic.DataContext = lesson;
                        BAccept.Content = "Провести";
                    }
                }
            }
        }

        private void ChBHideStudent_Checked(object sender, RoutedEventArgs e)
        {
            if (ChBHideStudent.IsChecked == true)
            {
                LVStudent.ItemsSource = _studentGrades.Where(x => x.IsAttend == true).ToList();
            }
            else 
            {
                LVStudent.ItemsSource = _studentGrades.ToList();
            }
        }


        private void BAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChBStudent_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent == null)
            {
                ChBHideStudent.IsChecked = false;
                return;
            }
            if (ChBStudent.IsChecked == true)
            {
                CBMark.IsEnabled = false;
                _selectedStudent.IsAttend = false;
            }
            else 
            { 
                CBMark.IsEnabled = true;
                _selectedStudent.IsAttend = true;
            }
        }

        private List<DateTime> DaySchedule(List<DayOfWeek> dayOfTheWeeks, DateTime startdDate, DateTime endDate)
        {
            List<DateTime> daysForSchedule = new List<DateTime>();

            for (; startdDate < endDate;)
            {
                foreach (var item in dayOfTheWeeks)
                {
                    if (startdDate.DayOfWeek == item)
                    {
                        daysForSchedule.Add(startdDate);
                    }
                }

                if (DateTime.DaysInMonth(startdDate.Year, startdDate.Month) < startdDate.Day + 1)
                {
                    startdDate = new DateTime(startdDate.Year, startdDate.Month + 1, 1);
                }
                else
                {
                    startdDate = new DateTime(startdDate.Year, startdDate.Month, startdDate.Day + 1);
                }
            }
            return daysForSchedule = daysForSchedule.GroupBy(x => x).Select(x => x.First()).ToList();
        }

        private void LVStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedStudent = LVStudent.SelectedItem as StudentGrade;
        }

    }

    public class StudentGrade
    {
        public Student Student { get; set; }
        public bool IsAttend { get; set; }
        public RaitingSystem Raiting { get; set; }
    }

}
