# EraShop

EraShop API is a modern and efficient e-commerce backend service built with **.NET 8** and **C# 12.0**. It provides a robust set of features to manage products, orders, payments, and user authentication, ensuring a seamless e-commerce experience. 

With **Entity Framework Core** for data operations, **JWT-based authentication** for security, and **Stripe integration** for payment handling, EraShop API is designed for scalability, reliability, and ease of use.

## 🚀 Key Features

- **🛒 Product Management** – Full CRUD operations for products, categories, and brands.
- **📦 Order Management** – Create, update, and retrieve orders with multiple delivery methods.
- **🔐 User Authentication** – Secure user registration, login, and profile management with **ASP.NET Core Identity**.
- **💳 Payment Processing** – Seamless **Stripe** integration for payments and webhook support.
- **🧺 Basket Management** – Manage user baskets and their items efficiently.
- **✉️ Email Notifications** – Automated email confirmations and password reset functionality.
- **📊 Logging & Monitoring** – Integrated **Serilog** for logging and **Hangfire** for background job processing.
- **📖 API Documentation** – Interactive **Swagger** documentation for easy exploration and testing.
- **❤️ Wishlist Feature** – Allow users to create multiple wishlists and add products to them.
- **⭐ Product Reviews** – Enable customers to leave reviews for products they've purchased.
- **☁️ Cloud Storage** – Integration with Cloudinary for product image storage and management.

## 🛠️ Technologies Used

- **.NET 8**
- **C# 12.0**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **Stripe API**
- **Serilog**
- **Hangfire**
- **Mapster** (for object mapping)
- **Redis** (for basket storage)
- **CloudinaryDotNet** (for file storage)
- **FluentValidation** (for request validation)
- **JWT Authentication** (for secure API access)
- **MailKit** (for email services)

## 📂 Project Structure

### Core Components

- **Abstractions** - Base types including Result pattern and error handling
- **Authentication** - JWT token generation and validation
- **Contracts** - Request/response DTOs and service interfaces
- **Controllers** - API endpoints for all features
- **Entities** - Domain models and database entity definitions
- **Errors** - Domain-specific error definitions
- **Extensions** - Helper extension methods
- **Helpers** - Utility classes including email templates
- **Mapping** - Object mapping configurations
- **Services** - Business logic implementation
- **Specification** - Query specifications for repository pattern

### Key Services

- **AuthService** - User authentication and account management
- **BasketService** - Shopping basket operations with Redis
- **OrderService** - Order processing and management
- **PaymentService** - Stripe integration for payments
- **ProductService** - Product catalog management
- **CloudinaryService** - Image upload and management
- **EmailService** - Sending transactional emails
- **NotificationService** - User notifications for new products
- **ReviewService** - Product review functionality
- **WishListService** - User wishlist management

## 🚀 Getting Started

### 📌 Prerequisites

Before running the project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [Stripe Account](https://stripe.com/) (for payment processing)
- [Cloudinary Account](https://cloudinary.com/) (for image storage)

### 🔧 Configuration

1. Clone the repository
2. Update the connection strings in `appsettings.json`
3. Configure the following settings:
   - JWT Authentication settings
   - Stripe API keys
   - Cloudinary credentials
   - Email service configuration
   - Redis connection
   - Hangfire settings

### 🚀 Running the Application

```bash
# Navigate to the API project directory
cd EraShop.API

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

### 📝 API Documentation

Once the application is running, access the Swagger UI at:
```
https://localhost:5001/swagger
```

## 🧩 Key Features Implementation

### Authentication Flow

The system uses JWT authentication with refresh tokens for secure access:
- User registration with email confirmation
- Login with token and refresh token generation
- Password reset functionality
- Role-based authorization

### Shopping Experience

- **Product Browsing** - Filter and search through categories and brands
- **Basket Management** - Add, update, remove items from basket
- **Checkout Process** - Create orders with delivery method selection
- **Payment Integration** - Secure payment processing with Stripe
- **Order History** - View past orders and their status

### Admin Features

- **Product Management** - Add, update, disable products
- **Category and Brand Management** - Organize product catalog
- **Order Management** - View and process customer orders
- **User Management** - Role assignment and user administration

---

EraShop API is designed to be **fast, secure, and scalable**, making it the perfect backend solution for modern e-commerce applications. 🚀  
Feel free to contribute and improve the project! 🤝
