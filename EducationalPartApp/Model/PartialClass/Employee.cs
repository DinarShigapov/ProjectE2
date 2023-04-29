using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPartApp.Model
{
    public partial class Employee : ICloneable
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

        public string PostTeacher
        {
            get 
            {
                string sPost = "";


                foreach (var item in App.LoggedEP.EmployeePost)
                {
                    sPost += $"{App.DB.Post.FirstOrDefault(x => x.Id == item.Id)?.Name}, ";

                }
                sPost = sPost.TrimEnd(' ', ',');

                return sPost ;
            }
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
