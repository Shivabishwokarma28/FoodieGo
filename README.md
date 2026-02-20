🍔 FoodieGo - Full-Stack Food Delivery App
FoodieGo is a modern, responsive food delivery platform built with a decoupled architecture. It features a dedicated ASP.NET Core Web API backend and a dynamic MVC Frontend, providing a seamless experience from browsing to account management.

🏗️ Project Architecture
This project follows the Client-Server model:

Backend (API): A secure RESTful API that handles data persistence, password hashing, and JWT issuance.

Frontend (MVC): A user-facing web application that consumes the API using HttpClient and manages user state via Sessions.

🛠️ Features (The "Cool" Stuff)
Secure Authentication: Uses Microsoft.AspNetCore.Identity for password hashing and JWT (JSON Web Tokens) for secure, stateless communication.

Dynamic Menu System: A loop-driven interface that renders food items directly from the database.

Smart Search: Search for your favorite dishes (Pizza, Burgers, Sushi) using a real-time query filter.

Category Navigation: Filter food items by category tags with dedicated detail views.

Modern UI: Built with Bootstrap 5, featuring custom CSS animations, glassmorphism navbars, and responsive grids.

Session Management: Keeps users logged in across pages by storing JWTs and user data in local memory caches.

💻 Tech Stack
The Engine (Backend)
C# / .NET 8

Entity Framework Core (Code-First Approach)

SQL Server (Relational Database)

JWT Authentication (Bearer Tokens)

The Interface (Frontend)
ASP.NET Core MVC (Razor Pages)

Bootstrap 5 & Icons (Styling)

CSS3 (Custom UI/UX)

HttpClient Factory (API consumption)

🚦 How to Run the Project
1. The Database & API
Open FoodieGo_Backend.sln.

Update your ConnectionStrings in appsettings.json to your local SQL Server instance.

Run Update-Database in the Package Manager Console.

Run the Backend project (ensure it's on https://localhost:7043).

2. The Frontend
Open FoodieGo_Frontend.sln.

In Program.cs, verify the BaseAddress matches the Backend URL.

Press F5 to start browsing!

📂 Project Structure
Plaintext

FoodieGo/
├── FoodieGo_Backend/      # API project (JWT, DB Context, DTOs)
│   ├── Controllers/       # Auth, FoodItems, Categories
│   ├── Models/            # Database Entities
│   └── Data/              # Migrations & AppDbContext
└── FoodieGo_Frontend/     # MVC project (UI, Layouts, Session)
    ├── Views/             # Razor Views (Home, Auth, Details)
    ├── wwwroot/           # Custom CSS, Images
    └── Program.cs         # Service Configuration (HttpClient, Session)
🛡️ Security Highlights
Password Security: Users' passwords are never stored in plain text. We use PBKDF2 hashing.

SSL Bypass: Configured custom HttpClientHandler for development to allow local HTTPS communication without certificate errors.

Role-Based Access: Tokens include "Customer" or "Admin" claims to restrict page access.
