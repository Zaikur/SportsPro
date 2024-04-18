# SportsPro

## About the Project
SportsPro is a dynamic web application designed for managing sports incidents, products, technicians, and customer data. Leveraging ASP.NET, this application provides a robust platform for efficient and effective operations management.

## Features
- **Customer Management**: Create, update, and delete customer information, handled by `CustomerController`.
- **Incident Management**: Track incidents involving products and assignments to technicians, enabled through `IncidentController`.
- **Product Management**: Manage product details including additions, updates, and deletions through `ProductController`.
- **Technician Management**: Oversee technician details, assignments, and deletions, facilitated by `TechnicianController`.
- **Registration Management**: Handle product registrations and deletions via `RegistrationsController`.

## Contributors
- **Quinton Nelson**
- **Ayden Hofts**
- **Jason Nelson**

## Setup and Configuration
- **Database Configuration**: Utilizes SQL Server with connection details specified in `appsettings.json`.
- **Session and Routing**: Configured session state management and routing to enhance application security and usability.

## Getting Started
To get the application running locally:
1. Clone the repository.
2. Ensure SQL Server is set up as per the connection string in `appsettings.json`.
3. Run the application through an appropriate IDE like Visual Studio.
