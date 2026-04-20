Overview

This project is a Jira-like Project Management System built using ASP.NET, designed to help teams efficiently manage projects, track issues, and communicate in real time. It provides a structured workflow for managers and team members to collaborate and deliver projects effectively.

Features

Project Management

* Create, update, and delete projects
* Assign team members to projects
* Track project progress and status

Issue Tracking

* Create and manage issues (tasks/bugs)
* Assign issues to team members
* Set priorities, deadlines, and statuses
* Track issue lifecycle (Open → In Progress → Resolved)

Real-Time Communication

* Built-in real-time chat between managers and team members
* Instant updates for project activities
* Improves collaboration and decision-making

User Roles

* Manager

  * Create/manage projects
  * Assign tasks
  * Monitor team progress
* Team Member

  * View assigned tasks
  * Update task status
  * Communicate with manager

Tech Stack

* Backend: ASP.NET
* Frontend: HTML, CSS, JavaScript
* Database: SQL Server / MySQL
* Real-Time Communication: SignalR / WebSockets

Installation & Setup

1. Clone the repository

git clone https://github.com/your-username/project-management-tool.git

2. Navigate to the project folder

cd project-management-tool

3. Configure database

* Update connection string in `appsettings.json`

4. Run migrations

dotnet ef database update

5. Run the application

dotnet run

Screenshots (Optional)

* Dashboard
* Project Creation Page
* Issue Tracking Board
* Chat System

Future Improvements

* Notifications system
* File attachments in tasks
* Role-based access control (RBAC) enhancements
* Analytics dashboard

 Contributing

Contributions are welcome! Feel free to fork the repo and submit a pull request.

License

This project is licensed under the MIT License.
