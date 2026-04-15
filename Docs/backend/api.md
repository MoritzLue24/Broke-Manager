# API-Dokumentation

Diese Dokumentation beschreibt die REST-API für die Broke-Manager Anwendung. Die API ermöglicht die Verwaltung von Benutzern, Transaktionen, Kategorien und Keywords sowie Analysen.

## 1. Base-URL
```
http://localhost:5033/api
```

## 2. Authentifizierung
Die API verwendet JWT-basierte Authentifizierung. Nach dem Login erhält der Benutzer ein Token, das in den `Authorization`-Header eingefügt werden muss:
```
Authorization: Bearer <token>
```

## 3. Response-Format
Alle Responses sind im JSON-Format. Erfolgreiche Responses haben den Status 200 (oder entsprechend), Fehler haben spezifische Statuscodes.
Erfolgreiche Response, wenn der Status nicht 204 ist, haben typischerweise folgendes Format:
```json
{
    TODO: Anwenden
    "data": {},
    "message": "User created successfully"
}
```

## 4. Error Response
Typischerweise wird bei Fehlern folgendes JSON-Format zurückgegeben:
```json
{
    TODO: Anwenden
    "error": "NotFound",
    "message": "Resource not found"
}
```

Ausnahmefall `ValidationError`:
```json
{
    TODO: Anwenden
    "error": "ValidationError",
    "message": "2 Validation error(s) orrcured",
    "details": {
        "Email": ["Invalid Email format"]
        "<SomeField>": ["<SomeError>"]
    }
}
```

Allgemeingültige Fehlercodes:
- 400: Bad Request (z.B. ungültige Eingabe, ValidationError)
- 401: Unauthorized (z.B. fehlendes oder ungültiges Token)
- 403: Forbidden (z.B. Zugriff auf Ressource nicht erlaubt)
- 404: Not Found (z.B. Ressource existiert nicht)
- 409: Conflict (z.B. Email bereits registriert)
- 500: Internal Server Error (unerwarteter Fehler)

## 4. Request- & Response-Bodies
Für detailierte Informationen zu den Request- und Response-Bodies siehe die jeweiligen Abschnitte:
- [Auth](features/auth.md)
- [Benutzer](features/users.md)
- [Transaktionen](features/transactions.md)
- [Kategorien](features/categories.md)
- [Keywords](features/keywords.md)
- [Analysen](features/analytics.md)
