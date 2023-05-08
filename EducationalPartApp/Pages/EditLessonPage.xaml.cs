﻿using EducationalPartApp.Codes;
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
            _schedule = listClass;
            _scheduleSave = (ScheduleListClass)listClass.Clone();
            DataContext = _scheduleSave;
            CBDisciplines.ItemsSource = App.DB.Discipline.ToList();
            CBAuditoriums.ItemsSource = App.DB.Auditorium.ToList();
            CBDisciplines.SelectedItem = _scheduleSave.schedule.Discipline;
            if (_scheduleSave.subgroups != null)
            {
                 _subgroupList.AddRange(_scheduleSave.subgroups);
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

            _scheduleSave.subgroups = _subgroupList.ToList();
            foreach (PropertyInfo property in typeof(ScheduleListClass).GetProperties().Where(p => p.CanWrite))
            {
                if (property.Name == "ScheduleListClass") break;
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
                x => x.Schedule.DayOfTheWeekId == _scheduleSave.schedule.DayOfTheWeekId
                && x.Schedule.ClassTimeId == _scheduleSave.schedule.ClassTimeId
                && x.Schedule.SemesterId == _scheduleSave.schedule.Group.Semester.Id
                && x.Schedule.Date.Year == DateTime.Now.Year).ToList();

            foreach (var item in subgroupsNow.ToList())
            {
                if (item.Employee == selectTeacher)
                {
                    MessageBox.Show($"В это время у {item.Employee.FullNameShort} проводиться урок");
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

            if (_subgroupList.FirstOrDefault(x => x.Auditorium == selectAuditorium) != null)
            {
                MessageBox.Show("Данный кабинет занят", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            _subgroupList.Add(new Subgroup { Employee = selectTeacher, Auditorium = selectAuditorium });
            LVTeacherList.ItemsSource = _subgroupList.ToList();
        }
    }
}
