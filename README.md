
# Vending Machine Service (VMS)

The Vending Machine Service (VMS) is a RESTful API application designed to facilitate the similation of sale of products in a vinding machine. It caters to two types of users: sellers, who can list products for sale, and buyers, who can purchase these products. The service is built with a focus on security, ease of use, and accessibility, ensuring seamless operations for listing, buying, and deposit management.

## Architecture

The VMS follows the principles of Domain-Driven Design (DDD) and Clean Architecture. The application is structured into four main layers:

1. **API**: This layer handles HTTP requests and responses, routing, and controller actions.
2. **Application**: This layer contains the application logic, including command and query handlers, and interfaces for interacting with the Domain and Infrastructure layers.
3. **Domain**: This layer encapsulates the core business logic and entities of the application.
4. **Infrastructure**: This layer handles data access, external service integrations, and other infrastructure concerns.

## Technologies

The VMS is built using the following technologies:

- **ASP.NET Core 8**: The web application framework used for building the API.
- **Entity Framework Core 8**: The Object-Relational Mapping (ORM) framework used for data access and persistence.
- **CQRS (Command Query Responsibility Segregation)**: The architectural pattern used for separating read and write operations, implemented using MediatR.
- **MediatR**: The library used for implementing the CQRS pattern and mediating requests between objects.
- **JWT (JSON Web Tokens)**: The authentication mechanism used for securing the API endpoints.
- **Serilog**: The logging library used for logging application events and errors.
- **Exception Handling Middleware**: A custom middleware for handling exceptions and returning appropriate error responses.

## Testing

The VMS implements unit testing using the following frameworks and libraries:

- **NUnit**: The unit testing framework used for writing and running tests.
- **Fluent Assertions**: The library used for writing more readable and expressive assertions in tests.
- **Moq**: The mocking library used for creating mock objects and isolating dependencies in tests.

## Getting Started

To get started with the VMS, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution file in your preferred IDE (e.g., Visual Studio, Visual Studio Code).
3. Configure the connection string and other settings in the `appsettings.json` file.
4. Build the solution to restore NuGet packages.
5. Run the application using the provided launch profiles or by running the `dotnet run` command from the project directory.

## Class Diagram 
![Class Diagram3](https://github.com/AkramSamirElhayani/VendingMachine/assets/97411158/f06df3d1-416f-44c8-afba-9f556edef5a0)


### User Story: Vending Machine Service

#### Background
The Vending Machine Service (VMS) is an online platform designed to facilitate the sale and purchase of products through a RESTful API interface. The platform caters to two types of users: sellers, who can list products for sale, and buyers, who can purchase these products. The service is designed with a focus on security, ease of use, and accessibility, ensuring that operations such as listing, buying, and deposit management are seamless and user-friendly.

#### User Roles
- **Seller:** Users who can list products for sale. They are responsible for managing their product listings (create, update, delete) and can view all their listed products.
- **Buyer:** Users who can browse products and purchase them using the funds deposited in their VMS account. They can deposit money into their accounts, purchase products, and check their balance.

#### Functional Requirements

1. **User Registration and Authentication:**
   - As a new user, I want to sign up for the VMS by providing a username and password so that I can securely access the platform.
   - As a seller, I need to log in to manage my product listings.
   - As a buyer, I need to log in to deposit funds, view products, and make purchases.

2. **User Management:**
   - As a buyer or seller, I want to update my profile information to keep my account up to date.
   - As a buyer or seller, I need the ability to delete my account when I no longer need the service.

3. **Product Management (Seller):**
   - As a seller, I want to list a new product by providing details such as the product name, cost, amount available, and my seller ID.
   - As a seller, I need to update my product listings to ensure that all information is current.
   - As a seller, I want to delete a product listing when the product is no longer available for sale.
   - As a seller, I wish to view all my product listings to manage them effectively.

4. **Deposit Management (Buyer):**
   - As a buyer, I want to deposit coins into my VMS account to use them for purchases.
   - As a buyer, I need to reset my deposit to zero if I decide not to make any purchases.

5. **Purchasing Products (Buyer):**
   - As a buyer, I want to view products available for purchase.
   - As a buyer, I wish to buy a product by specifying the product ID and the amount, using the funds available in my account.
   - As a buyer, I expect to receive details about my purchase, including the total spent, the products I have purchased, and any change returned in coins.

#### Non-Functional Requirements

- **Security:** Ensure that user authentication is secure and that passwords are stored securely. Protect against unauthorized access to user accounts and product management features.
- **Reliability:** The system should be reliable, with minimal downtime, to allow users to access and manage their accounts and listings at any time.
- **Scalability:** The system should be able to handle a growing number of users and product listings without degradation in performance.

#### Edge Cases and Access Issues

- Ensure that users cannot deposit negative amounts.
- Prevent buyers from purchasing products that are out of stock or that exceed the deposit amount in their account.
- Ensure that only the seller who created a product can update or delete it.
- Prevent unauthorized users from accessing endpoints that require authentication, especially sensitive operations related to product management and user account management.
- Implement rate limiting to prevent abuse of the /deposit and /buy endpoints.
- Ensure that the change returned to the buyer after a purchase is calculated accurately and in the smallest number of coins possible.

### Acceptance Criteria

- A user can register as a buyer or seller with a username and password.
- Sellers can create, update, view, and delete their product listings.
- Buyers can deposit money into their accounts, view products, purchase products, and receive accurate change.
- The system securely handles user authentication and protects against unauthorized access.
- The system is reliable and performs well under increased load.
- The system accurately handles edge cases and provides meaningful error messages to users when something goes wrong.
