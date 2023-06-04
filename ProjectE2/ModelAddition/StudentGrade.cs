using ProjectE2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectE2.ModelAddition
{
    public class StudentGrade
    {
        public Discipline Discipline { get; set; }
        public List<Assessment> Assessments { get; set; }

        public double? AverageRating
        {
            get 
            {
                if (Assessments.Count > 0)
                {
                    return Assessments.Sum(x => x.RaitingSystem.RaitingName) / Assessments.Count;
                }
                return null;
                
            }
        }
    }
}
