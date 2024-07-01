# C# and .NET Projects Collection

This repository is a collection of small C# and .NET projects, each demonstrating different aspects of software development with the .NET framework. These projects range from simple console applications to more complex applications utilizing various .NET features and libraries.

## Projects Overview

- **ForexAlert**: A console application that monitors foreign exchange rates and sends alerts based on user-defined thresholds. Utilizes .NET's HttpClient for API requests and JSON parsing for response handling.
  - **Technology Stack**: C#, .NET Core

- **HabitLogger**: A simple application for tracking and logging daily habits. Demonstrates basic CRUD operations using Entity Framework Core and SQLite as the database.
  - **Technology Stack**: C#, .NET Core, SQLite

- **PhoneBook**: A console-based phone book application that allows users to manage their contacts. Features include adding, updating, deleting, and searching for contacts. This project showcases the use of Entity Framework Core for database operations.
  - **Technology Stack**: C#, .NET Core, MS SQL Server

## Getting Started

To run these projects, you will need the .NET SDK installed on your machine. Each project targets .NET 8.0, so ensure you have the appropriate version of the SDK.

### Prerequisites

- .NET SDK 8.0 or later
- Visual Studio 2019 or later (optional, for IDE support)

### Running a Project

1. Clone the repository to your local machine.
2. Navigate to the project directory you wish to run, for example, `cd ForexAlert`.
3. Restore the project dependencies: `dotnet restore`.
4. Build the project: `dotnet build`.
5. Run the project: `dotnet run`.

## Contributing

Contributions are welcome! If you have improvements or bug fixes, please feel free to fork the repository and submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
