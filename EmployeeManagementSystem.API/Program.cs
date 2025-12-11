using AutoMapper;
using EmployeeManagementSystem.API.Data;
using EmployeeManagementSystem.API.DTOs;
using EmployeeManagementSystem.API.Mappings;
using EmployeeManagementSystem.API.Models;
using EmployeeManagementSystem.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;


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

            //var mapperConfig = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new EmployeeProfile());
            //});
            //IMapper mapper = mapperConfig.CreateMapper();
            //builder.Services.AddSingleton(mapper);

            //var config = new MapperConfiguration(cfg =>
            //cfg.CreateMap<Employee, EmployeeDto>(),
            //ILoggerFactory loggerFactory);



            //var config = new MapperConfiguration(
            //    cfg => cfg.AddProfile(new EmployeeProfile()),
            //    NullLoggerFactory.Instance
            //);


            // Adding Employee Profile to the DI container
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<EmployeeProfile>();
                
            });

            //builder.Services.AddAutoMapper(cfg =>
            //{
            //    cfg.CreateMap<Employee, EmployeeDto>().ReverseMap();
            //});


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepository<Employee>, EmployeesRepository>();

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowNg", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseRouting();

            app.UseCors("AllowNg");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
