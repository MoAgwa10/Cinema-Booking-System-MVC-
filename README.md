<div align="center">

# 🎬 **Cinema Booking System - Complete Implementation**

A full-featured **Cinema Booking System** built with **ASP.NET Core 8.0 MVC**, offering a seamless movie booking experience with modern UI, authentication, and complete CRUD functionality.

---

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue?logo=dotnet)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core%209.0.9-green)
![Bootstrap](https://img.shields.io/badge/Frontend-Bootstrap%205-orange)
![Status](https://img.shields.io/badge/Status-Production%20Ready-brightgreen)

</div>

---

## 🧩 **Project Overview**
A **comprehensive web application** designed to manage movies, cinemas, actors, producers, and ticket bookings — featuring full **authentication**, **shopping cart**, and **responsive UI**.

---

## 🚀 **Features Implemented**

### 🎞 CRUD Operations
- 🎬 **Movies Management** – Full CRUD with image preview, category selection & multi-cinema assignment  
- 👨‍🎤 **Actors Management** – Actor profiles with biography & photo upload  
- 🏛 **Cinemas Management** – Cinema info with logo & description  
- 🎥 **Producers Management** – Manage producer data & profile images  

### 🔐 Authentication & Authorization
- 📝 User registration with **email confirmation**  
- 🔑 Login/Logout with **secure session handling**  
- 📩 Password recovery via **email reset link**  
- 👑 Role-based access (Admin/User)  
- 🛡 Anti-forgery protection & password policies  

### 🛒 Shopping Cart & Orders
- ➕ Add movies to shopping cart  
- 🧾 Manage and update cart items  
- 💳 Complete order workflow with email confirmation  
- 📜 Order history and tracking  

### 💎 Modern UI/UX
- 📱 **Responsive** Bootstrap 5 layout  
- 🧭 Navigation bar, breadcrumbs, and dropdowns  
- ⚡ Real-time validation & feedback  
- 🎨 Clean, card-based interface  

---

## 🛠 **Technical Stack**

| Layer | Technology |
|:------|:------------|
| **Backend** | ASP.NET Core 8.0 MVC |
| **Database** | Entity Framework Core 9.0.9 (SQL Server) |
| **Authentication** | ASP.NET Core Identity |
| **Email** | MailKit SMTP Integration |
| **Frontend** | Bootstrap 5, jQuery |
| **Architecture** | Repository Pattern + Service Layer |

---

## 📁 **Project Structure**

Cinema-Booking-System/
├── Controllers/
│ ├── AccountController.cs ✅ Authentication
│ ├── ActorController.cs ✅ CRUD
│ ├── CinemasController.cs ✅ CRUD
│ ├── MoviesController.cs ✅ CRUD
│ ├── OrdersController.cs ✅ Cart & Orders
│ └── ProducerController.cs ✅ CRUD
├── Models/
│ ├── Actor.cs / Cinema.cs / Movie.cs / Producer.cs / Order.cs
│ └── ApplicationUser.cs
├── Views/
│ ├── Account/ Actor/ Cinemas/ Movies/ Orders/ Producer/
│ └── Shared/
├── Data/
│ ├── Services/ ├── Repositories/ └── AppDbContext.cs
└── ViewModel/


---

## ⚙️ **Configuration**

### 🗄 Database Connection
Update your connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "mycon": "Your SQL Server Connection String"
}

✉️ Email Settings

Configure email service for authentication emails:

"EmailSettings": {
  "SmtpServer": "your-smtp-server",
  "Port": 587,
  "Username": "your-email",
  "Password": "your-password"
}

🚀 Getting Started
🔧 Prerequisites

.NET 8.0 SDK

SQL Server

Visual Studio 2022 or VS Code

🧭 Setup Steps

Clone the repository

Update connection string in appsettings.json

Run database migrations:

dotnet ef database update


Build and run:

dotnet build
dotnet run

👥 Default Users
👑 Admin
Email: admin@cinema.com
Password: Admin123!
Role: Admin

🙍 User
Email: user@cinema.com
Password: User123!
Role: User

🎯 Key Features
👨‍💻 For Administrators

Manage all system content (Movies, Actors, Producers, Cinemas)

View and manage orders

Access admin-only sections

🎟 For Users

Browse movies & cinemas

Add tickets to cart & checkout

View order history

Manage account

🎨 UI / UX Highlights

✅ Fully responsive

⚡ Smooth animations & real-time validation

🧭 Simple navigation structure

♿ Accessible (ARIA labels & keyboard navigation)

🔮 Future Enhancements

💳 Payment Gateway Integration

🎭 Seat Selection System

⭐ Movie Ratings & Reviews

🔍 Advanced Search & Filtering

📱 Mobile App Version

🔗 REST API Support

🏆 Project Status

✅ Complete & Production Ready
All modules are implemented, tested, and ready for deployment.

💬 Support

For questions or issues, please use the GitHub Issues tab or contact the maintainer.

<div align="center">
❤️ Built with passion using ASP.NET Core MVC
</div> ```
