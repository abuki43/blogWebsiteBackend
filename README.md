# blog webiste  Backend

A minimalist backend implementation of a Medium-like blogging platform using ASP.NET Core and SQLite.

## Features

- Authentication with JWT
- Article creation and management
- User profiles and following system
- Comments system
- Article tagging system

## Tech Stack

- ASP.NET Core 8.0
- Entity Framework Core
- SQLite Database
- JWT Authentication

## Getting Started

1. Clone the repository
```bash
git clone https://github.com/yourusername/medium-clone-backend.git
```

2. Install dependencies
```bash
dotnet restore
```

3. Run migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

The API will be available at `http://localhost:5289`

## Testing the API

This project includes a `request.http` file that can be used with the REST Client extension in VS Code.

1. Install the [REST Client] extension in VS Code
2. Open `request.http`
3. Click "Send Request" above any request to test the API

## API Documentation

See [api-documentation.md](api-documentation.md) for detailed API documentation including:

- Authentication endpoints
- Article management
- User profiles
- Comments
- Following system

#Thanks

