using ProjectE2.Model;
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

        Dictionary<Discipline, List<Assessment>> marks = new Dictionary<Discipline, List<Assessment>>();


        public MarkStudentPage()
        {
            InitializeComponent();
            foreach (var item in App.DB.Curriculum.Where(x => 
            x.SpecializationId == App.LoggedStudent.Group.Qualification.SpecializationId &&
            x.SemesterId == App.LoggedStudent.Group.SemesterId).Select(x => x.Discipline))
            {
                var marksList = App.DB.Assessment.Where(x => 
                    x.Student.Id == App.LoggedStudent.Id &&
                    x.Lesson.ReportCard.Discipline.Id == item.Id).ToList();
                marks.Add(item, marksList);
            }
            LVDisciplineMark.ItemsSource = marks;
        }
    }
}
