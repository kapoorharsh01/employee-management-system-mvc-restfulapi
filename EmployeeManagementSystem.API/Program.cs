using EmployeeManagementSystem.API.Models;
using EmployeeManagementSystem.API.Repositories;
using EmployeeManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // FORCE CONSOLE LOGGING 
            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();
            //builder.Logging.AddDebug();

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepository<Employee>, EmployeesRepository>();
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowMVC", policy =>
            //    {
            //        policy.WithOrigins("https://localhost:7060;http://localhost:5085") // Your MVC URLs
            //              .AllowAnyHeader()
            //              .AllowAnyMethod();
            //    });
            //});
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            //app.UseRouting();

            //app.UseCors("AllowMVC");
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
