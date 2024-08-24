# MaintenancePlanner
Work in progress - project is still on early development stage. :hammer_and_wrench:
Main goal is to complete simple application and learn how to work with microservices. 
Business model is simple - it should be an app to keep record of repairs and spare parts in company. Application is inspired by real life UMUP. 

## What's inside?
Currently application is composed from three main microservices and an event bus:
- WarehouseServiceAPI: simple CRUD module to store data about available spare parts.
- IdentityServiceAPI: module which sotres data about users and workers.
- ActionServiceAPI: here we store data about conducted actions, f.e. repairs.
- EventBus: currently uses simple implementation od RabbitMQ to handle communication between modules in eventual consistency pattern.

More details about implementation of those services in later sections. 

## What's next?
#### Technical side:
There's still a lot of technical, safety and resiliency issues that must be resolved. This section will be properly updated on later stage of development.

#### Business side:
- Refactorize WarehouseServiceAPI to use Event Sourcing.
- Add new module which can store data about machines in company.

## Getting started
Currently application is on an early stage which means that database, secrets and other systems are currently mocked or oversimplified. Now all you have to do is clone the repository and start it using docekr compose up.

# Services details
### ActionServiceAPI:
Currently most important service in application. Stores data about taken actions, used parts and those performing the action.
Module is created using in clean architecture and utilizes CQRS pattern using MediatR package. Validation of input data is realized with FluentValidation library.
Service stores basic information about employees and spare parts (such as IDs and current inventory levels) in its own database and rely on eventual consistency approach.
Communication inside is conducted by using domain events send by mediator.

### IdentityServiceAPI:
Module which stores data about application users and factory workers realized with Microsoft Identity package.
Custom endpoints created to fit into business demands. Mapping is realized with AutoMapper.

### WarehouseServiceAPI:
Module stores data about available spare parts using MS SQL server.

### EventBus:
Event bus is realized using RabbitMQ. Most important task is to ensure eventual consistency between modules as I decided to rely on eventual consistency. 
Currently this is a working prototype, there is still much to do in order to ensure resiliency thus event bus and this section will be properly updated in later stage of development.
