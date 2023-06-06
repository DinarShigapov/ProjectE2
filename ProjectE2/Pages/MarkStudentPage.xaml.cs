using ProjectE2.Model;
using ProjectE2.ModelAddition;
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

namespace ProjectE2.Pages
{
    /// <summary>
    /// Логика взаимодействия для MarkStudentPage.xaml
    /// </summary>
    public partial class MarkStudentPage : Page
    {

        List<StudentGrade> marks = new List<StudentGrade>();
        public MarkStudentPage()
        {
            InitializeComponent();
            ShowMarkStudent();
        }

        private void ShowMarkStudent()
        {
            foreach (var item in App.DB.Curriculum.Where(x =>
            x.SpecializationId == App.LoggedStudent.Group.Qualification.SpecializationId &&
            x.SemesterId == App.LoggedStudent.Group.SemesterId).Select(x => x.Discipline))
            {
                var marksList = App.DB.Assessment.Where(x =>
                    x.Student.Id == App.LoggedStudent.Id &&
                    x.Lesson.ReportCard.Discipline.Id == item.Id).ToList();
                marks.Add(new StudentGrade
                {
                    Discipline = item,
                    Assessments = marksList
                });
            }
            LVDisciplineMark.ItemsSource = marks;
        }

        private void CMInfo_Click(object sender, RoutedEventArgs e)
        {
            var contextSchedule = (sender as MenuItem).DataContext as StudentGrade;
            if (contextSchedule == null)
                return;

            ReportCard reportCard = App.DB.ReportCard.FirstOrDefault(x =>
                x.DisciplineId == contextSchedule.Discipline.Id &&
                x.SemesterId == App.LoggedStudent.Group.SemesterId &&
                x.GroupId == App.LoggedStudent.Group.Id);
            NavigationService.Navigate(new ScheduleInfoPage(reportCard));
        }
    }
}
