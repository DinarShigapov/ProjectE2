using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherApp.Model;

namespace TeacherApp.ModelAddition
{
    public class StudentGrade
    {
        public Student Student { get; set; }
        public bool IsAttend { get; set; }
        public RaitingSystem Raiting { get; set; }
    }
}
