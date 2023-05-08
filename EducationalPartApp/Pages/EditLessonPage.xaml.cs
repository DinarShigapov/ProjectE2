using EducationalPartApp.Codes;
using EducationalPartApp.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для EditLessonPage.xaml
    /// </summary>
    public partial class EditLessonPage : Page
    {
        private ScheduleListClass _schedule;
        private ScheduleListClass _scheduleSave;
        private List<Subgroup> _subgroupList = new List<Subgroup>();
        public EditLessonPage(ScheduleListClass listClass)
        {
            InitializeComponent();
            _scheduleSave = (ScheduleListClass)listClass.Clone();
            _schedule = listClass;
            DataContext = _scheduleSave;
            CBDisciplines.ItemsSource = App.DB.Discipline.ToList();
            CBDisciplines.SelectedItem = _schedule.schedule.Discipline;
            CBAuditoriums.ItemsSource = App.DB.Auditorium.ToList();
            if (_schedule.subgroups != null)
            {
                LVTeacherList.ItemsSource = _schedule.subgroups.ToList();
            }
        }

        private void CBDisciplines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _schedule.schedule.Discipline = (Discipline)CBDisciplines.SelectedItem;
            if (CBDisciplines.SelectedItem != null)
            {
                var disciplineBuffer = CBDisciplines.SelectedItem as Discipline;
                var listEmployee = App.DB.DisciplineTeacher.Where(x => x.DisciplineId == disciplineBuffer.Id).ToList();
                List<Employee> employees = new List<Employee>();
                foreach (var item in listEmployee)
                {
                    employees = App.DB.Employee.Where(x => x.Id == item.TeacherId).ToList();
                }
                CBTeachers.ItemsSource = employees;
                SPTeacherAud.IsEnabled = true;
            }
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {

            foreach (PropertyInfo property in typeof(ScheduleListClass).GetProperties().Where(p => p.CanWrite))
            {
                if (property.Name == "ScheduleListClass") break;
                property.SetValue(_scheduleSave, property.GetValue(_schedule, null), null);
            }

            _schedule.subgroups = _subgroupList.ToList();

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.MainFrame.Content is MenuPage menuPage)
            {
                var menuFrame = menuPage.FindName("MenuFrame") as Frame;
                var addSchedule = menuFrame.Content as AddSchedulePage;
                var LVLessonBuffer = addSchedule.FindName("LVLesson") as ListView;
                
                var bufferNull = LVLessonBuffer.ItemsSource;
                LVLessonBuffer.ItemsSource = null;
                LVLessonBuffer.ItemsSource = bufferNull;
                var popUp = mainWindow?.FindName("DialogHostModal") as DialogHost;
                popUp.IsOpen = false;
            }
        }

        private void BAddTeacherAud_Click(object sender, RoutedEventArgs e)
        {
            if (_subgroupList.Count == 2)
                return;

            var selectTeacher = CBTeachers.SelectedItem as Employee;
            var selectAuditorium = CBAuditoriums.SelectedItem as Auditorium;
            if (selectTeacher == null || selectAuditorium == null) return;

            _subgroupList.Add(new Subgroup { Employee = selectTeacher, Auditorium = selectAuditorium });
            LVTeacherList.ItemsSource = _subgroupList.ToList();
        }
    }
}
