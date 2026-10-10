# Expense Tracker API

A RESTful API for personal expense tracking, built with ASP.NET Core and PostgreSQL. This project demonstrates backend development practices, secure authentication, database management, automated testing, and containerization.

## Features

* **Authentication:** User registration and login using JSON Web Tokens (JWT).
* **Expense Management:** Create and manage personal expenses.
* **Category Management:** Organize expenses into categories.
* **Budget Management:** Manage personal budgets.
* **Dashboard:** Access financial summaries and expense information.
* **Data Persistence:** PostgreSQL integration using Entity Framework Core.
* **Database Migrations:** Version-controlled database schema changes.
* **API Documentation:** Interactive API documentation with Swagger/OpenAPI.
* **Automated Testing:** Unit tests for application services.
* **Containerization:** Run the API and PostgreSQL database using Docker Compose.
* **Centralized Error Handling:** Consistent exception handling through custom middleware.

## Tech Stack

| Technology            | Purpose                                       |
| --------------------- | --------------------------------------------- |
| C#                    | Main programming language                     |
| ASP.NET Core 9        | REST API development                          |
| Entity Framework Core | Object-relational mapping (ORM)               |
| PostgreSQL            | Relational database                           |
| Npgsql                | PostgreSQL provider for Entity Framework Core |
| JWT                   | Authentication and authorization              |
| Swagger / OpenAPI     | API documentation                             |
| xUnit                 | Automated testing                             |
| Moq                   | Mocking dependencies in unit tests            |
| Docker                | Application containerization                  |
| Docker Compose        | Local development environment                 |

## Architecture

The solution follows a layered architecture to separate responsibilities and improve maintainability.

```text
ExpenseTracker/
├── ExpenseTracker.API/
│   ├── Controllers/
│   ├── Middleware/
│   └── Program.cs
├── ExpenseTracker.Application/
│   ├── Repositories/
│   └── Services/
├── ExpenseTracker.Domain/
├── ExpenseTracker.Infrastructure/
│   ├── Repositories/
│   └── ApplicationDbContext.cs
├── ExpenseTracker.Tests/
├── Dockerfile
├── docker-compose.yml
└── README.md
```

**Layer responsibilities:**

* **API:** Exposes HTTP endpoints, configures authentication, and handles incoming requests.
* **Application:** Contains application services and repository interfaces.
* **Domain:** Defines the core entities and domain concepts.
* **Infrastructure:** Implements data access using Entity Framework Core and PostgreSQL.
* **Tests:** Contains automated tests for application behavior.

The project uses dependency injection and the repository and service patterns to help separate business logic from infrastructure concerns.

## API Endpoints

The API is organized into the following functional areas:

| Area      | Description                                 |
| --------- | ------------------------------------------- |
| Auth      | User registration and authentication        |
| Budget    | Budget management                           |
| Category  | Expense category management                 |
| Dashboard | Financial summaries and expense information |
| Expense   | Expense management                          |

For the available routes, HTTP methods, request parameters, and response schemas, use the Swagger documentation.

## Getting Started

### Prerequisites

* [Docker Desktop](https://www.docker.com/products/docker-desktop/) or Docker Engine
* Docker Compose

### 1. Clone the repository

```bash
git clone <YOUR_GITHUB_REPOSITORY_URL>
cd ExpenseTracker
```

### 2. Configure environment variables

Create a `.env` file in the same directory as `docker-compose.yml`, using `.env.example` as a reference.

Example:

```dotenv
POSTGRES_PASSWORD=your-local-database-password
JWT_KEY=your-random-secret-key-at-least-32-characters
```

Use a strong, randomly generated JWT key. Do not commit `.env` or real credentials to version control.

### 3. Start the application

Run the following command from the directory containing `docker-compose.yml`:

```bash
docker compose up --build
```

Docker Compose starts the API and PostgreSQL database. The API waits for the database health check before starting.

### 4. Access the API

Once the containers are running, open Swagger UI:

**http://localhost:8080/swagger**

You can use Swagger to explore the available endpoints and send requests to the API.

### 5. Stop the application

```bash
docker compose down
```

To remove the database volume and its stored data as well:

```bash
docker compose down -v
```

**Warning:** The second command permanently deletes the PostgreSQL data stored in the Docker volume.

## Authentication

The API uses JWT bearer authentication to protect endpoints that require an authenticated user.

Typical authentication flow:

1. Register a user through the authentication endpoints.
2. Log in using the registered credentials.
3. Obtain a JWT access token.
4. Include the token in the `Authorization` header when calling protected endpoints.

Example:

```http
Authorization: Bearer <YOUR_ACCESS_TOKEN>
```

Swagger UI also supports bearer-token authentication through its **Authorize** feature.

## Database

The application uses PostgreSQL for persistent data storage and Entity Framework Core for database access.

Database schema changes are managed through Entity Framework Core migrations.

When running the application with Docker Compose, PostgreSQL data is stored in a named Docker volume, allowing it to persist when containers are stopped and restarted.

## Testing

The project includes automated tests using xUnit and Moq.

To run the test suite, execute the following command from the solution directory:

```bash
dotnet test
```

The test suite has been executed successfully during development.

## Configuration and Security

* Environment-specific configuration is supplied through application settings and environment variables.
* Database credentials and JWT signing keys should be kept outside version control.
* The JWT signing key must be sufficiently long and randomly generated.
* Development configuration should not be reused as production configuration.

Before deploying the application to production, configure HTTPS, production-grade secrets management, database access restrictions, and appropriate logging and monitoring.

## Future Improvements

Potential improvements include:

* Continuous integration with GitHub Actions.
* Additional integration tests.
* API deployment to a cloud platform.
* Enhanced request validation and API error documentation.
* Production logging and monitoring.

