﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TeacherApp.Model;

namespace TeacherApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EducationDBEntities1 DB = new EducationDBEntities1();
        public static Employee LoggedTeacher;
    }
}
