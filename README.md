# EventShopApp

EventShopApp is an ASP.NET Core application designed for managing a small shop by receiving and processing flower and arrangement orders for varieties of events. It allows users to browse flowers and arrangements, create and manage orders, and allows the management of products, users and orders.

## Features
- User registration & login
- Order management
- Flower & arrangement catalog
- Cart and order functionality
- Management panel for managing users and products

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- Bootstrap
- Font Awesome
- SQL Server

## Prerequisites
- .NET SDK (Recommended version: 5.0 or later)
- Git
- SQL Server or SQLite (depending on your setup)

## Installation

### Step 1: Clone the Repository
Clone the repository to your local machine:
```https://github.com/SVakov/EventShop.git```

### Step 2: Navigate to the Project Folder
Change into the project directory where the solution file is located:
```cd EventShop```

### Step 3: Install Dependencies
Restore the necessary NuGet packages for the project:
```dotnet restore```

### Step 4: Configure User Secrets
Initialize User Secrets for the project:
```dotnet user-secrets init```
Set the connection string for your local database using your credentials:
<!--dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=EventShopDB;Trusted_Connection=True;"-->

### Step 5: Apply Database Migrations
Apply pending migrations to set up the database schema:
```dotnet ef database update```

### Step 6: Run the Application
Start the application locally on your machine:
```dotnet run```
<!--The application will be accessible at `http://localhost:5000` or another port depending on your configuration.-->

## Additional Notes
- If you are using a different database or want to configure a new one, make sure to adjust the connection string accordingly.
- If you plan to deploy the app, ensure that the appropriate environment variables are set in your hosting platform.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
For any questions, feel free to contact me at: vakovslavcho@gmail.com
