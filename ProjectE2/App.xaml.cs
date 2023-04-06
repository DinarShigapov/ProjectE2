using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProjectE2.Model;

namespace ProjectE2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EducationKtitsEntities DB = new EducationKtitsEntities();
        public static Student LoggedStudent;
        public static string MainTitle { get; set; }
    }
}
