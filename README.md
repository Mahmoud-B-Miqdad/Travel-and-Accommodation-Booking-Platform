# 🧳 Travel and Accommodation Booking Platform

![.NET 8](https://img.shields.io/badge/.NET-8.0-blueviolet)
![Build](https://img.shields.io/github/actions/workflow/status/Mahmoud-B-Miqdad/Travel-and-Accommodation-Booking-Platform/dotnet.yml?label=build&logo=github)
![Tests](https://img.shields.io/badge/unit%20tests-passing-brightgreen?logo=xunit)
![Docker](https://img.shields.io/badge/Docker-Supported-2496ED?logo=docker&logoColor=white)
![Stripe](https://img.shields.io/badge/Stripe-Payment-blue?logo=stripe)
![License](https://img.shields.io/github/license/Mahmoud-B-Miqdad/Travel-and-Accommodation-Booking-Platform)

> A scalable, feature-rich platform for booking hotels and accommodations. It supports user authentication, hotel search, photo management, email invoicing, Stripe-based payments, Docker deployment, and follows Clean Architecture with CQRS and Unit Testing.

---

## 🚀 Overview

This API powers a professional-grade hotel booking system that supports modern development best practices like:

- ✅ Clean Architecture
- ✅ SOLID Principles & Design Patterns
- ✅ Layered separation of concerns
- ✅ Dockerized and CI/CD-ready (infrastructure can be extended with GitHub Actions or other CI tools)
- ✅ Automated Unit Testing

Built for learning, demoing, and potentially real-world usage.

---

## ✨ Key Features

- 🔐 **User Authentication** (JWT-based)
- 🏨 **Advanced Hotel Search** (by city, price, capacity, room type)
- 🏨 **Manage Rooms, Hotels, Room Types**
- 📷 **Image & Gallery Management** for hotels and cities
- 📬 **Email Notifications** with PDF invoices
- 🧾 **PDF Invoice Generation**
- ☁️ **Cloudinary for Media Storage**
- 💳 **Stripe Integration for Secure Online Payments**
- 🗃️ **Admin Control over Entities**
- 📊 **Trending Destinations & Featured Deals**
- 💳 **Online Payments** using Stripe integration
- 🧪 **Unit Testing** with xUnit
- 🐳 **Dockerized Deployment**
- 📚 **Interactive API Docs** via Swagger
- 🧼 **Clean Architecture with CQRS + MediatR**

---

## 🛠️ Tech Stack

| Category       | Technology                        |
|----------------|-----------------------------------|
| Language       | C# (.NET 8)                       |
| Framework      | ASP.NET Core                      |
| Database       | SQL Server + EF Core              |
| Authentication | JWT                               |
| Storage        | Cloudinary (image hosting)        |
| PDF Generation | NReco.PdfGenerator.LT             |
| Email Service  | SendGrid                          |
| Payment        | Stripe API                        |
| API Docs       | Swagger / OpenAPI                 |
| Logging        | Serilog                           |
| Testing        | xUnit                             |
| Architecture   | Clean Architecture                |
| Patterns       | UOF, Generic Repo, CQRS, MediatR  |
| Password Hashing | Argon2                          |
| Containers     | Docker                            |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (local or cloud)
- Docker (optional for containerized setup)

### 1. Clone the Repository

```bash
git clone https://github.com/Mahmoud-B-Miqdad/Travel-and-Accommodation-Booking-Platform.git
cd Travel-and-Accommodation-Booking-Platform
```

### 2. Configure Application Settings (`appsettings.json` and Environment Variables)

In your project, keep the **database connection string** inside `appsettings.json`, and store all sensitive and environment-specific settings such as API keys, secrets, and credentials in environment variables (e.g., `.env` file or system environment).

---

#### 2.1. Example `appsettings.json`

```json
{
  "ConnectionStrings": {
    "TravelEaseDb": "Server=.;Database=TravelEaseDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### 2.2. Example Environment Variables (.env or system environment)

Store all sensitive credentials here, using double underscores __ to denote nesting for .NET configuration binding:

```bash
# Email settings for sending emails
EmailSettings__ApiKey=SG.your_sendgrid_api_key_here
EmailSettings__SenderEmail=your.email@example.com
EmailSettings__SenderName=Your System Name

# Cloudinary configuration for media storage
CLOUDINARY__CloudName=your_cloud_name
CLOUDINARY__ApiKey=your_cloudinary_api_key
CLOUDINARY__ApiSecret=your_cloudinary_api_secret

# Stripe configuration for payment processing
Stripe__SecretKey=sk_test_your_stripe_secret_key
Stripe__WebhookSecret=whsec_your_stripe_webhook_secret

# SQL Server password or other database password
SA_PASSWORD=YourStrongPassword123!

# Authentication settings
Authentication__SecretForKey=your_authentication_secret_key
Authentication__TokenLifespanMinutes=60
Authentication__Issuer=https://localhost:7159
Authentication__Audience=YourAPIName

# Password Hasher settings
PasswordHasher__SaltSize=16
PasswordHasher__TimeCost=1
PasswordHasher__Secret=your_password_hasher_secret
PasswordHasher__HashLength=20
```
---

### 3. Apply Migrations & Run the Application

Follow the steps below to restore dependencies, apply database migrations, and run the API locally.
#### 📦 Restore Project Dependencies

Make sure all required NuGet packages are installed:

```bash
dotnet restore
```

#### 🧩 Apply Entity Framework Core Migrations
This will create the database and apply any pending migrations:

```bash
dotnet ef database update
```
> ⚠️ **Note:** Ensure that the connection string is correctly configured in `appsettings.json` or your environment variables before running this command.

#### 🚀 Run the Application
Start the API server:

```bash
dotnet run
```
Once running, you can access the application endpoints using the URLs below:

- **Base URL:** [`https://localhost:7159`](https://localhost:7159)
- **Swagger UI (API Documentation):** [`https://localhost:7159/swagger/index.html`](https://localhost:7159/swagger/index.html)

> 💡 **Tip:** The Swagger UI provides an interactive interface to test and explore all available API endpoints.

---

## 🐳 Running the Project with Docker

First, navigate to the project directory:

```bash
cd TravelEase
```

Then, build and run the Docker containers using:

```bash
docker compose up --build
```

Once the containers are up and running, you can access the application via:

http://localhost:8080/swagger – Swagger UI for testing and exploring the API.

> 💡 **Tip:** Ensure Docker is installed and running before executing these commands.

## 💳 Stripe Integration

Stripe is integrated to handle **secure and reliable payments**.

- You can simulate transactions using Stripe **test keys**.
- Invoices are automatically emailed to users after **booking confirmation**.

> 🧪 **Test Mode:** You can use Stripe’s test cards to simulate various payment scenarios during development.

---

## 🧪 Unit Testing

The platform uses **xUnit** for unit testing core business logic and services.

To run the test suite:

```bash
cd TravelEase/TravelEase.Tests
dotnet test
```

---

## Endpoints

> ⚠️ **Authorization Required:**  
> - All endpoints require authentication.  
> - Some actions (like creating or deleting) require `AdminOrOwner` policy.

### Authentication

| HTTP Method | Endpoint             | Description                 |
|-------------|----------------------|-----------------------------|
| POST        | `/api/auth/login`    | Processes a login request   |

### Bookings

| HTTP Method | Endpoint                                             | Description                                                 |
|--------     |------------------------------------------------------|-------------------------------------------------------------|
| GET         | `/api/hotels/{hotelId}/bookings`                     | Retrieves a paginated list of bookings for a specific hotel |
| POST        | `/api/hotels/{hotelId}/bookings`                     | Reserve a room                                              |
| GET         | `/api/hotels/{hotelId}/bookings/{bookingId}`         | Gets a specific booking by its ID within a specific hotel   |
| DELETE      | `/api/api/hotels/{hotelId}/bookings/{bookingId}`     | Deletes a specific booking by its unique identifier         |
| GET         | `/api/hotels/{hotelId}/bookings/{bookingId}/invoice` | Retrieves the invoice for a specific hotel booking          |

### Cities

| HTTP Method | Endpoint                                | Description                                                                       |
|--------     |-----------------------------------------|-----------------------------------------------------------------------------------|
| GET         | `/api/cities`                           | Retrieves a paginated list of cities with optional filtering and hotel inclusion. |
| POST        | `/api/cities`                           | Creates a new city                                                                |
| GET         | `/api/cities/{cityId}`                  | Retrieves a specific city by its unique identifier                                |
| PUT         | `/api/cities/{cityId}`                  | Updates an existing city by its unique identifier                                 |
| DELETE      | `/api/cities/{cityId}`                  | Deletes a specific city by its unique identifier                                  |
| GET         | `/api/cities/{cityId}/photos`           | Retrieves all photos associated with a city based on its unique identifier        |
| GET         | `/api/cities/{cityId}/gallery`          | Uploads an image to the gallery of a specific city                                |
| GET         | `/api/cities/{cityId}/thumbnail`        | Uploads a thumbnail image for a specific city                                     |
| DELETE      | `/api/cities/{cityId}/images/{imageId}` | Deletes a specific image from the city. *(Requires MustBeAdmin)*                  |


### Discounts

| HTTP Method | Endpoint                                              | Description                                                       |
|-------------|-------------------------------------------------------|-------------------------------------------------------------------|
| GET         | `/api/room-types/{roomTypeId}/discounts`              | Retrieves a paginated list of discounts for a specific room type. |
| POST        | `/api/room-types/{roomTypeId}/discounts`              | Creates a new discount for the specified room type.               |
| GET         | `/api/room-types/{roomTypeId}/discounts/{discountId}` | Retrieves a specific discount by its unique identifier.           |
| DELETE      | `/api/room-types/{roomTypeId}/discounts/{discountId}` | Deletes a specific discount by its unique identifier.             |

### Home

| HTTP Method | Endpoint                                       | Description                                                                        |
|-------------|------------------------------------------------|------------------------------------------------------------------------------------|
| GET         | `/api/home/trending-cities`                    | Retrieves the top 5 trending cities.                                               |
| GET         | `/api/home/search-hotels`                      | Searches for hotels based on filters like city, date, rating, and capacity.        |
| GET         | `/api/home/featured-deals`                     | Returns a list of featured hotel deals sorted by discount and final price.         |
| GET         | `/api/home/{guestId}/recently-visited-hotels`  | Retrieves the recent 5 distinct hotels visited by a specific guest. *(Admin only)* |
| GET         | `/api/home/recently-visited-hotels`            | Retrieves the recent 5 distinct hotels visited by the authenticated guest.         |

### Hotels

| HTTP Method | Endpoint                                          | Description                                                                 |
|-------------|---------------------------------------------------|-----------------------------------------------------------------------------|
| GET         | `/api/hotels`                                     | Retrieves all hotels with optional filtering and pagination. *(Admin only)* |
| POST        | `/api/hotels`                                     | Creates a new hotel. *(Requires AdminOrOwner policy)*                       |
| GET         | `/api/hotels/{hotelId}`                           | Retrieves information about a specific hotel. *(Admin only)*                |
| PUT         | `/api/hotels/{hotelId}`                           | Updates information about a specific hotel. *(Requires AdminOrOwner)*       |
| DELETE      | `/api/hotels/{hotelId}`                           | Deletes a hotel by its ID. *(Requires AdminOrOwner)*                        |
| GET         | `/api/hotels/{hotelId}/photos`                    | Retrieves paginated photos associated with a hotel. *(Authenticated)*       |
| POST        | `/api/hotels/{hotelId}/gallery`                   | Uploads an image to the hotel's gallery. *(Requires MustBeAdmin)*           |
| POST        | `/api/hotels/{hotelId}/thumbnail`                 | Uploads a thumbnail image for the hotel. *(Requires MustBeAdmin)*           |
| DELETE      | `/api/hotels/{hotelId}/images/{imageId}`          | Deletes a specific image from the hotel. *(Requires MustBeAdmin)*           |

### Payments

| HTTP Method | Endpoint                                 | Description                                                                                                            |
|-------------|------------------------------------------|------------------------------------------------------------------------------------------------------------------------|
| POST        | `/api/payments/create-payment-intent`    | Creates a Stripe Payment Intent using booking ID, amount, and payment method. Returns client secret. *(Authenticated)* |

### Reviews

| HTTP Method | Endpoint                                                        | Description                                                                                            |
|-------------|-----------------------------------------------------------------|--------------------------------------------------------------------------------------------------------|
| GET         | `/api/hotels/{hotelId}/reviews`                                 | Retrieves a paginated list of reviews for a specific hotel. *(Authenticated)*                          |
| POST        | `/api/hotels/{hotelId}/reviews`                                 | Creates a new review for the specified hotel. *(Requires MustBeGuest policy)*                          |
| GET         | `/api/hotels/{hotelId}/reviews/{reviewId}`                      | Retrieves a specific review by ID within a specific hotel. *(Requires AdminOrOwner policy)*            |


### Room Amenities

| Method | Endpoint                                 | Description                                                       
|--------|------------------------------------------|-------------------------------------------------------------------|
| GET    | `/api/room-amenities`                    | Retrieves a paginated list of room amenities.                     |
| POST   | `/api/room-amenities`                    | Creates a new room amenity.                                       | 
| GET    | `/api/room-amenities/{id}`               | Gets details of a specific room amenity by ID.                    | 
| PUT    | `/api/room-amenities/{id}`               | Updates an existing room amenity.                                 | 
| DELETE | `/api/room-amenities/{id}`               | Deletes a room amenity by ID.                                     | 

### Rooms

| Method | Endpoint                                 | Description                                                     | 
|--------|------------------------------------------|-----------------------------------------------------------------|
| GET    | `/api/hotels/{hotelId}/rooms`            | Retrieves a paginated list of rooms for a specific hotel.       | 
| POST   | `/api/hotels/{hotelId}/rooms`            | Creates a new room in the specified hotel.                      | 
| GET    | `/api/hotels/{hotelId}/rooms/{roomId}`   | Gets details of a specific room by its ID within a hotel.       | 
| PUT    | `/api/hotels/{hotelId}/rooms/{roomId}`   | Updates a room's details inside a hotel.                        | 
| GET    | `/api/hotels/{hotelId}/rooms/available`  | Retrieves available rooms based on check-in/check-out dates.    | 

### RoomTypes

| Method | Endpoint                                        | Description                                                         |
|--------|-------------------------------------------------|---------------------------------------------------------------------|
| GET    | `/api/hotels/{hotelId}/room-types`              | Retrieves a paginated list of room types for a specific hotel.      |
| POST   | `/api/hotels/{hotelId}/room-types`              | Creates a new room type for the specified hotel.                    |
| GET    | `/api/hotels/{hotelId}/room-types/{roomTypeId}` | Retrieves details of a specific room type by its ID within a hotel. |
| DELETE | `/api/hotels/{hotelId}/room-types/{roomTypeId}` | Deletes a specific room type by its ID.                             |

### Users

| HTTP Method | Endpoint                    | Description                                          |
|-------------|-----------------------------|------------------------------------------------------|
| POST        | `/api/users/Register`       | Registers a new user with the provided credentials   |

---

## 🏗️ Architecture Overview

The **Travel and Accommodation Booking Platform** is designed with a clean, scalable, and maintainable multi-layered architecture following industry best practices.

### Layers and Responsibilities

| Layer                       | Description                                                                             | Technologies/Patterns Used                                   |
|-----------------------------|-----------------------------------------------------------------------------------------|--------------------------------------------------------------|
| 🌐 API Layer                | Exposes RESTful endpoints for client interactions, handles HTTP requests and responses. | ASP.NET Core Web API, Controllers, MediatR, FluentValidation |
| 🧩 Application Layer        | Implements business logic with CQRS pattern, handling Commands and Queries separately.  | MediatR, DTOs, CQRS                                          |
| 🏛️ Domain Layer             | Core domain models, entities, enums, and business rules encapsulated in domain objects. | Domain-Driven Design (DDD) principles                        |
| ⚙️ Infrastructure Layer     | Manages data persistence, external service integrations (e.g., Stripe payments).        | Entity Framework Core, Repositories, UnitOfWork pattern      |
| 🔐 Security & Cross-cutting | Authentication, Authorization, Logging, Exception Handling, and API versioning.         | JWT Authentication, Policy-based Authorization, Serilog      |

### Key Highlights

- **CQRS Pattern:** Clean separation between command (write) and query (read) operations via MediatR to enhance scalability and maintainability.  
- **API Versioning:** Supports versioned APIs for backward compatibility and iterative development.  
- **Pagination & Filtering:** Standardized pagination support with consistent response headers (`X-Pagination`).  
- **External Integrations:** Seamless Stripe payment integration with webhook support for real-time payment processing.  
- **Security:** Fine-grained authorization policies ensuring role-based access control (Admin, Owner, Guest).  
- **Testability:** Modular design facilitates comprehensive unit and integration testing.

---

## Get Involved
Your Feedback and Contributions are Welcome!

### Ways to Contribute:
- **Feedback**: Share your thoughts and ideas.
- **Issue Reporting**: Help me by reporting any bugs or issues on GitHub.
- **Code Contributions**: Contribute to the codebase.

### Contact and Support:
Email: [mahmoud.b.miqdad@gmail.com](mailto:mahmoud.b.miqdad@gmail.com).

GitHub: [Mahmoud Miqdad](https://github.com/Mahmoud-B-Miqdad).

Thank you for your interest. I look forward to hearing from you!
