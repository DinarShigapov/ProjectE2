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

        public Schedule Clone()
        {
            var cloneBuffer = new Schedule();

            cloneBuffer.DayOfTheWeekId = App.DB.DayOfTheWeek.FirstOrDefault(x => x.Id == DayOfTheWeekId).Id;
            cloneBuffer.ClassTimeId = App.DB.ClassTime.FirstOrDefault(x => x.Id == ClassTimeId).Id;
            cloneBuffer.Group = App.DB.Group.FirstOrDefault(x => x.Id == Group.Id);
            cloneBuffer.Semester = App.DB.Semester.FirstOrDefault(x => x.Id == Semester.Id);

            if (Discipline != null)
            {
                cloneBuffer.Discipline = App.DB.Discipline.FirstOrDefault(x => x.Id == Discipline.Id);
            }
            if (StrAllTeacher != null)
            {
                cloneBuffer.StrAllTeacher = (string)StrAllTeacher.Clone();
            }

            var subgroups = new List<Subgroup>();
            foreach (var item in Subgroup)
            {
                subgroups.Add(item.Clone());
            }
            cloneBuffer.Subgroup = subgroups.ToList();

            return cloneBuffer;
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
