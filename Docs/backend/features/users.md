# Users
`/api/users` ist verantwortlich für die Verwaltung von Benutzern, z.B. das Abrufen von Benutzerinformationen, das Aktualisieren von Benutzerdaten, etc. Es ist NICHT verantwortlich für die Authentifizierung (Registrieren, Einloggen, etc.), da dies in `/api/auth`, [Auth](auth.md), behandelt wird.

- [1. Endpoints](#1-endpoints)
    - [1.1 Benutzerinformationen abrufen](#11-benutzerinformationen-abrufen)
    - [1.2 Benutzerinformationen aktualisieren](#12-benutzerinformationen-aktualisieren)
    - [1.3 Passwort ändern](#13-passwort-ändern)
    - [1.4 Benutzer löschen](#14-benutzer-löschen)
- [2. Admin Endpoints](#2-admin-endpoints)
    - [2.1 Alle Benutzer abrufen](#21-alle-benutzer-abrufen)
    - [2.2 Spezifischen Benutzer abrufen](#22-spezifischen-benutzer-abrufen)
    - [2.3 Benutzer aktualisieren](#23-benutzer-aktualisieren)
    - [2.4 Rolle aktualisieren](#24-benutzerrolle-aktualisieren)
    - [2.5 Benutzer löschen](#25-benutzer-löschen)
- [3. DTOs](#3-dtos)
    - [3.1 ResponseDTO](#31-responsedto)
    - [3.2 UpdateDTO](#32-updatedto)
    - [3.3 ChangePasswordDTO](#33-changepassworddto)
    - [3.4 ChangeRoleDTO](#34-changeroledto)


## 1. Endpoints

### 1.1 Benutzerinformationen abrufen

**GET** `/api/users/me`
Gibt die Daten des aktuell eingeloggten Benutzers zurück.

**Responses:**
- http 200, data: [ResponseDTO](#31-responsedto)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird

### 1.2 Benutzerinformationen aktualisieren

**PUT** `/api/users/me`
Aktualisiert die Daten des aktuell eingeloggten Benutzers.

**Request Body:**

[UpdateDTO](#32-updatedto)

**Responses:**
- http 200, data: [ResponseDTO](#31-responsedto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Eingaben (z.B. Email-Format falsch)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird
- http 409, status: ALREADY_EXISTS_ERROR, bei bereits registrierter Email

### 1.3 Passwort ändern

**PATCH** `/api/users/me/change-password`
Ändert das Passwort des aktuell eingeloggten Benutzers.

**Request Body:**

[ChangePasswordDTO](#33-changepassworddto)

**Responses:**
- http 204, wenn das Password erfolgreich geändert wurde.
- http 400, status: VALIDATION_ERROR, bei ungültigen Eingaben (z.B. Passwörter stimmen nicht überein).
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird

### 1.4 Benutzer löschen

**DELETE** `/api/users/me`
Löscht den aktuell eingeloggten Benutzer.

**Responses:**
- http 204, wenn der Benutzer erfolgreich gelöscht wurde.
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird


## 2. Admin-Endpoints
Diese Endpunkte sind nur für Benutzer mit der Rolle "Admin" zugänglich.

### 2.1 Alle Benutzer abrufen

**GET** `/api/users`
Gibt eine Liste aller Benutzer zurück.

**Responses:**
- http 200, data: List[[ResponseDTO](#31-responsedto)]
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 403, status: FORBIDDEN_ERROR, wenn der Benutzer keine Admin-Rechte hat

### 2.2 Spezifischen Benutzer abrufen

**GET** `/api/users/{id}`
Gibt die Daten eines spezifischen Benutzers zurück.

**Responses:**
- http 200, data: [ResponseDTO](#31-responsedto)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 403, status: FORBIDDEN_ERROR, wenn der Benutzer keine Admin-Rechte hat
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird

### 2.3 Benutzer aktualisieren

**PUT** `/api/users/{id}`
Aktualisiert die Daten eines spezifischen Benutzers.

**Request Body:**

[UpdateDTO](#32-updatedto)

**Responses:**
- http 200, data: [ResponseDTO](#31-responsedto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Eingaben (z.B. Email-Format falsch)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 403, status: FORBIDDEN_ERROR, wenn der Benutzer keine Admin-Rechte hat
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird
- http 409, status: ALREADY_EXISTS_ERROR, bei bereits registrierter Email

### 2.4 Benutzerrolle aktualisieren

**PATCH** `/api/users/{id}/change-role`
Aktualisiert die Rolle eines spezifischen Benutzers.

**Request Body:**

[ChangeRoleDTO](#34-changeroledto)

**Responses:**
- http: 200, data: [ResponseDTO](#31-responsedto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Eingaben (z.B. Email-Format falsch)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 403, status: FORBIDDEN_ERROR, wenn der Benutzer keine Admin-Rechte hat
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird

### 2.5 Benutzer löschen

**DELETE** `/api/users/{id}`
Löscht einen spezifischen Benutzer.

**Responses:**
- http 204, wenn der Benutzer erfolgreich gelöscht wurde.
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 403, status: FORBIDDEN_ERROR, wenn der Benutzer keine Admin-Rechte hat
- http 404, status: NOT_FOUND_ERROR, wenn der Benutzer nicht gefunden wird


## 3. DTOs

### 3.1 ResponseDTO

**GET** [/api/users/me](#11-benutzerinformationen-abrufen)

**PUT** [/api/users/me](#12-benutzerinformationen-aktualisieren)

**GET** [/api/users](#21-alle-benutzer-abrufen)

**GET** [/api/users/{id}](#22-spezifischen-benutzer-abrufen)

**PUT** [/api/users/{id}](#23-benutzer-aktualisieren)

**PATCH** [/api/users/{id}/change-role](#24-benutzerrolle-aktualisieren)

```json
{
    "id": 2,
    "email": "dieterhans@yahoo.com",
    "role": "User",
    "createdAt": "YYYY-MM-DD HH:MM:SS.mmm"
}
```

### 3.2 UpdateDTO

**PUT** [/api/users/me](#12-benutzerinformationen-aktualisieren)

**PUT** [/api/users/{id}](#23-benutzer-aktualisieren)

```json
{
    "email": "dieterhans@yahoo.com"
}
```
**Validierung**
- `email`
    - optional, default=null
    - Email-Format
    - Max. 255 Zeichen

### 3.3 ChangePasswordDTO

**PATCH** [/api/users/me/change-password](#13-passwort-ändern)

```json
{
    "currentPassword": "currentPassword123",
    "newPassword": "newPassword123",
    "confirmNewPassword": "newPassword123"
}
```
**Validierung**
- `currentPassword`
    - required
    - Min. 8 Zeichen
    - Max. 255 Zeichen
- `newPassword`
    - required
    - Min. 8 Zeichen
    - Max. 255 Zeichen
- `confirmNewPassword`
    - required
    - Equals `newPassword`
    - Min. 8 Zeichen
    - Max. 255 Zeichen

### 3.4 ChangeRoleDTO

**PATCH** [/api/users/{id}/change-role](#24-benutzerrolle-aktualisieren)

```json
{
    "role": "Admin"
}
```
**Validierung**
- `role`
    - optional, default=null
    - in ("User", "Admin")
