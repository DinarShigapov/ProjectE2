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

namespace ProjectE2.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
            RefreshPage(new SchedulePage());
        }

        private void BHome_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new SchedulePage());
        }

        private void BProfile_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new ProfilePage());
        }

        private void BMain_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new MainPage());
        }
        private void BAssessment_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage(new AssessmentPage());
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

    }
}
