# 🌱 Agri-Energy Connect Web Application

This web application prototype is part of the **PROG7311 Portfolio of Evidence**. It demonstrates a functional ecosystem where South African farmers and employees in the agricultural sector can collaborate, manage product data, and support sustainable farming practices.

---

## 🚀 Features

### 🔐 User Roles

- **Farmer**
  - Register/Login
  - Create and manage product listings (name, category, production date)
  - Create their own profile (location)
  - View their own products

- **Employee**
  - Register/Login
  - Add new farmers
  - View all farmers and their product counts
  - Filter all products by:
    - Category
    - Date range

---

## 🛠️ Technology Stack

| Area         | Tech                                      |
|--------------|-------------------------------------------|
| Language     | C# (.NET 8)                               |
| Framework    | ASP.NET Core MVC with Razor Views         |
| Database     | SQLite (via Entity Framework Core)        |
| Auth         | Session-based login, PBKDF2 password hashing |
| Frontend     | Bootstrap + Razor Pages                   |
| Tools Used   | Visual Studio, DB Browser for SQLite      |

---

## 📁 Project Structure

- /Core
- ├── Models # User, Farmer, Product
- ├── Services # AuthService for login/register
- ├── Data # AppDbContext, DataSeeder

- /Web
- ├── Controllers # Auth, Farmer, Employee
- ├── Views # Razor Views (Login, Register, Dashboards)
- ├── wwwroot # Static assets

---

## 🔧 Setup Instructions

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022+ or VS Code
- DB Browser for SQLite (optional for DB inspection)

### 📦 Installation Steps

1. **Clone or Extract Project:**
 
2. **Restore NuGet Packages:**

3. **Run the Project:**

4. **Visit in Browser:**

5. **Login Credentials (Seeded):**

| Role     | Username   | Password |
|----------|------------|----------|
| Farmer   | `farmer1`  | `pass123` |
| Employee | `employee1`| `pass123` |

---

## 🧪 Testing Instructions

1. Login as `farmer1` to:
- View "My Products"
- Add new product
- Create profile if not yet created

2. Login as `employee1` to:
- View and add farmers
- Filter products by category or date

---

## ✅ Prototype Requirements Mapping

| Requirement                                  | Status |
|---------------------------------------------|--------|
| Relational database with seed data          | ✅     |
| Farmer & Employee role-based access         | ✅     |
| Product and farmer CRUD operations          | ✅     |
| Product filtering by employee               | ✅     |
| Secure authentication & password hashing    | ✅     |
| User-friendly Bootstrap UI                  | ✅     |
| Session-based role enforcement              | ✅     |
| Seeded demo users and products              | ✅     |

---

## 🧾 Notes

- Passwords are hashed securely using PBKDF2.
- Session-based authentication stores `Username` and `Role`.
- Database is seeded with users and products via `DataSeeder.cs`.

---

