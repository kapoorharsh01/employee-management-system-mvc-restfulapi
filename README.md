# Employee Management System

A full-stack application built with .NET Web API backend and ASP.NET MVC frontend for managing employee records.

## Features

### Backend (API)
- RESTful API using .NET Web API
- Complete CRUD operations
- Paginated and sortable employee listing

### Frontend (MVC)
- ASP.NET MVC application
- Employee grid with sorting and paging
- Add/Edit/Delete functionality

## Employee Entity

- **EmployeeId**: int (auto-increment)
- **Name**: string (required)
- **Department**: string
- **Email**: string (unique)
- **DateOfJoining**: DateTime

## API Endpoints

| Method | Endpoint | Description | Query Parameters |
|--------|----------|-------------|------------------|
| GET | `/api/employees` | Returns paginated & sortable list | `page`, `pageSize`, `sortColumn`, `sortDirection` |
| GET | `/api/employees/{id}` | Returns employee by id | - |
| POST | `/api/employees` | Adds a new employee | - |
| PUT | `/api/employees/{id}` | Updates an existing employee | - |
| DELETE | `/api/employees/{id}` | Deletes an employee | - |

## MVC Features

### Employee List Page
- Display employee data in a grid/table
- Paging (10 records per page)
- Sorting (click on column header to sort)

### Add Employee
- Form to add a new employee (calls POST API)

### Edit Employee
- Edit existing employee (calls PUT API)

### Delete Employee
- Delete employee (calls DELETE API with confirmation)

## Author
Harsh K.
