using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Group = TeacherApp.Model.Group;

namespace TeacherApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportCardPage.xaml
    /// </summary>
    public partial class ReportCardPage : Page
    {

        private List<ReportCardDiscipline> _reportCardsList = new List<ReportCardDiscipline>();
        private ReportCard _selectedReportCard;
        private Lesson _currentLesson;
         

        public ReportCardPage()
        {
            InitializeComponent();

            var disciplinesTeachers = App.DB.DisciplineTeacher.Where(x => x.TeacherId == App.LoggedTeacher.Id).Select(x => x.Discipline).ToList();
            foreach (var item in disciplinesTeachers)
            {
                var reportCards = App.DB.ReportCard.Where(
                    x => x.Discipline.Id == item.Id &&
                    x.ReportCardTeacher.Any(z => z.TeacherId == App.LoggedTeacher.Id) &&
                    x.IsActive == true).ToList();

                _reportCardsList.Add(new ReportCardDiscipline(item, reportCards));
            }
            CBDiscipline.ItemsSource = disciplinesTeachers.ToList();
            CBDiscipline.SelectedIndex = 0;
            RefreshButton(0);
        }

        private void RefreshButton(int index) 
        {
            SPGroup.Children.Clear();
            var reportCardBuffer = _reportCardsList[index].ReportCardList;
            if (reportCardBuffer.Count < 0) return;
            for (int i = 1; i <= reportCardBuffer.Count; i++)
            {
                RadioButton radioBtn = new RadioButton();
                radioBtn.DataContext = reportCardBuffer[i - 1];
                radioBtn.Content = reportCardBuffer[i - 1].Group.Name;
                radioBtn.Click += RBGroupSelect_Click;  
                SPGroup.Children.Add(radioBtn);
            }
        }

        private void CBDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var disciplines = CBDiscipline.SelectedItem as Discipline;
            var index = _reportCardsList.FindIndex(x => x.DisciplineName == disciplines);
            _selectedReportCard = null;
            _currentLesson = null;
            TBStudent.DataContext = null;
            BorderContext.DataContext = null;
            RefreshButton(index);
            GridVisible.Visibility = Visibility.Collapsed;
        }

        private void RefreshGroupSelect(ReportCard reportCard)
        {

            var dayOfTheWeek = App.DB.Schedule.Where(
                x => x.GroupId == reportCard.GroupId &&
                x.DisciplineId == reportCard.DisciplineId &&
                x.Subgroup.Any(z => z.TeacherId == App.LoggedTeacher.Id)).ToList();

            List<DayOfWeek> days = new List<DayOfWeek>();
            foreach (var day in dayOfTheWeek)
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

            int MonthNow = DateTime.Now.Month;
            int YearNow = DateTime.Now.Year;

            CBDateLesson.ItemsSource = DaySchedule(
                days,
                new DateTime(YearNow, MonthNow, 1),
                new DateTime(YearNow, MonthNow,
                DateTime.DaysInMonth(YearNow, MonthNow)));
        }

        void OnChecked(object sender, RoutedEventArgs e)
        {
            
        }


        private void RBGroupSelect_Click(object sender, RoutedEventArgs e)
        {
            var reportCard = (sender as RadioButton).DataContext as ReportCard;
            RefreshGroupSelect(reportCard);
            _selectedReportCard = reportCard;
            _currentLesson = null;
            GridVisible.Visibility = Visibility.Visible;
        }


        private List<DateTime> DaySchedule(List<DayOfWeek> dayOfTheWeeks, DateTime startdDate, DateTime endDate)
        {
            List<DateTime> daysForSchedule = new List<DateTime>();

            for (; startdDate < endDate;) {
                foreach (var item in dayOfTheWeeks) {
                    if (startdDate.DayOfWeek == item) {
                        daysForSchedule.Add(startdDate);
                    }
                }

                if (DateTime.DaysInMonth(startdDate.Year, startdDate.Month) < startdDate.Day + 1) {
                    startdDate = new DateTime(startdDate.Year, startdDate.Month + 1, 1);
                }
                else {
                    startdDate = new DateTime(startdDate.Year, startdDate.Month, startdDate.Day + 1);
                }
            }
            return daysForSchedule = daysForSchedule.GroupBy(x => x).Select(x => x.First()).ToList();
        }


        private void BAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void CBDateLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectLessonDate = Convert.ToDateTime((dynamic)CBDateLesson.SelectedValue);

            if (selectLessonDate != null && selectLessonDate != default(DateTime))
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

                    if (lessonBuffer.DateOfTheLesson.Date != default(DateTime))
                    {
                        _currentLesson = lessonBuffer;
                        TBLessonTopic.DataContext = _currentLesson;
                        LVStudent.ItemsSource = _currentLesson.ReportCard.Group.Student.ToList();
                        BAccept.Content = "Сохранить";
                    }
                    else
                    {
                        Lesson lesson = new Lesson();
                        lesson.ReportCard = _selectedReportCard;
                        lesson.DateOfTheLesson = selectLessonDate;
                        _currentLesson = lesson;
                        LVStudent.ItemsSource = lesson.ReportCard.Group.Student.ToList();
                        TBLessonTopic.DataContext = lesson;
                        BAccept.Content = "Провести";
                    }
                }

            }
        }


        private void LVStudent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectStudent = LVStudent.SelectedItem as Student;
            if (selectStudent == null) return;
            TBStudent.DataContext = selectStudent;
        }
    }

    public class ReportCardDiscipline
    {
        public ReportCardDiscipline(Discipline disciplineName, List<ReportCard> reportCardList)
        {
            ReportCardList = reportCardList;
            DisciplineName = disciplineName;
        }

        public List<ReportCard> ReportCardList { get; set; }
        public Discipline DisciplineName { get; set; }
    }
}
