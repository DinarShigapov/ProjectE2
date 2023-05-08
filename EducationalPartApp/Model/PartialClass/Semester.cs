using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPartApp.Model
{
    public partial class Semester
    {
        public string StrCourseSemester
        {
            get 
            {
                return $"{Course} курс / {Semester1} семестр";
            }
        }
    }
}
