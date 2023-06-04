using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TeacherApp.ModelAddition;

namespace TeacherApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShowLessonPage.xaml
    /// </summary>
    public partial class ShowLessonPage : Page
    {
        private List<StudentGrade> _studentGrades = new List<StudentGrade>();
        private Lesson _currentLesson;
        private readonly ReportCard _selectedReportCard;
        private StudentGrade _selectedStudent;
        private List<Lesson> _lessonsCurrentSemester;

        public ShowLessonPage(ReportCard selectedReportCard)
        {
            InitializeComponent();
            _selectedReportCard = selectedReportCard;
            CBDateLesson.ItemsSource = RefreshGroupSelect(_selectedReportCard);
            CBMark.ItemsSource = App.DB.RaitingSystem.ToList();
            _lessonsCurrentSemester = App.DB.Lesson.Where(x => x.ReportCard.Id == _selectedReportCard.Id && x.IsConducted == true).ToList();
        }

        private List<DateTime> RefreshGroupSelect(ReportCard reportCard)
        {
            var dayOfTheWeek = App.DB.Schedule.Where(
                x => x.GroupId == reportCard.GroupId &&
                x.DisciplineId == reportCard.DisciplineId &&
                x.Subgroup.Any(z => z.TeacherId == App.LoggedTeacher.Id)).ToList();

            int MonthNow = DateTime.Now.Month;
            int YearNow = DateTime.Now.Year;

           return DaySchedule(
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
            if (_selectedStudent.Student != null)
            {
                GStudentInfo.DataContext = _selectedStudent;
                GStudentInfo.Visibility = Visibility.Visible;
            }
        }


        private List<StudentGrade> GetStudentGrade(Lesson lesson)
        {
            List<StudentGrade> studentGradesBuffer = new List<StudentGrade>();

            if (lesson != null &&
                        lesson.DateOfTheLesson.Date != default)
            {
                foreach (var item in lesson.ReportCard.Group.Student)
                {
                    var mark = App.DB.Assessment.FirstOrDefault(x =>
                        x.Student.Id == item.Id &&
                        x.Lesson.Id == lesson.Id);
                    var attendance = App.DB.Attendance.FirstOrDefault(x =>
                        x.StudentId == item.Id &&
                        x.Lesson.Id == lesson.Id);

                    var studentList = new StudentGrade()
                    {
                        Student = item,
                        IsAttend = attendance == null ? false : true,
                        Raiting = mark?.RaitingSystem
                    };
                    studentGradesBuffer.Add(studentList);
                }
                return studentGradesBuffer.ToList();
            }
            else
            {
                foreach (var item in lesson.ReportCard.Group.Student)
                {
                    studentGradesBuffer.Add(new StudentGrade()
                    {
                        Student = item,
                        IsAttend = false
                    });
                }
                return studentGradesBuffer.ToList();
            }
        }

        private void GetDateLesson()
        {
            DateTime selectLessonDate = Convert.ToDateTime((dynamic)CBDateLesson.SelectedValue);
            if (selectLessonDate != null && selectLessonDate != default)
            {
                if (selectLessonDate.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Данный урок не был еще проведен");
                    CBDateLesson.SelectedIndex = -1;
                    return;
                }
                else
                {
                    Lesson lessonBuffer = _lessonsCurrentSemester.SingleOrDefault(x =>
                        x.DateOfTheLesson.Date == selectLessonDate.Date &&
                        x.IsConducted == true &&
                        x.ReportCard.Id == _selectedReportCard.Id);

                    if (lessonBuffer != null)
                    {
                        _studentGrades = GetStudentGrade(lessonBuffer);
                        _currentLesson = lessonBuffer;
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
                        _studentGrades = GetStudentGrade(lesson);
                        LVStudent.ItemsSource = _studentGrades.ToList();
                        BAccept.Content = "Провести";
                    }
                }
            }
        }

        private void CBDateLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LVStudent.Visibility = Visibility.Visible;
            GListStudent.Visibility = Visibility.Visible;
            _selectedStudent = new StudentGrade();
            GStudentInfo.DataContext = _selectedStudent;
            ChBStudent.IsChecked = false;
            _currentLesson = null;
            _studentGrades.Clear();

            GetDateLesson();
        }

        private void ChBHideStudent_Checked(object sender, RoutedEventArgs e)
        {
            if (ChBHideStudent.IsChecked == true)
            {
                LVStudent.ItemsSource = _studentGrades.Where(x => x.IsAttend == false).ToList();
            }
            else 
            {
                LVStudent.ItemsSource = _studentGrades.ToList();
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

        private void BAccept_Click(object sender, RoutedEventArgs e)
        {
            if (_currentLesson.Id != 0)
            {
                var editList = _studentGrades.ToList();
                var oldList = GetStudentGrade(_currentLesson).ToList();
                foreach (var item in oldList)
                {
                    var student = editList.FirstOrDefault(x => x.Student.Id == item.Student.Id);

                    if (student.IsAttend != item.IsAttend)
                    {
                        if (student.IsAttend == false)
                        {
                            var deleteAttend = App.DB.Attendance.SingleOrDefault(x =>
                                x.StudentId == student.Student.Id &&
                                x.LessonId == _currentLesson.Id);
                            App.DB.Attendance.Remove(deleteAttend);
                        }
                    }

                    if (student.Raiting != item.Raiting)
                    {
                        if (student.Raiting == null)
                        {
                            var deleteAttend = App.DB.Assessment.SingleOrDefault(x =>
                                x.StudentId == student.Student.Id &&
                                x.LessonId == _currentLesson.Id);
                            App.DB.Assessment.Remove(deleteAttend);
                        }
                    }
                }
            }
            foreach (var item in _studentGrades)
            {
                if (item.Raiting != null)
                {
                    if (App.DB.Assessment.Any(x => 
                        x.StudentId != item.Student.Id &&
                        x.LessonId == _currentLesson.Id))
                    {
                        _currentLesson.Assessment.Add(new Assessment
                        {
                            Student = item.Student,
                            RaitingSystem = item.Raiting,
                            Employee = App.LoggedTeacher
                        });
                    }
                    else
                    {
                        var markStudent = App.DB.Assessment.FirstOrDefault(x =>
                            x.StudentId == item.Student.Id &&
                            x.LessonId == _currentLesson.Id);
                        markStudent.RaitingSystemId = item.Raiting.Id;
                    }
                }
                if (item.IsAttend != false)
                {
                    _currentLesson.Attendance.Add(new Attendance
                    {
                        Student = item.Student
                    });
                }
            }

            if (_currentLesson.Id == 0)
            {
                App.DB.Lesson.Add(_currentLesson);
            }

            App.DB.SaveChanges();
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
                _selectedStudent.IsAttend = true;
            }
            else 
            { 
                CBMark.IsEnabled = true;
                _selectedStudent.IsAttend = false;
            }
        }

        private void LVStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedStudent = LVStudent.SelectedItem as StudentGrade;
        }

        private void CBMark_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBMark.SelectedIndex == -1)
            {
                ChBStudent.IsEnabled = true;
            }
            else
            {
                ChBStudent.IsEnabled = false;
            }
        }

        private void CBMark_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[2-5]") == false)
            {
                e.Handled = true;
                return;
            }

            if (CBMark.SelectedItem != null)
            {
                e.Handled = true;
                return;
            }
        }

        private void CBMark_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }


}
