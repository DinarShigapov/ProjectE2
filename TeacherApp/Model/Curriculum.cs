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
    
    public partial class Curriculum
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public int SemesterId { get; set; }
        public int DisciplineId { get; set; }
        public string IndexCurriculum { get; set; }
        public string Link { get; set; }
    
        public virtual Discipline Discipline { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual Specialization Specialization { get; set; }
    }
}
