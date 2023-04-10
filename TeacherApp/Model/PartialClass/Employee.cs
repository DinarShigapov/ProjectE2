using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherApp.Model
{
    public partial class Employee
    {
        public string FullNameShort
        {
            get 
            {
                return $"{LastName} {FirstName.ToCharArray()[0]}. {Patronymic.ToCharArray()[0]}. ";
            }
        }

        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName} {Patronymic}";
            }
        }

        public string PostTeacher
        {
            get 
            {
                string sPost = "";


                foreach (var item in App.LoggedTeacher.EmployeePost)
                {
                    sPost += $"{App.DB.Post.FirstOrDefault(x => x.Id == item.Id).Name}, ";

                }
                sPost = sPost.TrimEnd(' ').TrimEnd(',');

                return sPost ;
            }
        }
    }
}
