using EducationalPartApp.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPartApp.Model
{
    public partial class Discipline: ICloneable
    {
        public object Clone() => (Discipline)MemberwiseClone();
    }
}
