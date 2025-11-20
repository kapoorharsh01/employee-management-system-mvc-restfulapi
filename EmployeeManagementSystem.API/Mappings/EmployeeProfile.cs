using AutoMapper;
using EmployeeManagementSystem.API.DTOs;
using EmployeeManagementSystem.API.Models;

namespace EmployeeManagementSystem.API.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            //CreateMap<Employee, EmployeeDto>(); //Domain -> DTO(GET)

            //CreateMap<EmployeeDto, Employee>(); //DTO -> Domain(POST / PUT)

            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
