# CodeSign — Corporate Email Signature Manager

> A full-stack web application for managing corporate email signatures.  

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-7B2FBE?style=flat&logo=blazor)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-512BD4?style=flat&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?style=flat&logo=microsoftsqlserver)
![JWT](https://img.shields.io/badge/Auth-JWT_Bearer-000000?style=flat&logo=jsonwebtokens)

---

## 📋 Overview

CodeSign allows companies to centrally manage email signature templates and assign them to employees. Administrators create and manage HTML signature templates, assign them to users, and users can preview and export their assigned signature as a ready-to-use HTML file.

This mirrors the core functionality of products like **CodeTwo Email Signatures** — managing corporate email identity at scale.

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🔐 JWT Authentication | Register, login, 24h token expiry, role-based claims |
| 👥 Role-Based Access | Admin manages everything; User sees own profile and signature |
| ✍️ Signature Templates | Full CRUD with live HTML preview and accent color picker |
| ✏️ Dual-mode Editor | Create signatures via form fields with live preview OR raw HTML editor |
| 📋 User Management | Admin assigns/removes templates per user |
| 📥 HTML Export | Download email signature as ready-to-use `.html` file |
| 💾 Session Persistence | Token in `localStorage`, survives page refresh |
| 🎨 Dark UI | Custom CSS dark theme, no external CSS frameworks |

---

## 🏗️ Architecture

```
CodeSign/
├── CodeSign.API/              # ASP.NET Core Web API
│   ├── Controllers/
│   │   ├── AuthController.cs          # Register, Login → JWT token
│   │   ├── SignaturesController.cs    # CRUD for signature templates
│   │   └── UsersController.cs        # User profiles, assign signatures
│   ├── Services/
│   │   ├── TokenService.cs            # JWT generation
│   │   ├── SignatureService.cs        # Signature business logic
│   │   └── UserService.cs            # User business logic
│   ├── Data/
│   │   ├── AppDbContext.cs            # EF Core DbContext + Identity
│   │   └── Migrations/               # Database migrations
│   ├── Models/
│   │   ├── AppUser.cs                 # Extends IdentityUser
│   │   ├── SignatureTemplate.cs       # Signature template model
│   │   └── UserSignature.cs          # User ↔ Template relationship
│   └── DTOs/                         # Request/Response objects
│
├── CodeSign.Client/           # Blazor WebAssembly
│   ├── Pages/
│   │   ├── Login.razor                # Login form
│   │   ├── Register.razor             # Registration form
│   │   ├── Dashboard.razor            # User profile + signature preview + export
│   │   ├── Signatures.razor           # Template management (Admin)
│   │   └── Users.razor               # User management (Admin)
│   ├── Services/
│   │   ├── AuthService.cs             # JWT storage, login/logout
│   │   └── AuthenticatedComponentBase.cs  # Base class for protected pages
│   └── wwwroot/index.html            # JS helper for file download
│
└── CodeSign.Shared/           # Shared models
    └── Models.cs              # LoginRequest, AuthResponse, UserProfile, etc.
```

---

## 🔑 API Endpoints

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| POST | `/api/Auth/register` | Public | Register new user |
| POST | `/api/Auth/login` | Public | Login, returns JWT |
| GET | `/api/Signatures` | User/Admin | Get all templates |
| POST | `/api/Signatures` | Admin | Create template |
| PUT | `/api/Signatures/{id}` | Admin | Update template |
| DELETE | `/api/Signatures/{id}` | Admin | Delete template |
| GET | `/api/Users/me` | User/Admin | Own profile + signature |
| GET | `/api/Users` | Admin | All users |
| POST | `/api/Users/{id}/assign-signature` | Admin | Assign template |
| DELETE | `/api/Users/{id}/signature` | Admin | Remove signature |

---

## 🔐 Role-Based Access

| Feature | Admin | User |
|---------|:-----:|:----:|
| View signature templates | ✅ | ✅ |
| Create / Edit / Delete templates | ✅ | ❌ |
| View own profile | ✅ | ✅ |
| Export signature as HTML | ✅ | ✅ |
| View all users | ✅ | ❌ |
| Assign/remove signatures | ✅ | ❌ |

---

## ⚡ How to Run

### Prerequisites
- Visual Studio
- .NET 10 SDK
- SQL Server LocalDB (included with VS)

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/romayavor/CodeSign.git

# 2. Open CodeSign in Visual Studio 2022

# 3. Set multiple startup projects:
#    Right-click Solution → Set Startup Projects
#    Set CodeSign.API → Start
#    Set CodeSign.Client → Start
```

```
# 4. Apply database migrations (Package Manager Console, project: CodeSign.API)
Update-Database
```

```bash
# 5. Press F5
#    API runs on:    https://localhost:7278
#    Client runs on: https://localhost:7100
```

### First Use
1. Go to `https://localhost:7100/register`
2. Create your account — **first registered user becomes Admin automatically**
3. Login at `/login`
4. Create signature templates at `/signatures`
5. Assign signatures to users at `/users`

> **Password requirements:** minimum 6 characters, at least 1 digit

---

## 🛠️ Tech Stack

**Backend**
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core 10 + SQL Server LocalDB
- ASP.NET Identity (user management, password hashing)
- JWT Bearer Authentication with role claims

**Frontend**
- Blazor WebAssembly (.NET 10)
- Custom dark CSS theme (no Bootstrap, no Tailwind)
- `IJSRuntime` + `localStorage` for session persistence
- `AuthenticatedComponentBase` pattern for protected routes

**Tooling**
- Visual Studio 2022
- Git + GitHub
- `.http` files for API testing (no Swagger due to .NET 10 compatibility)

---

## 💡 Key Technical Decisions

**Why service layer?**
Controllers never access the database directly. Each controller delegates to a dedicated service (`SignatureService`, `UserService`, `TokenService`). This separation makes the code testable and maintainable.

**Why AuthenticatedComponentBase?**
Instead of repeating authorization checks in every Blazor page, a base component class handles `InitializeAsync()` and redirect to `/login`. Each protected page simply inherits it and overrides `OnAuthenticatedAsync()`.

**Why localStorage over in-memory state?**
Blazor WASM loses in-memory state on page refresh. Storing the JWT token in `localStorage` via `IJSRuntime` ensures the session survives navigation and refresh without requiring re-login.

**Why dual-mode signature editor?**
Users can create signatures either through a structured form (name, title, email, phone, company, website) with automatic HTML generation and live preview, or switch to raw HTML mode for full control. This mirrors real-world email signature tools like CodeTwo which offer both simple and advanced editing modes.

---

## 👨‍💻 Author

**Roman Yavorenko**  
Student of Computer Science at Karkonoska Akademia Nauk Stosowanych, Jelenia Góra  
Building skills in ASP.NET Core, Blazor, and enterprise .NET development.

[![GitHub](https://img.shields.io/badge/GitHub-romayavor-181717?style=flat&logo=github)](https://github.com/romayavor)
