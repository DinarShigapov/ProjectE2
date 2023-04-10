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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BEnter_Click(object sender, RoutedEventArgs e)
        {
            ButtonEnter();
        }

        private void BEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            ButtonEnter();
        }

        private void ButtonEnter() 
        {

            var student = App.DB.Student.FirstOrDefault(x => x.Login == TBLogin.Text);
            if (student == null)
            {
                MessageBox.Show("Логин неверный");
                return;
            }
            if (student.Password != TBPassword.Text)
            {
                MessageBox.Show("Пароль неверный");
                return;
            }

            App.LoggedStudent = student;
            NavigationService.Navigate(new MenuPage());
        }

    }
}
