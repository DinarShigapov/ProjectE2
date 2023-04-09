using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE2.Model
{
    public partial class Group
    {
        public string GroupFN
        {
            get 
            {
                var qualification = App.DB.Qualification.FirstOrDefault(x => x.Id == QualificationId).Abbreviation.ToString();
                if (qualification == null) qualification = "NaN";
                
                return $"{Name} {qualification}";
            }
        }
    }
}
