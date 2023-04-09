using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE2.Model
{
    public partial class Employee
    {
        public string FullName
        {
            get 
            {
                return $"{LastName} {FirstName} {Patronymic}";
            }
        }
    }
}
