# Hr-System (.Net Core Web Api | Angular)

## Introduction
This is an HR (Human Resources) management system, designed using Clean Architecture principles, to streamline various HR tasks, including employee management, attendance tracking, department management, and user access control. Additionally, it offers extensive General Settings customization For Each Employee

## Features
- **Employee Management**: Create, edit, and delete employee profiles with detailed information.
- **Attendance Tracking**: Record and manage employee attendance records.
- **Department Management**: Manage and organize company departments.
- **User Roles and Permissions**: Assign roles and permissions to control access to different parts of the application.
- **Add General and Custom Settings**: Customize settings for each employee, including weekends, public holidays, discount hours, and bonus hour values.

- ## Technologies

- **ASP.NET Core 7.0**: The application is built on the ASP.NET Core framework.
- **Entity Framework Core**: We use EF Core for data modelling and database operations.
- **JWT Authentication**: JSON Web Tokens are used for user authentication.
- **Angular**: The front-end is developed using Angular.
- **Reactive Forms**: Angular's reactive forms are used for data entry and validation.
- **Routing**: Angular routing is implemented for navigating within the application.
- **Services**: Angular services are used to manage data and perform HTTP requests.
- **Auth Guard**: Route guards are used to protect routes based on user authentication.
- **Swagger**: API documentation is generated using Swagger.
- **SQL Server**: The application uses SQL Server for database storage.

## Getting Started

Follow these instructions to set up and run the HR Management System locally:

1. **Prerequisites**:
   - [Visual Studio](https://visualstudio.microsoft.com/downloads/)
   - [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core)
   - [Node.js](https://nodejs.org/)
   - [Angular CLI](https://cli.angular.io/)

2. **Clone the Repository**:
   ```sh
   git clone https://github.com/yourusername/hr-management-system.git
3. **Database Setup**:
   - Create a SQL Server database and update the connection string in appsettings.json.
   - Run Entity Framework migrations to set up the database. 
   ```sh
   dotnet ef database update
