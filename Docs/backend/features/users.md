# Users

Das Modul ist verantwortlich für das modifizieren (Password, Email & Rolle) von Userdaten,
das Löschen und Rauslesen, NICHT verantwortlich für Registrieren, Einloggen, etc., siehe [Auth](auth.md)

## 1. Model
* Id: int
* Email: string
* PasswordHash: string
* Role: Role (Admin | User)

## 2. Regeln
* Email ist unique
* Password wird als Hash gespeichert, mit BCrypt
* Rolle regelt Access-Level

## 3. DTOs

**UserResponseDto**
* Id
* Email

## 3. Admin-Endpoints

`GET /api/users`

Liefert eine Liste von allen registrierten Benutztern, oder eine leere liste, falls es keine Benutzter gibt.

Responses:
* Status 200: `List<UserResponseDto>`

---

`GET /api/users/{id}`

Liefert Daten über einen bestimmten User.

Responses:
* Status 200: `UserResponseDto`
* Status 404, falls der Benutzter nicht existiert:
    { "message": "..", "status": 404 }

---

``