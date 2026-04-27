# API-Dokumentation

Diese Dokumentation beschreibt die REST-API für die Broke-Manager Anwendung. Die API ermöglicht die Verwaltung von Benutzern, Transaktionen, Kategorien und Keywords sowie Analysen.

## 1. Base-URL
```
https://localhost:5033/api
```

## 2. Authentifizierung
Die API verwendet JWT-basierte Authentifizierung. Nach dem Login erhält der Client ein Token, in Form eines Kekses `"jwt"`.
Der Keks hat die Optionen:
```cs
new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Strict,
    Expires = DateTime.UtcNow.AddHours(1)
}
```

## 3. Response-Format
Alle Responses sind im JSON-Format. Ist die Response erfolgreich, ist "error" null. Das Field "details" ist nicht verlässlich gegeben, nur z.B. bei VALIDATION_ERRORs. Im developement environment ist der error bei internen server errors detailliert, in production nur sowas wie "Internal server error occured".
```json
{
    "data": DTO,    // or null
    "error": {  // or null
        "code": "NOT_FOUND_ERROR",
        "message": "Resource not found",
        "details": {
            "someDetail": "Some message"
        }
    }
}
```
Errorcodes:
- VALIDATION_ERROR, http-status 400
- UNAUTHORIZED_ERROR, http-status 401
- FORBIDDEN_ERROR, http-status 403
- NOT_FOUND_ERROR, http-status 404
- ALREADY_EXISTS_ERROR, http-status 409
- SERVER_ERROR, http-status 500

## 4. Request- & Response-Bodies
Für detailierte Informationen zu den Request- und Response-Bodies siehe die jeweiligen Abschnitte:
- [Auth](features/auth.md)
- [Benutzer](features/users.md)
- [Transaktionen](features/transactions.md)
- [Kategorien](features/categories.md)
- [Keywords](features/keywords.md)
- [Analysen](features/analytics.md)
