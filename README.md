# 🚗 Car Rental Web API

A RESTful Web API for managing a car rental system, built with **ASP.NET Core** and **Entity Framework Core**.
The project is a continuation of the previous Console-based Car Rental Management System, redesigned as a standalone Web API.

## ✨ Features

* Customer management
* Employee management
* Branch management
* Vehicle type management
* Vehicle management
* Rental management
* Payment management
* Authentication
* Entity relationships using Entity Framework Core
* DTOs for request/response models
* AutoMapper for object mapping
* API versioning
* Database migrations
* Validation and business rules
* Proper HTTP status codes and RESTful endpoints

## 🛠️ Technologies

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **AutoMapper**
* **REST API**
* **JWT Authentication**
* **Swagger / OpenAPI**

## 📁 Project Structure

```text
CarRental-WebAPI/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── BranchesController.cs
│   ├── CustomersController.cs
│   ├── EmployeesController.cs
│   ├── PaymentsController.cs
│   ├── RentalsController.cs
│   ├── VehiclesController.cs
│   └── VehicleTypesController.cs
│
├── DbContexts/
│   └── AppDbContext.cs
│
├── Migrations/
│
├── Models/
│   ├── DTOs/
│   ├── Profiles/
│   ├── Branch.cs
│   ├── Customer.cs
│   ├── Employee.cs
│   ├── Payment.cs
│   ├── Rental.cs
│   ├── Vehicle.cs
│   └── VehicleType.cs
│
├── Program.cs
└── appsettings.json
```

## 🗃️ Main Entities

```text
Branch
 ├── Employees
 └── Vehicles

VehicleType
 └── Vehicles

Vehicle
 └── Rentals

Customer
 └── Rentals

Rental
 ├── Customer
 ├── Vehicle
 └── Payments
```

The relationships model the real-world workflow of a car rental business.

## 🔄 Rental Workflow

When creating a rental, the API performs several business validations:

```text
Customer exists
      ↓
Vehicle exists
      ↓
Vehicle is Available
      ↓
Validate rental dates
      ↓
Calculate rental duration
      ↓
Calculate Total Price
      ↓
Create Rental
      ↓
Vehicle → Rented
```

When the vehicle is returned:

```text
Rental
  ↓
Return Vehicle
  ↓
Vehicle → Available
```

## 🔐 Authentication

The API includes an authentication system to protect secured endpoints.

Authentication is handled through **JWT tokens**, allowing authenticated users to access protected resources.

## 📡 API Endpoints

### Customers

```text
POST   /api/v1/customers
GET    /api/v1/customers
GET    /api/v1/customers/{customerId}
PUT    /api/v1/customers/{customerId}
DELETE /api/v1/customers/{customerId}
```

### Employees

```text
POST   /api/v1/employees
GET    /api/v1/employees
GET    /api/v1/employees/{employeeId}
PUT    /api/v1/employees/{employeeId}
DELETE /api/v1/employees/{employeeId}
```

### Branches

```text
POST   /api/v1/branches
GET    /api/v1/branches
GET    /api/v1/branches/{branchId}
PUT    /api/v1/branches/{branchId}
DELETE /api/v1/branches/{branchId}
```

### Vehicle Types

```text
POST   /api/v1/vehicletypes
GET    /api/v1/vehicletypes
GET    /api/v1/vehicletypes/{vehicleTypeId}
PUT    /api/v1/vehicletypes/{vehicleTypeId}
DELETE /api/v1/vehicletypes/{vehicleTypeId}
```

### Vehicles

```text
POST   /api/v1/vehicles
GET    /api/v1/vehicles
GET    /api/v1/vehicles/{vehicleId}
PUT    /api/v1/vehicles/{vehicleId}
DELETE /api/v1/vehicles/{vehicleId}
```

### Rentals

```text
POST   /api/v1/rentals
GET    /api/v1/rentals
GET    /api/v1/rentals/{rentalId}
PUT    /api/v1/rentals/{rentalId}/return
```

### Payments

```text
POST   /api/v1/payments
GET    /api/v1/payments
GET    /api/v1/payments/{paymentId}
```

## 🧩 DTOs & AutoMapper

The API uses DTOs to prevent exposing the domain entities directly through HTTP responses.

Example:

```text
Request
   ↓
Request DTO
   ↓
AutoMapper
   ↓
Entity
   ↓
Database
```

And for responses:

```text
Database
   ↓
Entity
   ↓
AutoMapper
   ↓
Response DTO
   ↓
JSON
```

This also helps prevent circular reference problems between related entities.

## 🗄️ Database

Entity Framework Core is used as the ORM for database operations.

Migrations are included in the project to manage database schema changes.

To apply migrations:

```bash
dotnet ef database update
```

To create a new migration:

```bash
dotnet ef migrations add MigrationName
```

## ▶️ Running the Project

### 1. Clone the repository

```bash
git clone <repository-url>
```

### 2. Open the project

```bash
cd CarRental-WebAPI
```

### 3. Configure the database

Update the connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CarRentalDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Apply migrations

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

### 6. Test the API

The project provides Swagger/OpenAPI documentation for exploring and testing the available endpoints.

## 📌 Business Rules

Some of the important rules implemented in the API:

* A vehicle must belong to an existing branch.
* A vehicle must belong to an existing vehicle type.
* A rental requires an existing customer.
* A rental requires an existing vehicle.
* A vehicle must be `Available` before it can be rented.
* Creating a rental changes the vehicle status to `Rented`.
* Returning a vehicle changes its status back to `Available`.
* Rental duration is calculated from the start and end dates.
* Rental price is calculated based on the vehicle type's daily rate.
* Related entities cannot be deleted when doing so would violate the defined relationships.

## 🎯 Project Goal

The goal of this project is to implement a structured and realistic **Car Rental Management Web API**, applying practical concepts such as:

* RESTful API design
* Entity Framework Core
* Relational database modeling
* DTOs
* AutoMapper
* Authentication
* API versioning
* Business logic
* Entity relationships
* HTTP status codes
* Clean project organization

## 👨‍💻 Author

**Ezaldin Alzaher**

Full-Stack Web Developer
C# | ASP.NET Core | Angular | React | SQL

---

> This project was developed as a practical application of backend development concepts using ASP.NET Core Web API and Entity Framework Core.
