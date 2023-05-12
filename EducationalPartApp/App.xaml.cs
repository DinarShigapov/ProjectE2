using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EducationalPartApp.Model;

namespace EducationalPartApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EducationDBEntities1312 DB = new EducationDBEntities1312();
        public static Employee LoggedEP;
        public static List<Page> scheduleLists = new List<Page>();
    }
}
