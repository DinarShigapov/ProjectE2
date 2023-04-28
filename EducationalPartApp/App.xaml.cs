using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EducationalPartApp.Model;

namespace EducationalPartApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PolyEntities DB = new PolyEntities();
        public static Employee LoggedEP;
    }
}
