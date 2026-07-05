# Overview

The backend provides a REST API that allows users to manage their personal finances.

## 1. Aims
The user can:

**User**
- Change email & password, delete himself / herself
- Configure of how/if he/she wants to get warnings about missing transactions of standing orders. And notifications about incoming transactions.

**CRUD**
- Create, update, delete transactions
- Create, update, delete categories
- Create, update, delete standing orders

**Transfer**
- Import transactions from Bank-CSV
- Import (all) user data from CSV
- Export all user data to CSV

**Filter**
- Filter & search transactions

**Categorization**
- Manually assign categories & standing orders to transactions
- Assign standing orders to categories
- Automatically assign categories & standing orders to transactions using keywords, set in each category / standing order

**Notifications**
- Be reminded if transactions of standing orders are missing

**Analytics**
- View summary of transactions inside a given range & of specified categories (optional), containing:
	- Category distribution
	- Net balance
	- Total expenses & income
	- A "remaining € this month" section: All income (+fixed incomes for whole month) - all expenses (+fixed expenses for whole month)
- View a history over last months for income / expenses of certain category(s) +  the average net of these category(s)
- View a forecast / prognose for the next months / years / TODO, containing:
	- Future net balance
	- Timeline with expenses, income & net per grouping (=day / week / month / TODO)
	- TODO: considered with estimated development of expenses / incomes of categories

> for the accessibility, see [api.md](./api.md)  

## 2. Technical implementation
**Stack**
- Containerization: *Docker*
- Runtime: *.NET 8*
- Language: *C# 12(?), Nullable enabled, Implicit usings*
- API: *ASP.NET Core Web Api*
- ORM: *Entity Framework Core 8*
- Database: *PostgreSQL*
- Hashing: *BCrypt.NET*
- Authentication: *Session-Based*

**Architecture**
We follow Domain-Driven-Design (DDD) principles, with a Clean Architecture layering. See [Architecture.md](architecture.md) for more.