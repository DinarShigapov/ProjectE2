using EducationalPartApp.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPartApp.Model
{
    public partial class Subgroup : ICloneable
    {

        public string StrSubgroup
        {
            get 
            {
                return $"{Employee.FullNameShort} {Auditorium.Name} каб.";
            }
        }

        public Subgroup Clone()
        {
            var cloneBuffer = new Subgroup();
            cloneBuffer.Employee = App.DB.Employee.FirstOrDefault(x => x.Id == Employee.Id);
            cloneBuffer.Auditorium = App.DB.Auditorium.FirstOrDefault(x => x.Id == Auditorium.Id);
            return cloneBuffer;
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
