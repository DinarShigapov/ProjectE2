using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherApp.Model
{
    public partial class ClassTime
    {
        public string StrBreak 
        {
            get 
            {
                if (App.DB.ClassTime.Count() >= Id + 1)
                {
                    var result = EndClass - App.DB.ClassTime.FirstOrDefault(x => x.Id == Id + 1).StartClass;
                    return $"{result:mm} мин";
                }
                return ""; 
                
            }
        }
    }
}
