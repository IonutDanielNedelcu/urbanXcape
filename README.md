# ProiectDAW (Urban Xcape)

Demo social media app built with ASP.NET Core 8 MVC and EF Core.

Features: posts (image/video/YouTube), comments, likes, follow system, groups, location tagging (Google Maps), and Identity-based roles.

Tech: ASP.NET Core 8, Entity Framework Core, ASP.NET Identity, Razor Views, Bootstrap, jQuery.

# ASP.NET Web Applications Development

This workspace contains course and demo projects. The primary application is `ProiectDAW`, a social media demo named "Urban Xcape" implemented with ASP.NET Core MVC and Entity Framework Core.

**Goal of this README:** give a concise, high-level view of the project, the stack, key responsibilities demonstrated, and how to run it locally.

**Project summary**
- **Name:** Urban Xcape (`ProiectDAW`)
- **Type:** ASP.NET Core 8 MVC web app with server-rendered Razor views and REST-style API endpoints.
- **Primary responsibilities demonstrated:** full-stack ASP.NET MVC development, EF Core data modelling, Identity-based authentication and role management, file uploads, Google Maps integration, and basic client-side interactivity.

**Tech stack**
- **Framework:** ASP.NET Core 8 (C#)
- **ORM:** Entity Framework Core (SQL Server provider configured; SQLite package also present)
- **Auth:** ASP.NET Core Identity with roles (Admin, Editor, User) and seeded users/roles
- **UI:** Razor Views (MVC), Bootstrap, jQuery, and client-side form validation
- **Assets/APIs:** Google Maps JavaScript API used in map-related views

**Architecture & notable files**
- **Startup:** `ProiectDAW/Program.cs` sets up services, EF, Identity, large file upload limits, and seeds roles/users via `SeedData`.
- **Db context:** `ProiectDAW/Data/ApplicationDbContext.cs` — EF entity sets and Fluent API relationship configuration (composite keys for join tables, delete behaviors).
- **Models:** `ProiectDAW/Models` contains domain entities (Post, Comment, ApplicationUser, Group, Location, Rating, etc.) and navigation properties.
- **Controllers:** `ProiectDAW/Controllers` implements MVC controllers for Posts, Groups, Comments, ApplicationUsers and an API controller for locations (`api/locations`).
- **Views:** Razor views in `ProiectDAW/Views` use partials for posts/comments and include map components (`MapNew`, `MapEdit`, `MapShow`).
- **Static assets:** `ProiectDAW/wwwroot` contains Bootstrap/jQuery and custom JS/CSS.

**Key features implemented**
- Posting with optional image or YouTube embed and location tagging (maps + server-side storage).
- Comments, likes for posts and comments.
- User follow/requests and user privacy settings.
- Groups with moderator, join requests, and group posts.
- Role-based access (Admin/User) and seeded demo accounts.

**Data modeling & complexity**
- Composite keys used for many-to-many and request entities (PostLocation, UserGroup, GroupRequest, FollowRequest, CommentLike, Rating).
- Many navigation properties and configured delete behaviors (NoAction/Restrict) to prevent cascade deletes where appropriate.

**Security & maintainability notes (for reviewers)**
- The app seeds roles and users on startup via `SeedData.Initialize` (useful for demo, remove or secure in production).
- Google Maps API key appears directly in view files — recommend moving to secrets or environment variables.
- File upload limits are configured (`MultipartBodyLengthLimit = 100MB`) and uploads are validated by extension; ensure further validation (content inspection, unique filenames) before production usage.
- Both SQL Server and SQLite EF providers are referenced; `Program.cs` uses the connection string `DefaultConnection` with SQL Server by default.

**How to run (local development)**
1. Open the project folder `ProiectDAW` in Visual Studio or VS Code.
2. Configure a database connection in `appsettings.json` or user secrets (recommended for credentials).
3. Restore and run:

```powershell
cd "ASP.NET Web Applications Development\ProiectDAW"
dotnet restore
dotnet ef database update   # applies migrations (ensure correct connection string)
dotnet run
```

**Where to look for technical details**
- Startup and service registration: `ProiectDAW/Program.cs`
- EF mapping and DbSets: `ProiectDAW/Data/ApplicationDbContext.cs`
- Domain models: `ProiectDAW/Models/` folder
- Main controllers: `ProiectDAW/Controllers/PostsController.cs`, `GroupsController.cs`, `ApplicationUsersController.cs`, `CommentsController.cs`

