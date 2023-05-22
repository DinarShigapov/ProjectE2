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
using TeacherApp.Pages;

namespace TeacherApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
            SPProfile.DataContext = App.LoggedTeacher;
        }

        private void BHome_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BProfile_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new ProfilePage());
        }

        private void BMain_Click(object sender, RoutedEventArgs e)
        {

        }
        public void RefreshPage(Page page)
        {
            MenuFrame.Navigate(page);
            MainTextHeader.Text = page.Title;
        }

        private void BLogOut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }



        private void MenuFrame_ContentRendered(object sender, EventArgs e)
        {
            var contextContent = (sender as Frame).Content;
            if (contextContent == null) return;

            RefreshPage(contextContent as Page);
        }

        private void BReportCard_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new ReportCardPage());
        }

        private void BCallSchedule_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new CallSchedulePage());
        }
    }
}
