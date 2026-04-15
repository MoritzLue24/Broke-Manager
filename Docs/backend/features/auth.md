# Auth
`/api/auth` ist verantwortlich für die Authentifizierung, also das Registrieren, Einloggen, etc. Es ist NICHT verantwortlich für das Verwalten von Benutzern. Dafür ist `/api/users`, [Users](users.md), zuständig.

## 1. Endpunkte

### 1.1 Registrierung

**POST** `/api/auth/register`
Registriert einen neuen Benutzer.

**Request Body:**
```json
{
    "email": "user@example.com",
    "password": "password123",
    "confirmPassword": "password123"
}
```

**Responses:**
- Status: 201, message: "User successfully registered"
```json
..
"data": {
    "token": "<12312312312blablabal>"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben (z.B. Passwort zu kurz, Email-Format falsch)
- Status: 409, error: "ConflictError", bei bereits registrierter Email


### 1.2 Login
**POST** `/api/auth/login`
Loggt einen Benutzer ein und gibt ein JWT-Token zurück.

**Request Body:**
```json
{
    "email": "peterhans@gmx.de",
    "password": "password123"
}
```

**Responses:**
- Status: 200, message: "Login successful"
```json
..
"data": {
    "token": "jwt-token-here"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben
- Status: 401, error: "UnauthorizedError", bei falschen Credentials
