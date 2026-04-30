# Infrastructure
In most cases, this layer implements technical details of interfaces defined in the application layer. With the database configuration as an exception.

## 1. Database
The persistence is implemented through an ORM, in this case the **Entity Framework Core**
We load the aggregates, and entities from the domain layer, and map them to a database schema.

The receiving & modification of this data is happening through repositories, which are defined inside the Application layer, and implemented in this layer.


### 1.1 Schema
Is a not fully direct, but mostly, reflection of the domain model, with one table per entity.

**General**
* Primary keys are not handled by the database, but by the domain as `Guid`s -> not auto increment
* Dates are not set automatically, also handled by the domain
* Enums are mapped to strings, not integers
* No navigational properties need to be configured since we have one aggregate for every entity

**Delete behavior**
* User -> Category, User -> Transaction: Cascade
* Category -> Keyword: Cascade (implicit with `OwnsMany`)
* Category -> Transaction: Restrict. We want to force the application to assign each transaction to the default-category before deletion.

**Other**
* For keywords, we create a separate table because managing a list inside a `nvarchar` column is shit.
* `nvarchar` max-length:
    * email: 255
    * hash: 128
    * category name: 100
    * keywords: 255
    * transaction title: 255
    * transaction counterparty: 255 
    * enums: 50
* Index:
    - Email, unique
    - `userId` on Transaction & Category
    - `userId + name` on Category, unique
    - `userId + isDefault` on Category
    - `categoryId + value` on Keyword, unique
    - `categoryId` on Transaction
    - `categoryId + date` on Transaction


### 1.2 Data retrieval / manipulation
Is handled with Repositories, which are defined in the [Application layer](../Architecture.md) and implemented here.

> `SaveChangesAsync()` is not implemented in these repositories. This is inside a `UnitOfWork` implementation 

The `UnitOfWork` acts as a transaction boundary and ensures that either all changes within a business operation are persisted successfully, or none are applied.


## 2. Hashing
Before the password of a  `User` is created or updated, it is hashed with a hashing function, which is implemented here. The domain layer receives this hash and assumes that the string is hashed.

We use `BCrypt` for hashing.


## 3. JWT
After successful login, the system issues a signed access token that contains the minimal required user information to identify and authorize the user in subsequent requests.

TODO

## 4. Später: Logging, Caching (TODO)