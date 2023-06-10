using EducationalPartApp.Model;
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

namespace EducationalPartApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddCurriculumPage.xaml
    /// </summary>
    public partial class AddCurriculumPage : Page
    {
        List<Discipline> disciplineList;
        public AddCurriculumPage()
        {
            InitializeComponent();
            RefreshDiscipline();
            CBSpecialization.ItemsSource = App.DB.Specialization.Where(x => x.Curriculum.Count == 0).ToList();
        }

        private void RefreshDiscipline() 
        {
            IEnumerable<Discipline> disciplineFilter = App.DB.Discipline.ToList();

            if (TBSearchDiscipline.Text.Length > 0)
            {
                disciplineFilter = disciplineFilter.Where(p => p.Name.ToLower().Contains(TBSearchDiscipline.Text.ToLower())).ToList();
            }
            LVDisciplineSearch.ItemsSource = disciplineFilter.ToList();
        }

        private void TBSearchDiscipline_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDiscipline();
        }

        private void CBSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LVDisciplineSearch.IsEnabled = true;
            disciplineList = new List<Discipline>();
        }

        private void LVDisciplineSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectDiscipline = LVDisciplineSearch.SelectedItem as Discipline;
            if (selectDiscipline == null) 
            {
                return;
            }
            if (!disciplineList.Any(x => x.Id == selectDiscipline.Id))
            {
                disciplineList.Add(selectDiscipline);
            }
            LVCurriculum.ItemsSource = disciplineList.ToList();
        }
    }
}
