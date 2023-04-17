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

namespace TeacherApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportCardPage.xaml
    /// </summary>
    public partial class ReportCardPage : Page
    {
        List<Model.Group> groups = new List<Model.Group>();

        


        public ReportCardPage()
        {
            InitializeComponent();
            groups.AddRange(App.DB.Group.ToList());
            if (groups.Count > 0)
            {
                RefreshButton();
                
            }
        }

        private void RefreshButton() 
        {
            SPGroup.Children.Clear();
            for (int i = 1; i <= groups.Count; i++)
            {
                RadioButton radioBtn = new RadioButton();
                radioBtn.Content = groups[i - 1].Name;
                if (i == 1) radioBtn.IsChecked = true;
                SPGroup.Children.Add(radioBtn);
            }
        }

    }
}
