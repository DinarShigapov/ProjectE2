﻿using System;
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


        public void GetFrameWindow(Page page, double width, double height) 
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window == null)
                return;

            MaterialDesignThemes.Wpf.DialogHost popUp = window.FindName("DialogHostModal") as MaterialDesignThemes.Wpf.DialogHost;
            var gridSize = popUp.FindName("gridDialogHost") as Grid;

            foreach (var item in gridSize.Children)
            {
                if (item is Frame)
                {
                    var item1 = (Frame)item;
                    item1.Navigate(page);
                    gridSize.Width = width;
                    gridSize.Height = height;
                }
            }
            popUp.IsOpen = true;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BСollapse_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
