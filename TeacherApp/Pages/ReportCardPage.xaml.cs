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
            CBGrade.ItemsSource = App.DB.RaitingSystem.ToList();
            DGGrade.ItemsSource = App.DB.Student.ToList();
            CBDiscipline.ItemsSource = disciplinesTeachers.ToList();
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
                if (i == 1)
                {
                    radioBtn.IsChecked = true;
                    RefreshGroupSelect(reportCardBuffer[i - 1]);
                }
                    
                SPGroup.Children.Add(radioBtn);
            }
        }

        private void CBDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var disciplines = CBDiscipline.SelectedItem as Discipline;
            var index = _reportCardsList.FindIndex(x => x.DisciplineName == disciplines);
            RefreshButton(index);
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



            CBDateLesson.ItemsSource = DaySchedule(days, new DateTime(2022, 09, 1), new DateTime(2022, 09, DateTime.DaysInMonth(2022, 9)));
        }

        private void RBGroupSelect_Click(object sender, RoutedEventArgs e)
        {
            var reportCard = (sender as RadioButton).DataContext as ReportCard;
            RefreshGroupSelect(reportCard);
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
            return daysForSchedule;
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
