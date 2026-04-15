# Transactions
`/api/transactions` ist verantwortlich für das Verwalten von Transaktionen, also das Erstellen, Abrufen, Aktualisieren und Löschen.

## 1. Endpunkte

### 1.1 Alle Transaktionen abrufen

**GET** `/api/transactions`
Gibt eine Liste aller Transaktionen des aktuell eingeloggten Benutzers zurück.

**Responses:**
- Status: 200, message: "Transactions retrieved successfully"
```json
..
"data": [
    {
        "id": 2,
        "categoryId": 1,
        "date": "2024-01-01",
        "amount": -20.5,
        "counterParty": "John Doe",
        "title": "Essen gehen",
        "categoryName": "Lebensmittel"
    }
]
..
```
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird

### 1.2 Spezifische Transaktion abrufen

**GET** `/api/transactions/{id}`
Gibt die Daten einer spezifischen Transaktion zurück.

**Responses:**
- Status: 200, message: "Transaction retrieved successfully"
```json
..
"data": {
    "id": 2,
    "categoryId": 1,
    "date": "2024-01-01",
    "amount": -20.5,
    "counterParty": "John Doe",
    "title": "Essen gehen",
    "categoryName": "Lebensmittel"
}
..
```
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn die Transaktion nicht gefunden wird

### 1.3 Transaktion erstellen

**POST** `/api/transactions`
Erstellt eine neue Transaktion.

**Request Body:**
```json
{
    "categoryId": 1,
    "date": "2024-01-01",
    "amount": -20.5,
    "counterParty": "John Doe",
    "title": "Essen gehen"
}
```

**Responses:**
- Status: 201, message: "Transaction created successfully"
```json
..
TODO: Nicht sicher?
"data": {
    "id": 5,
    "categoryId": 1,
    "date": "2024-01-01",
    "amount": -20.5,
    "counterParty": "John Doe",
    "title": "Essen gehen",
    "categoryName": "Lebensmittel"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben (z.B. fehlende Felder, ungültiges Datum)
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der user, oder die angegebene Kategorie nicht gefunden wird

### 1.4 Transaktion aktualisieren

**PUT** `/api/transactions/{id}`
Aktualisiert die Daten einer spezifischen Transaktion.

**Request Body:**
```json
{
    "categoryId": 1, // optional
    "date": "2024-01-01", // optional
    "amount": -20.5, // optional
    "counterParty": "John Doe", // optional
    "title": "Essen gehen" // optional
}
```

**Responses:**
- Status: 200, message: "Transaction updated successfully"
```json
..
"data": {
    "id": 2,
    "categoryId": 1,
    "date": "2024-01-01",
    "amount": -20.5,
    "counterParty": "John Doe",
    "title": "Essen gehen",
    "categoryName": "Lebensmittel"
}
..
```
- Status: 400, error: "ValidationError", bei ungültigen Eingaben
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der user, die Transaktion oder die angegebene Kategorie nicht gefunden wird

### 1.5 Transaktion löschen

**DELETE** `/api/transactions/{id}`
Löscht eine spezifische Transaktion.

**Responses:**
- Status: 204 wenn die Transaktion erfolgreich gelöscht wurde.
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der User oder die Transaktion nicht gefunden wird

### 1.6 Alle Transaktionen löschen

**DELETE** `/api/transactions`
Löscht alle Transaktionen des aktuell eingeloggten Benutzers.

**Responses:**
- Status: 204 wenn alle Transaktionen erfolgreich gelöscht wurden.
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird


## 2. Transaktionen filtern

### 2.1 Transaktionen nach Datum filtern

**GET** `/api/transactions?startDate=2024-01-01&endDate=2024-01-31`
Gibt alle Transaktionen zurück, die zwischen `startDate` und `endDate` liegen, inklusive der beiden Daten.

**Responses:**
- Status: 200, message: "Transactions retrieved successfully"
```json
..
"data": [
    {
        "id": 2,
        "categoryId": 1,
        "date": "2024-01-01",
        "amount": -20.5,
        "counterParty": "John Doe",
        "title": "Essen gehen",
        "categoryName": "Lebensmittel"
    }
]
..
```

- Status: 400, error: "ValidationError", bei ungültigen Datumsangaben
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird

### 2.2 Transaktionen nach Kategorien filtern

**GET** `/api/transactions?categoryIds=1&categoryIds=2`
Gibt alle Transaktionen zurück, die zur Kategorie gehören.

**Responses:**
- Status: 200, message: "Transactions retrieved successfully"
```json
..
"data": [
    {
        "id": 2,
        "categoryId": 1,
        "date": "2024-01-01",
        "amount": -20.5,
        "counterParty": "John Doe",
        "title": "Essen gehen",
        "categoryName": "Lebensmittel"
    }
]
..
```
- Status: 400, error: "ValidationError", bei ungültigen Kategorie-IDs
- Status: 401, error: "UnauthorizedError", wenn kein gültiges JWT-Token bereitgestellt wird
- Status: 404, error: "NotFoundError", wenn der Benutzer nicht gefunden wird.
