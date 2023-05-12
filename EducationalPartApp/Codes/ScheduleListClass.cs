using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EducationalPartApp.Model;

namespace EducationalPartApp.Codes
{
    [Serializable]
    public class ScheduleListClass: ICloneable
    {

        public Schedule schedule { get; set; }
        public List<Subgroup> subgroups { get; set; }
        public ScheduleListClass()
        {
        }

        public ScheduleListClass(Schedule schedule, List<Subgroup> subgroups)
        {
            this.schedule = schedule;
            this.subgroups = subgroups;
        }

        public string StrAllTeacher
        {
            get 
            {
                string buffer = "";
                if (subgroups != null) 
                {
                    foreach (var item in subgroups)
                    {

                        buffer += $"{item.Employee.FullNameShort} / ";

                    }
                    return buffer.TrimEnd(' ', '/');
                }
                return null;
            }
        }



        public object Clone() 
        {
            var cloneBuffer = (ScheduleListClass)MemberwiseClone();
            cloneBuffer.schedule = schedule.Clone();
            return cloneBuffer;
        }

        //object Clone()
        //{
        //    var cloneBuffer = (ScheduleListClass)this.MemberwiseClone();
        //    cloneBuffer.subgroups = (Subgroup)this.subgroups.Clone();
        //}
    }
}
