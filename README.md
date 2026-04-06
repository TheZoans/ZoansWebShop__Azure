# ☁️ ZoansWebshop — Cloud Engineering & Deployment Documentation

**Azure App Service (Windows) • Self-Contained .NET • MongoDB Atlas • Secure Cloud Auth**

---

## 🌐 Live Demo

**Website:** [https://zoanswebshop-aaadenefhcc3etgh.canadacentral-01.azurewebsites.net](https://zoanswebshop-aaadenefhcc3etgh.canadacentral-01.azurewebsites.net)

**GitHub:** [TheZoans/ZoansWebShop__Azure](https://github.com/TheZoans/ZoansWebShop__Azure)

---

## 🔑 Login Credentials

| Role | Username | Password |
|---|---|---|
| Admin | admin | admin123 |
| User | Dometic | 1234 |
| User | Pamono | 1234 |
| User | Prime Disk International | 1234 |
| User | Decathlon | 1234 |
| User | ZM Watches | 1234 |

---

## Overview (Cloud Perspective)

Zoans Webshop is a cloud-native, fully stateless e-commerce application deployed on **Microsoft Azure App Service (Windows)** and supported by **MongoDB Atlas** as an external managed database.

The system was engineered with cloud constraints, scalability patterns, runtime isolation, and Azure hosting behavior in mind — not just built to run locally. This documentation focuses on **WHY** the solution was built this way from a cloud engineering standpoint.

---

## 🛠️ Technology Stack

| Layer | Technology |
|---|---|
| Backend Framework | ASP.NET Core 8 (MVC) |
| Database | MongoDB Atlas (Cloud NoSQL) |
| Authentication | ASP.NET Core Cookie Authentication |
| Password Hashing | BCrypt.Net-Next |
| Frontend | Razor Views (cshtml), Custom CSS, Vanilla JavaScript |
| Cloud Platform | Microsoft Azure |
| Hosting | Azure App Service (Windows) — Canada Central |

---

## ☁️ Cloud Architecture

### Microsoft Azure — App Service

The application is hosted on **Azure App Service (Windows)** in the **Canada Central** region. The app runs as a self-contained deployment, bundling its own .NET 8 runtime so it is fully independent of whatever runtime version Azure has installed on the server.

Cookie authentication is configured specifically for Azure cloud compatibility:

```csharp
// added bcz it was required for azure app srvs
options.Cookie.SameSite = SameSiteMode.None;
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
options.SlidingExpiration = true;
```

These settings are required because Azure App Service runs behind a reverse proxy, which can cause authentication redirects to fail without them.

### MongoDB Atlas — Cloud Database

The database is hosted on **MongoDB Atlas**, a fully managed cloud NoSQL database service. The application connects to Atlas using a secure connection string stored as an Azure environment variable — never hardcoded in the source code.

The app reads the connection string at runtime via:

```csharp
builder.Configuration.AddEnvironmentVariables();
```

This allows the same codebase to run both locally (using `appsettings.json`) and on Azure (using environment variables injected by the cloud platform).

---

## 🔐 Securing the Database with Environment Variables

On Azure, all sensitive application settings are stored inside **App Service Environment Variables** instead of `appsettings.json`. Under the App Service's Configuration panel, the following keys were added:

- `MongoDB__ConnectionString` → MongoDB Atlas URI
- `MongoDB__DatabaseName` → `zoansdb`

After saving the configuration, the App Service was restarted so the new cloud-side environment values would be injected into the application at runtime.

Azure automatically maps double-underscore `__` in variable names to nested JSON keys, so `MongoDB__ConnectionString` maps to `MongoDB:ConnectionString` in the app configuration — exactly what `appsettings.json` uses locally. This means no code changes are needed between local and cloud environments.

---

## 🔍 Cloud Debugging with Kudu

After deploying the application, Azure App Service returned generic **HTTP 500** errors with no details visible in the browser or portal. To identify the actual failure, **Kudu** (Azure's built-in diagnostic environment) was used.

**Kudu URL:**
```
https://zoanswebshop-aaadenefhcc3etgh.scm.canadacentral-01.azurewebsites.net
```

Inside Kudu, **LogStream** and **eventlog.xml** were checked, where the real exception appeared:

```
System.IO.FileNotFoundException: Could not load file or assembly
'System.Runtime, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
```

This immediately revealed a **runtime dependency mismatch**. The project had pulled in a .NET 10-targeting version of `MongoDB.Driver`, while Azure App Service was running **.NET 8**. This mismatch only surfaced on the login route because that execution path triggered the MongoDB driver's internal assemblies.

**Fix applied:**
- Downgraded `MongoDB.Driver` to version `2.28.0` (compatible with .NET 8)
- Switched deployment to **Self-Contained (win-x86)** so the app ships with its own runtime instead of relying on Azure's installed environment

Without Kudu's Windows event logs, the root cause would have remained hidden behind Azure's generic 500 response.

---

## 💡 Why This Technology Stack?

**C# and ASP.NET Core** were used because these are the primary backend technologies taught at Óbuda University, and they integrate natively with Azure App Service, making cloud deployment, configuration, and troubleshooting much smoother. ASP.NET Core is cloud-optimized, supports environment-based configuration, and works perfectly with Windows/IIS hosting and self-contained deployment.

For the frontend, **JavaScript and CSS** were used to keep the UI lightweight and fully client-side. JavaScript handles small interactions without consuming server resources, and CSS ensures fast loading times — both ideal for a scalable cloud environment where the backend remains completely stateless.

**MongoDB Atlas** was chosen as a fully managed cloud database that requires zero infrastructure setup, scales automatically, and connects securely to Azure via a connection string.

---

## ✨ Features

- **Browse & Filter** — View all products or filter by category using query parameters
- **User Authentication** — Secure cookie-based login/logout with role claims
- **Role-Based Access Control** — Separate permissions for `admin` and `user` roles
- **Product Ratings** — Users can rate items (1–5 stars), average rating updates automatically
- **Product Reviews** — Users can write and edit reviews, edits are tracked with timestamps
- **Admin Panel** — Admins can add/remove items and manage users
- **Responsive Design** — Mobile-friendly layout

---

## 🛒 Regular User Flow

1. Go to the home page — all items are visible without login
2. Use the category filter buttons to filter by category
3. Click **Login** and enter credentials
4. Click any item to open its detail page
5. On the detail page, select a star rating (1–5) and click **Submit Rating**
6. Write a review and click **Submit Review**
7. Click **My Profile** in the navbar to see your average rating and all your reviews
8. To update a review, submit again — the edit is appended with a timestamp

---

## 🔧 Admin Flow

1. Click **Login** and enter `admin / admin123`
2. You are redirected to the **Admin Panel**
3. Use the **Add New Item** form to create items (category-specific fields appear automatically)
4. Use the **Remove** button on any item or user to delete them
5. Use **Add New User** to create regular or admin users

---

## 📁 Project Structure

```
Zoans/
├── Controllers/
│   ├── AccountController.cs    # Login, logout
│   ├── AdmnController.cs       # Admin panel logic
│   ├── HomeController.cs       # Homepage + category filtering
│   ├── ItemController.cs       # Item details, ratings, reviews
│   ├── UserController.cs       # User profile
│   ├── SeedController.cs       # Seed default users
│   └── ItemSeedController.cs   # Seed default items
├── Models/
│   ├── Items.cs                # Item + Review model
│   └── User.cs                 # User model
├── Services/
│   └── MongoDBService.cs       # MongoDB connection + collections
├── Views/                      # Razor view templates
├── wwwroot/
│   ├── css/site.css            # Custom stylesheet
│   └── js/site.js              # Custom JavaScript
├── appsettings.json            # Local configuration
└── Program.cs                  # App entry point + middleware pipeline
```

---

## 🗂️ Product Categories

- Vinyls
- Antique Furniture
- GPS Sport Watches
- Running Shoes
- Camping Tents
