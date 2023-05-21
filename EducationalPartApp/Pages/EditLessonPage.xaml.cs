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
        private Schedule _schedule;
        private Schedule _scheduleSave;
        private List<Schedule> _schedulesBuffer;
        private List<Subgroup> _subgroupList = new List<Subgroup>();
        public EditLessonPage(Schedule listClass, List<Schedule> schedulesList)
        {
            InitializeComponent();
            _schedulesBuffer = schedulesList;
            _schedule = listClass;
            _scheduleSave = listClass.Clone();
            DataContext = _scheduleSave;
            CBDisciplines.ItemsSource = App.DB.Discipline.ToList();
            CBAuditoriums.ItemsSource = App.DB.Auditorium.ToList();
            CBDisciplines.SelectedItem = _scheduleSave.Discipline;
            if (_scheduleSave.Subgroup != null)
            {
                 _subgroupList.AddRange(_scheduleSave.Subgroup);
                LVTeacherList.ItemsSource = _subgroupList.ToList();
            }   
        }

        private void CBDisciplines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _subgroupList.Clear();
            LVTeacherList.ItemsSource = _subgroupList.ToList();

            if (CBDisciplines.SelectedItem != null)
            {
                var disciplineBuffer = CBDisciplines.SelectedItem as Discipline;
                var listEmployee = App.DB.DisciplineTeacher.Where(x => x.DisciplineId == disciplineBuffer.Id).ToList();
                List<Employee> employees = new List<Employee>();
                foreach (var item in listEmployee)
                {
                    employees.Add(item.Employee);
                }
                CBTeachers.ItemsSource = employees;
                SPTeacherAud.IsEnabled = true;
            }
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {

            if (_subgroupList.Count == 0)
            {
                MessageBox.Show("Выберите преподавателя","Внимание",MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            foreach (var item in _schedulesBuffer)
            {
                if (item.ClassTimeId == _scheduleSave.ClassTimeId
                    && item.DayOfTheWeekId == _scheduleSave.DayOfTheWeekId) break;

                if (item.Discipline == _scheduleSave.Discipline)
                {
                    string dayBuf = App.DB.DayOfTheWeek.FirstOrDefault(x => x.Id == item.DayOfTheWeekId).Name.ToString();
                    string classTimeBuf = App.DB.ClassTime.FirstOrDefault(x => x.Id == item.ClassTimeId).ClassNumber.ToString();
                    if (item.Subgroup.Count > _subgroupList.Count)
                    {

                        var result = MessageBox.Show(
                            $"'{dayBuf} {classTimeBuf} п.' было установлено больше преподавателей\nДобавить преподавателя?",
                            "Внимание", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            foreach (var teacher in item.Subgroup)
                            {
                                if (!_subgroupList.Any(x => x.Employee == teacher.Employee))
                                {
                                    _subgroupList.Add(teacher.Clone());
                                }
                            }
                        }
                        else if (result == MessageBoxResult.No) return;

                    }
                    else if (item.Subgroup.Count < _subgroupList.Count)
                    {
                        MessageBox.Show($"Вы указали больше преподавателей, чем в '{dayBuf} {classTimeBuf} п.'");
                        return;
                    }
                }
            }

            _scheduleSave.Subgroup = _subgroupList.ToList();
            foreach (PropertyInfo property in typeof(Schedule).GetProperties().Where(p => p.CanWrite))
            {
                if (property.Name == "Schedule") break;
                property.SetValue(_schedule, property.GetValue(_scheduleSave, null), null);
            }

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

            var subgroupsNow = App.DB.Subgroup.Where(
                x => x.Schedule.DayOfTheWeekId == _scheduleSave.DayOfTheWeekId
                && x.Schedule.ClassTimeId == _scheduleSave.ClassTimeId
                && x.Schedule.SemesterId == _scheduleSave.Group.Semester.Id
                && x.Schedule.Date.Year == DateTime.Now.Year).ToList();


            foreach (var item in subgroupsNow.ToList())
            {
                if (item.Employee == selectTeacher)
                {
                    MessageBox.Show($"В это время {item.Employee.FullNameShort} проводит урок у {item.Schedule.Group.StrFullName}");
                    return;
                }

                if (item.Auditorium == selectAuditorium)
                {
                    MessageBox.Show($"В это время каб. {item.Auditorium.Name} занят");
                    return;
                }
            }

            if (_subgroupList.FirstOrDefault(x => x.Employee == selectTeacher) != null)
            {
                MessageBox.Show("Данный преподаватель есть в списке", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _subgroupList.Add(new Subgroup { Employee = selectTeacher, Auditorium = selectAuditorium });
            LVTeacherList.ItemsSource = _subgroupList.ToList();
        }
    }
}
