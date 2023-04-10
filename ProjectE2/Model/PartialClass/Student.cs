using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE2.Model
{
    public partial class Student
    {
        public string FullNameShort
        {
            get 
            {
                return $"{LastName} {FirstName.ToCharArray()[0]}. {Patronymic.ToCharArray()[0]}.";
            }
        }

        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName} {Patronymic}";
            }
        }

    }
}
