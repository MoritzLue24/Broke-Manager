# Api
This document describes all the formalities of this REST-Api. 


## 1. Response-Format
Every possible response has the same (json) format. If the response is successfull, `error` is null. The `details` field is not reliable set, only for specific errors, e.g. call stacks (in developement), validation errors.

**Response:**
```json
{
    "data": DTO,    // or null
    "error": {      // or null
        "code": "NOT_FOUND_ERROR",
        "details": {
            "someDetail": "Some message"
        }
    }
}
```
**Error-Codes:**
- `USER_NOT_FOUND`
- TODO
- `SERVER_ERROR`


## 2. Authentication
We use JWT-based authentication. After logging in, the client receives a token as a cookie.

**Get JWT cookie**
```mermaid
flowchart LR
    A
```

## 3. Endpoints

### 3.1 Auth
> [`Auth.md`](../Features/Auth.md)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | [`/api/auth/register`]() | Register a new user |
| POST | [`/api/auth/login`]() | Login and receive JWT token |


### 3.2 Users
> [`Users.md`](../Features/Users.md)

| Method | Endpoint                            | Description         |
| ------ | ----------------------------------- | ------------------- |
| GET    | [`/api/users/me`]()                 | Get current user    |
| PATCH  | [`/api/users/me`]()                 | Update current user |
| PATCH  | [`/api/users/me/change-password`]() | Change password     |
| DELETE | [`/api/users/me`]()                 | Delete current user |

**Admin only:**

| Method | Endpoint                          | Description          |
| ------ | --------------------------------- | -------------------- |
| GET    | [`/api/users`]()                  | Get all users        |
| GET    | [`/api/users/{id}`]()             | Get specific user    |
| PATCH  | [`/api/users/{id}`]()             | Update specific user |
| PATCH  | [`/api/users/{id}/change-role`]() | Change user role     |
| DELETE | [`/api/users/{id}`]()             | Delete specific user |


### 3.3 Categories
> [`Categories.md`](../Features/Categories.md)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | [`/api/categories`]() | Get all categories by current User |
| GET | [`/api/categories/{id}`]() | Get specific category |
| POST | [`/api/categories`]() | Create category |
| PATCH | [`/api/categories/{id}`]() | Update category |
| DELETE | [`/api/categories/{id}`]() | Delete category |
| DELETE | [`/api/categories`]() | Delete all categories by user |

**Keywords:**

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | [`/api/categories/{categoryId}/keywords`]() | Add keyword |
| PATCH | [`/api/categories/{categoryId}/keywords/{keywordId}`]() | Update keyword |
| DELETE | [`/api/categories/{categoryId}/keywords/{keywordId}`]() | Delete keyword |


### 3.4 Transactions
> [`Transactions.md`](../Features/Transactions.md)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | [`/api/transactions`]() | Get & filter all transactions |
| GET | [`/api/transactions/{id}`]() | Get specific transaction |
| POST | [`/api/transactions`]() | Create transaction |
| PATCH | [`/api/transactions/{id}`]() | Update transaction |
| DELETE | [`/api/transactions/{id}`]() | Delete transaction |
| DELETE | [`/api/transactions`]() | Delete all transactions, with filter |
| POST | [`/api/transactions/auto-categorize`]() | Auto-categorize transactions |


### 3.5 Analytics
> [`Analytics.md`](../Features/Analytics.md)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | [`/api/analytics/missing-transactions`]() | Get missing recurring transactions |
| GET | [`/api/analytics/summary`]() | Get income/expense summary |
| GET | [`/api/analytics/timeline`]() | Get financial timeline |
| GET | [`/api/analytics/forecast`]() | Get financial forecast |
| GET | [`/api/analytics/averages`]() | Get category averages |


### 3.6 Transfer
> [`Transfer.md`](../Features/Transfer.md)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | [`/api/transactions/import`]() | Import transactions from CSV |
| POST | [`/api/transactions/export`]() | Export transactions to CSV |

