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
                RadioButton radioBtn = new RadioButton
                {
                    DataContext = reportCardBuffer[i - 1],
                    Content = reportCardBuffer[i - 1].Group.Name
                };
                radioBtn.Click += RBGroupSelect_Click;  
                SPGroup.Children.Add(radioBtn);
            }
        }

        private void CBDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LessonFrame.Content = null;
            var disciplines = CBDiscipline.SelectedItem as Discipline;
            var index = _reportCardsList.FindIndex(x => x.DisciplineName == disciplines);
            RefreshButton(index);
        }




        private void RBGroupSelect_Click(object sender, RoutedEventArgs e)
        {
            var reportCard = (sender as RadioButton).DataContext as ReportCard;
            LessonFrame.Navigate(new ShowLessonPage(reportCard));
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
