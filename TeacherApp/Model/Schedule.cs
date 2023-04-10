//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeacherApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedule()
        {
            this.Subgroup = new HashSet<Subgroup>();
        }
    
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int DayOfTheWeekId { get; set; }
        public int ClassTimeId { get; set; }
        public int SemesterId { get; set; }
        public int DisciplineId { get; set; }
    
        public virtual ClassTime ClassTime { get; set; }
        public virtual DayOfTheWeek DayOfTheWeek { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Group Group { get; set; }
        public virtual Semester Semester { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subgroup> Subgroup { get; set; }
    }
}