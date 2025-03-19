# Product Manager Web Application

A full-stack web application built with:

- 🌐 **Frontend**: React.js (TypeScript) with Redux-Saga
- 🖥 **Backend**: .NET Core Web API with Dapper ORM
- 📦 **Database**: Microsoft SQL Server

---

## 📌 Prerequisites

Ensure you have the following installed:

- [Node.js](https://nodejs.org/) (v16 or later)
- [Yarn](https://yarnpkg.com/) or npm
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [.NET SDK 7.0+](https://dotnet.microsoft.com/en-us/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## 🛠 Backend Setup (ASP.NET Core API)

### 1️⃣ **Configure the Database**
1. Open **`appsettings.json`**.
2. Update the **Connection String** for SQL Server:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your-server;Database=your-db;User Id=your-user;Password=your-password;"
   }
---

