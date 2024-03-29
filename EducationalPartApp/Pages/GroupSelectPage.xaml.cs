﻿using EducationalPartApp.Model;
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
    /// Логика взаимодействия для GroupSelectPage.xaml
    /// </summary>
    public partial class GroupSelectPage : Page
    {
        public GroupSelectPage()
        {
            InitializeComponent();
            CBCourse.ItemsSource = App.DB.Semester.Where(x => x.Semester1 == 2).ToList();
            GetGroupsCurrentSemester();
            RefreshListGroup();
        }

        private void RefreshListGroup()
        {
            IEnumerable<Group> groupsFilter = GetGroupsCurrentSemester().Where(
                x => x.ReportCard.Where(z => z.SemesterId == x.SemesterId).Count() == 0 || 
                x.ReportCard.Count == 0).ToList();

            if (CBCourse.SelectedItem != null) {
                groupsFilter = groupsFilter.Where(x => x.Semester.Course == (CBCourse.SelectedItem as Semester).Course).ToList();
            }

            if (TBSearch.Text.Length > 0) {
                groupsFilter = groupsFilter.Where(
                    x => x.Name.ToLower().StartsWith($"{TBSearch.Text.ToLower()}") ||
                    x.Qualification.Abbreviation.ToLower().StartsWith($"{TBSearch.Text.ToLower()}") ||
                    x.StrFullName.ToLower().StartsWith($"{TBSearch.Text.ToLower()}")
                    ).ToList();
            }

            LVGroupSelect.ItemsSource = groupsFilter.ToList();
        }


        private List<Group> GetGroupsCurrentSemester()
        {
            int startFirstSemester = 9;
            int endFirstSemester = 12;
            int startSecondSemester = 1;
            int endSecondSemester = 6;

            var semesterValue = 0;
            var currentMonth = DateTime.Now.Month;

            if (currentMonth >= startFirstSemester &&
                currentMonth <= endFirstSemester) {
                semesterValue = 1;
            }
            else if (currentMonth >= startSecondSemester &&
                currentMonth <= endSecondSemester) {
                semesterValue = 2;
            }

            return App.DB.Group.Where(x => x.Semester.Semester1 == semesterValue).ToList();
        }

        private void BGroupSelection_Click(object sender, RoutedEventArgs e)
        {
            var selectGroup = LVGroupSelect.SelectedItem as Group;
            if (LVGroupSelect.SelectedItem == null) {
                MessageBox.Show("Выберите группу для создания расписания.");
                return;
            }
            if (!App.DB.Curriculum.Any(x => 
                x.SpecializationId == selectGroup.Qualification.SpecializationId &&
                x.SemesterId == selectGroup.SemesterId))
            {
                MessageBox.Show("Для данной специальности не создан учебный план");
                return;
            }

            new MainWindow().ChangeFrameWindow(new AddSchedulePage(selectGroup, false));
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListGroup();
        }

        private void CBCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListGroup();
        }
    }
}
