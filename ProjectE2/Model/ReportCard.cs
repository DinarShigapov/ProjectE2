//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectE2.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReportCard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReportCard()
        {
            this.Assessment = new HashSet<Assessment>();
            this.ReportCardTeacher = new HashSet<ReportCardTeacher>();
        }
    
        public int Id { get; set; }
        public int GroupId { get; set; }
        public System.DateTime DateOfCreation { get; set; }
        public int SemesterId { get; set; }
        public int DisciplineId { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assessment> Assessment { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Group Group { get; set; }
        public virtual Semester Semester { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportCardTeacher> ReportCardTeacher { get; set; }
    }
}
