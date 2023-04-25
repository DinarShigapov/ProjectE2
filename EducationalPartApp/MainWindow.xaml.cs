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
using EducationalPartApp.Pages;
using MaterialDesignThemes.Wpf;

namespace EducationalPartApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthPage());
        }

        public void ChangeFrameWindow(Page page) 
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.MainFrame.Content is MenuPage menuPage)
            {
                var menuFrame = menuPage.FindName("MenuFrame") as Frame;
                menuFrame.Navigate(page);
            }
            var popUp = mainWindow?.FindName("DialogHostModal") as DialogHost;
            popUp.IsOpen = false;
        }

        public void GetFrameWindow(Page page, double width, double height) 
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window == null)
                return;

            var popUp = window?.FindName("DialogHostModal") as DialogHost;
            var gridSize = popUp.FindName("gridDialogHost") as Grid;
            foreach (var item in gridSize.Children)
            {
                if (item is Frame)
                {
                    var frame = (Frame)item;
                    frame.Navigate(page);
                    gridSize.Width = page.Width;
                    gridSize.Height = page.Height;
                    popUp.IsOpen = true;
                    break;
                }
            }

        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void BСollapse_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
