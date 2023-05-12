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
            var cloneBuffer = (Subgroup)MemberwiseClone();
            cloneBuffer.Employee = Employee.Clone();
            return cloneBuffer;
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
