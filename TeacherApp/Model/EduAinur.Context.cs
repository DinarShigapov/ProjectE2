﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EducationDataBaseEntities5 : DbContext
    {
        public EducationDataBaseEntities5()
            : base("name=EducationDataBaseEntities5")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Assessment> Assessment { get; set; }
        public virtual DbSet<Auditorium> Auditorium { get; set; }
        public virtual DbSet<ClassTime> ClassTime { get; set; }
        public virtual DbSet<Curriculum> Curriculum { get; set; }
        public virtual DbSet<DayOfTheWeek> DayOfTheWeek { get; set; }
        public virtual DbSet<Discipline> Discipline { get; set; }
        public virtual DbSet<DisciplineTeacher> DisciplineTeacher { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeePost> EmployeePost { get; set; }
        public virtual DbSet<FormOfTraining> FormOfTraining { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Qualification> Qualification { get; set; }
        public virtual DbSet<ReportCard> ReportCard { get; set; }
        public virtual DbSet<ReportCardTeacher> ReportCardTeacher { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<Specialization> Specialization { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Subgroup> Subgroup { get; set; }
    }
}
