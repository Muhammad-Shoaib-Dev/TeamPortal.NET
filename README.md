# TeamPortal.NET — Employee Management System

A full-featured **Employee Management System** built with **ASP.NET Core MVC**, following clean architecture principles like the **Repository Pattern** and **Service Layer**. This project focuses on real-world backend development practices, including manually implemented authentication (without Identity scaffolding), role-based authorization, and advanced data operations.

---

## 📌 About the Project

TeamPortal.NET allows organizations to manage employees, departments, and internal announcements through a clean, role-based web interface. This is my 2nd ASP.NET Core project, built to strengthen my understanding of architecture patterns and core backend concepts beyond scaffolded code.

---

## ✨ Features

- 🔐 **Manual Authentication & Authorization** (built from scratch — no Identity scaffolding)
- 👥 **Role-Based Access Control** (Admin / Employee)
- 🧩 **Repository Pattern** for data access abstraction
- ⚙️ **Service Layer** for business logic separation
- 🔍 **Advanced Filtering** (Department, Designation, Status, Salary range)
- ↕️ **Sorting** (Name, Email, Designation, Department)
- 🔎 **Search** functionality
- 📄 **Pagination** for large datasets
- 🏢 **Department Management** (CRUD)
- 📢 **Announcement Management** (CRUD, with Active/Inactive status)
- 🧑‍💼 **Employee Management** (CRUD with profile pictures)
- 🎨 Custom-styled, responsive UI (Bootstrap + custom theme)

```
TeamPortal.NET/
├── Controllers/     # MVC Controllers
├── Models/          # Entity Models & ViewModels
├── Repositories/     # Repository Pattern (Data Access Layer)
├── Services/         # Business Logic Layer
├── Views/            # Razor Views
├── wwwroot/          # Static files (CSS, JS, images)
└── Program.cs        # App configuration & DI setup
```
## 🚀 Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or later)
- SQL Server (or LocalDB)
- Visual Studio 2022 / VS Code

### Installation

1. Clone the repository
```bash
   git clone https://github.com/Muhammad-Shoaib-Dev/TeamPortal.NET.git
   cd TeamPortal.NET
```

2. Update the connection string in `appsettings.json`
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TeamPortalDB;Trusted_Connection=True;"
   }
```

3. Apply migrations
```bash
   dotnet ef database update
```

4. Run the application
```bash
   dotnet run
```

5. Open your browser at `https://localhost:5001`

---

---

## 🎯 What I Learned

- Implementing authentication and authorization manually instead of relying on Identity scaffolding
- Structuring an ASP.NET Core project using Repository and Service patterns for maintainability
- Building dynamic filtering, sorting, and pagination logic from scratch
- Designing a consistent, professional UI theme across multiple views

---

## 👤 Author

**Shoaib**
Computer Science Student | Aspiring ASP.NET Core Developer

---

## 📄 License

This project is for learning purposes and is open for feedback and suggestions.
