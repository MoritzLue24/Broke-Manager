# API

Kuss an claude

Basis-URL (Dev): `http://localhost:5180`

Authentifizierung läuft über einen **HttpOnly-Cookie** (`access_token`), der nach Login/Register gesetzt wird.

---

## 1. Auth

### `POST /auth/register`

Registriert einen neuen Benutzer, setzt den JWT-Cookie und legt automatisch eine Default-Kategorie an.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```

**Response:** `201 Created`
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "role": "User",
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `POST /auth/login`

Loggt einen bestehenden Benutzer ein und setzt den JWT-Cookie.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:** `200 OK` *(kein Body)*

---

## 2. Users

> Alle Endpunkte erfordern einen gültigen JWT-Cookie.

### `GET /users/me`

Gibt die Daten des aktuell eingeloggten Benutzers zurück.

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "role": "User",
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `PATCH /users/me`

Aktualisiert die Daten des aktuell eingeloggten Benutzers.

**Request Body:**
```json
{
  "email": "newemail@example.com"
}
```

*(Alle Felder optional)*

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "newemail@example.com",
  "role": "User",
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `PATCH /users/me/change-password`

Ändert das Passwort des aktuell eingeloggten Benutzers.

**Request Body:**
```json
{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword",
  "confirmNewPassword": "newpassword"
}
```

**Response:** `204 No Content`

---

## 3. Categories

> Alle Endpunkte erfordern einen gültigen JWT-Cookie.

### `GET /categories`

Gibt alle Kategorien des aktuell eingeloggten Benutzers zurück.

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "...",
    "name": "Essen",
    "isDefault": false,
    "keywords": ["rewe", "edeka"],
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /categories/{categoryId}`

Gibt eine spezifische Kategorie zurück.

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "...",
  "name": "Essen",
  "isDefault": false,
  "keywords": ["rewe", "edeka"],
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `POST /categories`

Erstellt eine neue Kategorie.

**Request Body:**
```json
{
  "name": "Essen",
  "keywords": ["rewe", "edeka"]
}
```

**Response:** `201 Created`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "...",
  "name": "Essen",
  "isDefault": false,
  "keywords": ["rewe", "edeka"],
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `PATCH /categories/{categoryId}`

Aktualisiert eine Kategorie. Funktioniert nicht auf der Default-Kategorie.

**Request Body:**
```json
{
  "name": "Lebensmittel"
}
```

*(Alle Felder optional)*

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userId": "...",
  "name": "Lebensmittel",
  "isDefault": false,
  "keywords": ["rewe", "edeka"],
  "createdAt": "2026-01-01T00:00:00Z"
}
```

---

### `DELETE /categories/{categoryId}`

Löscht eine Kategorie. Nicht möglich auf der Default-Kategorie. Transaktionen der gelöschten Kategorie werden automatisch der Default-Kategorie zugewiesen.

**Response:** `204 No Content`

---

## 4. Transactions

> Alle Endpunkte erfordern einen gültigen JWT-Cookie.

### `GET /transactions`

Gibt alle Transaktionen des aktuell eingeloggten Benutzers zurück.

**Response:** `200 OK`
```json
[
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
]
```

---

### `GET /transactions/{transactionId}`

Gibt eine spezifische Transaktion zurück.

**Response:** `200 OK`
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

### `POST /transactions`

Erstellt eine neue Transaktion. Wird keine `categoryId` angegeben, wird automatisch die Default-Kategorie verwendet (`categorySource = Unmatched`).

**Request Body:**
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
| `title` | ja | Max. 255 Zeichen |
| `description` | ja | Darf leer sein, max. 500 Zeichen |
| `counterParty` | ja | Darf leer sein, max. 255 Zeichen |

**Response:** `201 Created`
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

### `PATCH /transactions/{transactionId}`

Aktualisiert eine Transaktion. Wird eine `categoryId` übergeben, wird `categorySource` auf `Manual` gesetzt.

**Request Body:**
```json
{
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 35.00,
  "type": "Expense",
  "date": "2026-01-02",
  "title": "Einkauf Aldi",
  "description": "Wocheneinkauf",
  "counterParty": "Aldi GmbH"
}
```

*(Alle Felder optional)*

**Response:** `200 OK` *(selbes Format wie GET)*

---

### `DELETE /transactions/{transactionId}`

Löscht eine spezifische Transaktion.

**Response:** `204 No Content`

---

## Fehlerformat

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

---

## Enums

**`type` (TransactionType):** `Income` · `Expense`

**`categorySource` (CategorySource):** `Unmatched` · `Manual` · `Auto` · `FromStandingOrder`

**`role` (Role):** `User` · `Admin`