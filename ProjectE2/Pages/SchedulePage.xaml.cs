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
using ProjectE2.Model;

namespace ProjectE2.Pages
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
            ScheduleOutput();
        }

        bool isAciveLeft = false;
        bool isAciveRight = false;

        private void ScheduleOutput()
        {
            var student = App.LoggedStudent;

            LVMonday.ItemsSource = App.DB.Schedule.Where(x => 
            x.GroupId == student.GroupId && 
            x.DayOfTheWeekId == 1 && 
            x.SemesterId == student.Group.SemesterId).ToList();

            LVTuesday.ItemsSource = App.DB.Schedule.Where(x => 
            x.GroupId == student.GroupId && 
            x.DayOfTheWeekId == 2 && 
            x.SemesterId == student.Group.SemesterId).ToList();

            LVWednesday.ItemsSource = App.DB.Schedule.Where(x => 
            x.GroupId == student.GroupId &&
            x.DayOfTheWeekId == 3 &&
            x.SemesterId == student.Group.SemesterId).ToList();

            LVThursday.ItemsSource = App.DB.Schedule.Where(x => 
            x.GroupId == student.GroupId && 
            x.DayOfTheWeekId == 4 && 
            x.SemesterId == student.Group.SemesterId).ToList();

            LVFriday.ItemsSource = App.DB.Schedule.Where(x => 
            x.GroupId == student.GroupId &&
            x.DayOfTheWeekId == 5 &&
            x.SemesterId == student.Group.SemesterId).ToList();

            LVSaturday.ItemsSource = App.DB.Schedule.Where(x => 
            x.GroupId == student.GroupId &&
            x.DayOfTheWeekId == 6 &&
            x.SemesterId == student.Group.SemesterId).ToList();
        }


        private void SizeColumn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
                return;

            List<Rectangle> LeftColumn = new List<Rectangle>() 
            { CellMonday, CellTuesday, CellWednesday };
            List<Rectangle> RightColumn = new List<Rectangle>()
            { CellThursday, CellFriday, CellSaturday};
            var DayWeek = sender as Rectangle;

            if (LeftColumn.Any(x => x == DayWeek))
            {
                SwitchRightDefault();
                if (!isAciveLeft)
                {
                    isAciveLeft = true;
                    SizeActionLeft.Width = new GridLength(700, GridUnitType.Star);
                }
                else
                    SwitchLeftDefault();
            }
            else
            {
                SwitchLeftDefault();
                if (!isAciveRight)
                {
                    isAciveRight = true;
                    SizeActionRight.Width = new GridLength(700, GridUnitType.Star);
                }
                else
                    SwitchRightDefault();
            }
        }

        private void SwitchRightDefault() 
        {
            isAciveRight = false;
            SizeActionRight.Width = new GridLength(400, GridUnitType.Star);
        }
        private void SwitchLeftDefault() 
        {
            isAciveLeft = false;
            SizeActionLeft.Width = new GridLength(400, GridUnitType.Star);
        }


        private void CMInfo_Click(object sender, RoutedEventArgs e)
        {
            var contextSchedule = (sender as MenuItem).DataContext as Schedule;
            if (contextSchedule == null)
                return;

            NavigationService.Navigate(new ScheduleInfoPage(contextSchedule));
        }
    }
}
