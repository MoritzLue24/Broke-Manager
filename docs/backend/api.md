# API

Kuss an claude

- [1. Overview](#1-overview)
- [2. Auth](#2-auth)
    - [POST /auth/register](#post-authregister)
    - [POST /auth/login](#post-authlogin)

- [3. Users](#3-users)
    - [GET /users/me](#get-usersme)
    - [PATCH /users/me](#patch-usersme)
    - [PATCH /users/me/change-password](#patch-usersmechange-password)
    - [DELETE /users/me](#delete-usersme)
    - [GET /users](#get-users) (Admin)
    - [GET /users/{userId}](#get-usersuserid) (Admin)
    - [PATCH /users/{userid}/change-role](#patch-usersuseridchange-role) (Admin)
    - [DELETE /users/{userId}](#delete-usersuserid) (Admin)

- [4. Categories](#4-categories)
    - [GET /categories](#get-categories)
    - [GET /categories/{categoryId}](#get-categoriescategoryid)
    - [POST /categories](#post-categories)
    - [PATCH /categories/{categoryId}](#patch-categoriescategoryid)
    - [POST /categories/{categoryId}/rules](#post-categoriescategoryidrules)
    - [DELETE /categories/{categoryId}/rules?keyword={keyword}](#delete-categoriescategoryidruleskeywordkeyword)
    - [DELETE /categories/{categoryId}](#delete-categoriescategoryid)

- [5. Transactions](#5-transactions)
    - [GET /transactions](#get-transactions)
    - [GET /transactions/{transactionId}](#get-transactionstransactionid)
    - [POST /transactions](#post-transactions)
    - [PATCH /transactions/{transactionId}](#patch-transactionstransactionid)
    - [DELETE /transactions/{transactionId}](#delete-transactionstransactionid)
    - [POST /transactions/auto-assign](#post-transactionsauto-assign)

- [6. Data Models](#6-data-models)

- [7. Enums](#7-enums)

- [8. Error Handling](#8-fehlerformat)

---

## 1. Overview

Basis-URL (Dev): `http://localhost:5180`

Authentifizierung läuft über einen **HttpOnly-Cookie** (`access_token`), der nach Login/Register gesetzt wird.

---

## 2. Auth

### `POST /auth/register`
Registriert einen neuen Benutzer, und automatisch eine Default-Kategorie an. Erstellt keine Session.

**Request Body:** [`RegisterRequest`](#registerrequest)

**Response:** `201 Created`, [`UserResponse`](#userresponse)

---

### `POST /auth/login`
Loggt einen bestehenden Benutzer ein, erstellt eine neue session und setzt diese als cookie.

**Request Body:** [`LoginRequest`](#loginrequest)

**Response:** `200 OK`, [`UserResponse`](#userresponse)

---

### `POST /auth/logout`
Loggt den aktuell eingeloggten Benutzer aus, löscht die Session.

**Response:** `204 No Content`

---

## 3. Users
> Alle Endpunkte erfordern einen gültigen JWT-Cookie.

### `GET /users/me`
Gibt die Daten des aktuell eingeloggten Benutzers zurück.

**Response:** `200 OK`, [`UserResponse`](#userresponse)

---

### `PATCH /users/me`
Aktualisiert die Daten des aktuell eingeloggten Benutzers.

**Request Body:** [`UpdateUserRequest`](#updateuserrequest)

**Response:** `200 OK`, [`UserResponse`](#userresponse)

---

### `PATCH /users/me/change-password`
Ändert das Passwort des aktuell eingeloggten Benutzers.

**Request Body:** [`ChangePasswordRequest`](#changepasswordrequest)

**Response:** `204 No Content`

---

### `DELETE /users/me`
Löscht den aktuell eingeloggten Benutzer, und all seine Daten.

**Response:** `204 No Content`

---

### `GET /users`
Gibt alle Benutzer zurück. Nur für Admins.

**Response:** `200 OK`, List[[`UserResponse`](#userresponse)]

---

### `GET /users/{userId}`
Gibt einen spezifischen Benutzer zurück. Nur für Admins.

**Response:** `200 OK`, [`UserResponse`](#userresponse)

---

### `PATCH /users/{userid}/change-role`
Ändert die Rolle eines Benutzers. Nur für Admins.

**Request Body:** [`ChangeRoleRequest`](#changerolerequest)

**Response:** `200 OK`, [`UserResponse`](#userresponse)

---

### `DELETE /users/{userId}`
Löscht einen spezifischen Benutzer, und all seine Daten. Nur für Admins.

**Response:** `204 No Content`

---

## 4. Categories
> Alle Endpunkte erfordern einen gültigen JWT-Cookie.

### `GET /categories`
Gibt alle Kategorien des aktuell eingeloggten Benutzers zurück.

**Response:** `200 OK`, List[[`CategoryResponse`](#categoryresponse)]

---

### `GET /categories/{categoryId}`
Gibt eine spezifische Kategorie zurück.

**Response:** `200 OK`, [`CategoryResponse`](#categoryresponse)

---

### `POST /categories`
Erstellt eine neue Kategorie.

**Request Body:** [`CreateCategoryRequest`](#createcategoryrequest)

**Response:** `201 Created`, [`CategoryResponse`](#categoryresponse)

---

### `PATCH /categories/{categoryId}`
Aktualisiert eine Kategorie. Funktioniert nicht auf der Default-Kategorie.

**Request Body:** [`UpdateCategoryRequest`](#updatecategoryrequest)

**Response:** `200 OK`, [`CategoryResponse`](#categoryresponse)

---

### `POST /categories/{categoryId}/rules`
Erstellt eine neue Regel für eine Kategorie.

**Request Body:** [`AddRuleRequest`](#addrulerequest)

**Response:** `200 OK`, [`CategoryResponse`](#categoryresponse)

---

### `DELETE /categories/{categoryId}/rules?keyword={keyword}`
Löscht eine Regel einer Kategorie.

**Response:** `204 No Content`

---

### `DELETE /categories/{categoryId}`
Löscht eine Kategorie. Nicht möglich auf der Default-Kategorie. Transaktionen der gelöschten Kategorie werden automatisch der Default-Kategorie zugewiesen.

**Response:** `204 No Content`

---

## 5. Transactions
> Alle Endpunkte erfordern einen gültigen JWT-Cookie.

### `GET /transactions`
Gibt alle Transaktionen des aktuell eingeloggten Benutzers zurück.

**Response:** `200 OK`, List[[`TransactionResponse`](#transactionresponse)]

---

### `GET /transactions/{transactionId}`
Gibt eine spezifische Transaktion zurück.

**Response:** `200 OK`, [`TransactionResponse`](#transactionresponse)

---

### `POST /transactions`
Erstellt eine neue Transaktion. Wird keine `categoryId` angegeben, wird automatisch eine kategorie zugewiesen. Gibt zusätzlich eine Liste von categoryIds zurück, die laut Regelwerk mit der Transaktion matchen würden, inklusive eines confidence scores.

**Request Body:** [`CreateTransactionRequest`](#createtransactionrequest)

**Response:** `201 Created`, [`AutoAssignResponse`](#autoassignresponse)

---

### `PATCH /transactions/{transactionId}`
Aktualisiert eine Transaktion. Wird eine `categoryId` übergeben, wird `categorySource` auf `Manual` gesetzt.

**Request Body:** [`UpdateTransactionRequest`](#updatetransactionrequest)

**Response:** `200 OK`, [`TransactionResponse`](#transactionresponse)

---

### `DELETE /transactions/{transactionId}`
Löscht eine spezifische Transaktion.

**Response:** `204 No Content`

---

### `POST /transactions/auto-assign`
Weist allen Transaktionen, welche auf den Filter passen, automatisch EINE Kategorie zu. Gibt zusätzlich für jede betroffene Transaktion die Kategorien zurück, die eventuell mit der zugewiesenen Kategorie konkurrieren würden, inklusive eines confidence scores. Wird `overwriteManual=true` übergeben, werden auch Transaktionen mit `categorySource=Manual` überschrieben.

**Request Body:** [`AutoAssignRequest`](#autoassignrequest)

**Response:** `200 OK`, List[[`AutoAssignResponse`](#autoassignresponse)]

---

## 6. Data Models

### `RegisterRequest`
```json
{
  "email": "user@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```
| Feld | Pflicht | Beschreibung |
|---|---|---|
| `email` | ja | nicht leer, email format, max. 255 Zeichen |
| `password` | ja | nicht leer |
| `confirmPassword` | ja | nicht leer, muss mit `password` übereinstimmen |

---

### `LoginRequest`
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```
| Feld | Pflicht | Beschreibung |
|---|---|---|
| `email` | ja | nicht leer, email format, max. 255 Zeichen |
| `password` | ja | nicht leer |

---

### `UserResponse`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "role": "User",
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `UpdateUserRequest`
```json
{
  "email": "newemail@example.com"
}
```
Alle Felder optional
| Feld | Pflicht | Beschreibung |
|---|---|---|
| `email` | nein | nicht leer, email format, max. 255 Zeichen |

---

### `ChangePasswordRequest`
```json
{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword",
  "confirmNewPassword": "newpassword"
}
```
| Feld | Pflicht | Beschreibung |
|---|---|---|
| `currentPassword` | ja | nicht leer |
| `newPassword` | ja | nicht leer, darf nicht mit `currentPassword` übereinstimmen |
| `confirmNewPassword` | ja | nicht leer, muss mit `newPassword` übereinstimmen |

---

### `ChangeRoleRequest`
```json
{
  "role": "Admin"
}
```
| Feld | Pflicht | Beschreibung |
|---|---|---|
| `role` | ja | `"User"` oder `"Admin"` |

---

### `CategoryResponse`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "...",
  "name": "Essen",
  "isDefault": false,
  "matchingRules": [
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "keyword": "rewe"
    },
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "keyword": "edeka"
    }
  ],
  "createdAt": "2026-01-01T00:00:00Z"
}
```

### `CreateCategoryRequest`
```json
{
  "name": "Essen",
  "keywords": ["rewe", "edeka"]
}
```

| Feld | Pflicht | Beschreibung |
|---|---|---|
| `name` | ja | nicht leer, max. 255 Zeichen |
| `keywords` | ja, aber darf leer sein | je max. 255 Zeichen, nicht leer |

---

### `UpdateCategoryRequest`
```json
{
  "name": "Essen"
}
```
Alle Felder optional.

| Feld | Pflicht | Beschreibung |
|---|---|---|
| `name` | nein | nicht leer, max. 255 Zeichen |

---

### `AddRuleRequest`
```json
{
  "keyword": "rewe"
}
```
| Feld | Pflicht | Beschreibung |
|---|---|---|
| `keyword` | ja | nicht leer, max. 255 Zeichen |

---

### `TransactionResponse`
```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "...",
    "categoryId": "...",
    "categorySource": "Manual",
    "amount": 20.50,
    "type": "Expense",
    "date": "2026-01-01",
    "title": "Einkauf Rewe",
    "description": "",
    "counterParty": "Rewe GmbH",
    "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `AutoAssignResponse`
```json
{
    "transaction": `TransactionResponse`,
    "conflictingTransactions": [
        {
            "categoryId": "...",
            "score": 0.85
        }
    ]
}
```

---

### `CreateTransactionRequest`
```json
{
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 20.50,
  "type": "Expense",
  "date": "2026-01-01",
  "title": "Einkauf Rewe",
  "description": "",
  "counterParty": "Rewe GmbH"
}
```

| Feld | Pflicht | Beschreibung |
|---|---|---|
| `categoryId` | nein | Wird nicht angegeben → Default-Kategorie |
| `amount` | ja | Muss > 0 sein |
| `type` | ja | `"Income"` oder `"Expense"` |
| `date` | ja | Format `YYYY-MM-DD` |
| `title` | ja | Nicht leer, max. 255 Zeichen |
| `description` | ja | Darf leer sein, max. 500 Zeichen |
| `counterParty` | ja | Darf leer sein, max. 255 Zeichen |

---

### `UpdateTransactionRequest`
```json
{
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 20.50,
  "type": "Expense",
  "date": "2026-01-01",
  "title": "Einkauf Rewe",
  "description": "",
  "counterParty": "Rewe GmbH"
}
```
Alle Felder optional.

| Feld | Pflicht | Beschreibung |
|---|---|---|
| `categoryId` | nein | Wird nicht angegeben → category verändert sich nicht |
| `amount` | nein | Muss > 0 sein |
| `type` | nein | `"Income"` oder `"Expense"` |
| `date` | nein | Format `YYYY-MM-DD` |
| `title` | nein | Nicht leer, max. 255 Zeichen |
| `description` | nein | Darf leer sein, max. 500 Zeichen |
| `counterParty` | nein | Darf leer sein, max. 255 Zeichen |

---

### `AutoAssignRequest`
```json
{
  "filter": {
    "transactionIds": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
    "categoryIds": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
    "from": "2026-01-01",
    "to": "2026-01-31"
  },
  "useCategoryIds": ["3fa85f64-5717-4562-b3fc-2c963f66afa6"],
  "overwriteManual": false
}
```

| Feld | Pflicht | Beschreibung |
|---|---|---|
| `filter` | ja | Schränkt die zu ändernden Transaktionen ein |
| `filter.transactionIds` | nein | Wird nicht angegeben → kein filter |
| `filter.categoryIds` | nein | Wird nicht angegeben → kein filter |
| `filter.from` | nein | Wird nicht angegeben → von "Anfang" an |
| `filter.to` | nein | Wird nicht angegeben → alle transaktionen bis nix |
| `useCategoryIds` | ja | Wird nicht angegeben → Alle kategorien werden verwendet |
| `overwriteManual` | ja | true -> Manual- Zuweisungen werden überschrieben |

---

## 7. Enums

**`type` (TransactionType):** `Income` | `Expense`

**`categorySource` (CategorySource):** `Unmatched` | `Manual` | `Auto` | (TODO: `FromStandingOrder`)

**`role` (Role):** `User` | `Admin`

---

## 8. Fehlerformat

Fehler werden als [RFC 9457 Problem Details](https://www.rfc-editor.org/rfc/rfc9457) zurückgegeben:

```json
{
  "type": "urn:broke-manager:errors:validation",
  "title": "One or more validation errors occurred.",
  "status": 400
}
```

**Fehlercodes (`type`):**

| Code | Status | Beschreibung |
|---|---|---|
| `urn:broke-manager:errors:internal-server-error` | 500 | Unbehandelter Fehler |
| `urn:broke-manager:errors:validation` | 400 | Validierungsfehler |
| `urn:broke-manager:errors:unauthorized` | 401 | Nicht authentifiziert |
| `urn:broke-manager:errors:forbidden` | 403 | Keine Berechtigung |
| `urn:broke-manager:errors:category-not-found` | 404 | Kategorie nicht gefunden |
| `urn:broke-manager:errors:default-category-not-found` | 404 | Default-Kategorie nicht gefunden |
