//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Assessment
    {
        public int Id { get; set; }
        public int ReportCardId { get; set; }
        public int StudentId { get; set; }
        public int Assessment1 { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual ReportCard ReportCard { get; set; }
    }
}
