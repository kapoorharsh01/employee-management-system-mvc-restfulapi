using EmployeeManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Employee> AllEmployees { get; set; }
        
        //Seeding Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Employee>()
            //.HasIndex(u => u.Email)
            //.IsUnique();
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    Name = "Harsh Kapoor",
                    Department = "Technical",
                    Email = "harsh@test.com",
                    DateOfJoining = new DateTime(2025, 1, 25)
                },

                new Employee
                {
                    EmployeeId = 2,
                    Name = "Harry",
                    Department = "HR",
                    Email = "harry@test.com",
                    DateOfJoining = new DateTime(2025, 1, 26)
                }

                );
        }
    }
}
