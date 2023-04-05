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

namespace StudentApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
        }

        bool isAciveLeft = false;
        bool isAciveRight = false;

        private void SizeColumn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
                return;

            List<Border> LeftColumn = new List<Border>() 
            { CellMonday, CellTuesday, CellWednesday };
            List<Border> RightColumn = new List<Border>()
            { CellThursday, CellFriday, CellSaturday};
            var DayWeek = sender as Border;

            if (LeftColumn.Any(x => x == DayWeek))
            {
                SwitchRightDefault();
                if (!isAciveLeft)
                {
                    isAciveLeft = true;
                    SizeActionLeft.Width = new GridLength(900, GridUnitType.Star);
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
                    SizeActionRight.Width = new GridLength(900, GridUnitType.Star);
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

    }
}
