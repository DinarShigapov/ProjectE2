using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPartApp.Model
{
    public partial class Group
    {
        public string StrFullName
        {
            get 
            {
                return $"{Name} {Qualification.Abbreviation}";
            }
        }
    }
}
