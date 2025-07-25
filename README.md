# Live C# Playground Collaboration Hub

## Overview

**Live C# Playground Collaboration Hub** is a web-based platform for real-time collaborative editing and sharing of C# code snippets. It enables multiple users to create, join, and manage live coding sessions, making it ideal for education, interviews, or remote team collaboration.

## Key Features

- **Live Collaborative Sessions:** Users can create and join sessions to edit C# code together in real time.
- **Session Management:** Track session history, participants, ownership, and activity status for each collaboration instance.
- **Code Snippet Integration:** Each session is linked to a code snippet, allowing focused work on specific pieces of code.
- **Participant Roles:** Manage session ownership and participant lists, including join codes for secure access.
- **Edit History:** Automatic tracking of session edits with user attribution for accountability.
- **User Management:** Unique user profiles with support for blocking and secure access.

## Project Structure

- `src/Core/Entities`: Domain models (Sessions, Participants, Users, CodeSnippets, EditHistory)
- `src/Infrastructure/DbContext`: Entity Framework database context and schema configuration
- `src/Infrastructure/Repositories`: Data access logic for sessions and participants
- `src/Web`: ASP.NET Core server-side Blazor app (entry point and DI setup)
- `src/Tests`: Unit tests for core session features and administrative actions

## Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Ovdikos/Live-C-Playground-Collaboration-Hub.git
   cd Live-C-Playground-Collaboration-Hub
   ```

2. **Configure Database Connection**
   - Update `DefaultConnection` string in `src/Web/appsettings.json` to point to your SQL Server instance.
  
    ```bash
   {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
      "AllowedHosts": "*",
        "ConnectionStrings": {
          "DefaultConnection": "your connection string"
        },
          "Jwt" : {
            "Key" : "generate your key and put it here"
          }
    }
   ```

3. **Run the Application**
   ```bash
   dotnet build
   dotnet run --project src/Web
   ```
   The app uses ASP.NET Core Blazor Server. Access it in your web browser at the URL shown in the terminal.

## Usage

- Create a user account and log in/use a sql scripts to create basic database.
- Start a new collaboration session or join an existing one with a join code.
- Edit C# snippets with other participants in real time.
- Track session history and participant activity.

## Technologies

- **C# / .NET 8**
- **ASP.NET Core (Blazor Server)**
- **Entity Framework Core (SQL Server)**
- **xUnit, FluentAssertions** (Testing)
