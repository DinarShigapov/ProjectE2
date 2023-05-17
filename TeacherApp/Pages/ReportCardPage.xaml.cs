using System;
using System.Collections.Generic;
using System.Linq;
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

        List<ReportCardGroup> reportCardsList = new List<ReportCardGroup>();


        public ReportCardPage()
        {
            InitializeComponent();

            
            
            if (reportCardsList.Count > 0)
            {
                RefreshButton();
            }
        }

        private void RefreshButton() 
        {
            SPGroup.Children.Clear();
            for (int i = 1; i <= reportCardsList.Count; i++)
            {
                RadioButton radioBtn = new RadioButton();
                radioBtn.Content = reportCardsList[i - 1].GroupName.Name;
                if (i == 1) radioBtn.IsChecked = true;
                SPGroup.Children.Add(radioBtn);
            }
        }

    }

    public class ReportCardGroup
    {
        public ReportCardGroup(ReportCard reportCard)
        {

        }

        public List<ReportCard> ReportCardList { get; set; }
        public Group GroupName { get; set; }

        
    }
}
