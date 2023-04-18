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
        public static EducationDataBaseEntities1 DB = new EducationDataBaseEntities1();
        public static Employee LoggedEP;
    }
}
