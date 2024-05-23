using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UVCRMS.Models;

namespace UVCRMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClassRoomAllocation> ClassRoomAllocations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAssignToTeacher> CourseAssignToTeachers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<EnrollInACourse> EnrollInACourses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SaveStudentResult> SaveStudentResults { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SevenDayWeek> SevenDayWeeks { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

    }
}