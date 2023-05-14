using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPartApp.Model
{
    [Serializable]
    public partial class Schedule: ICloneable
    {
        public Schedule Clone()
        {
            return (Schedule)MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        public string StrAllTeacher
        {
            get
            {
                string buffer = "";
                if (Subgroup != null)
                {
                    foreach (var item in Subgroup)
                    {
                        buffer += $"{item.Employee.FullNameShort} / ";

                    }
                    return buffer.TrimEnd(' ', '/');
                }
                return null;
            }
            set { }
        }

    }
}
