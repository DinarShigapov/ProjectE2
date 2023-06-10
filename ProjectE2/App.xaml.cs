using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProjectE2.Model;
using ProjectE2.Pages;

namespace ProjectE2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EducationBaseEntities DB = new EducationBaseEntities();
        public static Student LoggedStudent;

    }
}
