# Users
`/api/users` ist verantwortlich für die Verwaltung von Benutzern, z.B. das Abrufen von Benutzerinformationen, das Aktualisieren von Benutzerdaten, etc. Es ist NICHT verantwortlich für die Authentifizierung (Registrieren, Einloggen, etc.), da dies in `/api/auth`, [Auth](auth.md), behandelt wird.

## 1. Endpunkte

### 1.1 Benutzerinformationen abrufen

**GET** `/api/users/me`
Gibt die Daten des aktuell eingeloggten Benutzers zurück.

**Responses:**
- Status: 200, message: "User data retrieved successfully"
```json
..
"data": {
    "id": 2,
    "email": "dieterhans@yahoo.com",
    "role": "User"
}
..
```
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird

### 1.2 Benutzerinformationen aktualisieren

**PUT** `/api/users/me`
Aktualisiert die Daten des aktuell eingeloggten Benutzers.

**Request Body:**
```json
{
    "email": "dieterhans@yahoo.com" // optional
}
```

**Responses:**
- Status: 200, message: "User data updated successfully"
```json
..
"data": {
    "id": 5,
    "email": "dieterhans@yahoo.com",
    "role": "User"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben (z.B. Email-Format falsch)
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird

### 1.3 Passwort ändern

**PUT** `/api/users/me/password`
Ändert das Passwort des aktuell eingeloggten Benutzers.

**Request Body:**
```json
{
    "currentPassword": "currentPassword123",
    "newPassword": "newPassword123",
    "confirmNewPassword": "newPassword123"
}
```

**Responses:**
- Status: 204, wenn das Password erfolgreich geändert wurde.
- Status: 400, error: "ValidationError", bei ungültigen Eingaben (z.B. neues Passwort zu kurz, neue Passwörter stimmen nicht überein)
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird oder das aktuelle Passwort falsch ist
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird

### 1.4 Benutzer löschen

**DELETE** `/api/users/me`
Löscht den aktuell eingeloggten Benutzer.

**Responses:**
- Status: 204, wenn der Benutzer erfolgreich gelöscht wurde.
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird


## 2. Admin-Endpunkte
Diese Endpunkte sind nur für Benutzer mit der Rolle "Admin" zugänglich.

### 2.1 Alle Benutzer abrufen

**GET** `/api/users`
Gibt eine Liste aller Benutzer zurück.

**Responses:**
- Status: 200, message: "Users retrieved successfully"
```json
..
"data": [
    {
        "id": 2,
        "email": "dieterhans@yahoo.com",
        "role": "User"
    }
]
..
```
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 403, error: "ForbiddenError", wenn der Benutzer keine Admin-Rechte hat

### 2.2 Spezifischen Benutzer abrufen

**GET** `/api/users/{id}`
Gibt die Daten eines spezifischen Benutzers zurück.

**Responses:**
- Status: 200, message: "User retrieved successfully"
```json
..
"data": {
    "id": 2,
    "email": "dieterhans@yahoo.com",
    "role": "User"
}
..
```
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 403, error: "ForbiddenError", wenn der Benutzer keine Admin-Rechte hat
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird

### 2.3 Benutzer aktualisieren

**PUT** `/api/users/{id}`
Aktualisiert die Daten eines spezifischen Benutzers.

**Request Body:**
```json
{
    "email": "newemail@example.com" // optional
}
```

**Responses:**
- Status: 200, message: "User updated successfully"
```json
..
"data": {
    "id": 2,
    "email": "dieter@yahoo.de",
    "role": "User"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben (z.B. Email-Format falsch)
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 403, error: "ForbiddenError", wenn der Benutzer keine Admin-Rechte hat
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird
- Status: 409, error: "ConflictError", bei bereits registrierter Email

### 2.4 Benutzerrolle aktualisieren

**PUT** `/api/users/{id}/role`
Aktualisiert die Rolle eines spezifischen Benutzers.

**Request Body:**
```json
{
    "role": "Admin" // oder "User"
}
```

**Responses:**
- Status: 200, message: "User role updated successfully"
```json
..
"data": {
    "id": 2,
    "email": "dieterhans@yahoo.com",
    "role": "Admin"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben (z.B. ungültige Rolle)
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 403, error: "ForbiddenError", wenn der Benutzer keine Admin-Rechte hat
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird

### 2.5 Benutzer löschen

**DELETE** `/api/users/{id}`
Löscht einen spezifischen Benutzer.

**Responses:**
- Status: 204, wenn der Benutzer erfolgreich gelöscht wurde.
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token
bereitgestellt wird
- Status: 403, error: "ForbiddenError", wenn der Benutzer keine Admin-Rechte hat
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird