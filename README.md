# Employee Management System

A full-stack application built with **ASP.NET Core Web API** as the
backend and **ASP.NET MVC** as the frontend for managing employee
records efficiently.

## 🚀 Technologies Used

### Backend (API)

-   ASP.NET Core Web API
-   Entity Framework Core
-   AutoMapper
-   SQL Server
-   Repository Design Pattern

### Frontend (MVC)

-   ASP.NET MVC (.NET Core)
-   HttpClient for API communication
-   Razor Views


## 📌 Features

### Backend (API)

-   RESTful API design
-   Complete CRUD operations
-   AutoMapper for DTO ↔ Entity mapping
-   Paginated & sortable employee listing
-   Centralized API response structure
-   Repository Pattern for data access abstraction

### Frontend (MVC)

-   Employee listing with **paging** & **sorting**
-   Add Employee
-   Edit Employee
-   Delete Employee with confirmation
-   API integration through **HttpClient**
-   Error handling & validation


## 📄 Employee Entity

| Property      | Type     | Notes                       |
| ------------- | -------- | --------------------------- |
| EmployeeId    | int      | Auto-increment, Primary Key |
| Name          | string   | Required, max length 30     |
| Department    | string   | Max length 75               |
| Email         | string   | Unique                      |
| DateOfJoining | DateTime | Auto-set to current date    |


## 🔌 API Endpoints

### Base URL: `/api/employeesapi`

| Method | Endpoint                 | Description                 | Query Parameters                          |
| ------ | ------------------------ | --------------------------- | ----------------------------------------- |
| GET    | `/api/employeesapi`      | Get paginated + sorted list | page, pageSize, sortColumn, sortDirection |
| GET    | `/api/employeesapi/{id}` | Get employee by ID          | --                                        |
| POST   | `/api/employeesapi`      | Add new employee            | --                                        |
| PUT    | `/api/employeesapi/{id}` | Update existing employee    | --                                        |
| DELETE | `/api/employeesapi/{id}` | Delete employee             | --                                        |


## 🔄 AutoMapper Configuration

``` csharp
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<EmployeeProfile>();
});
```

``` csharp
public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>().ReverseMap();
    }
}
```


## 🖥️ MVC Functionality

### Employee List Page

-   Displays employee table
-   Supports sorting and paging
-   Fetches data from API using HttpClient

### Add Employee

-   Form with validations
-   Sends POST request to API

### Edit Employee

-   Pre-filled form
-   Sends PUT request to API

### Delete Employee

-   Confirmation popup
-   Sends DELETE request to API
