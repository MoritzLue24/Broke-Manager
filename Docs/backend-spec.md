# Finance App Backend Spec

## 1. Ziele

Der Benutzter kommuniziert mit einer REST-API und kann:
* Einen Account / User erstellen, mit Password + Email
* Transaktionen erstellen, bearbeiten, löschen
* Transaktionen automatisch kategorisieren
* Kategorien erstellen, bearbeiten, löschen
* Standart Kategorie ist "Anderes", und wird dem User nicht zum bearbeiten bzw löschen angezeigt 
* Transaktionen kategoriesieren, indem nach Sender/Empfänger- und Titel-Schlüsselwörter gesucht werden
* Finanzübersichten einsehen
* Transaktionen durchsuchen & nach Kategorie filtern 
* Banktransaktionen als CSV importieren
* Zukünftige Ausgaben prognositzieren, indem
  * Kategorien als Wiederholend (Interval: Wöchentlich, Monatlich, Vierteljährlich, Jährlich) markiert werden können
  * Durchschnitt von den restlichen kosten der Kategorien (Interval: Einmalig)

Die RESTful-API wird mit ASP.NET Core implementiert und nutzt den Microsoft SQL-Server als Datenbank.


## 3. Geschäftsregeln

* **Ownership**: Ein User darf nur seine eigenen Daten einsehen & modifizieren
* **Kategorien**:
    * Jeder user hat genau eine **default-kategorie**, die nicht bearbeitet oder gelöscht werden kann. Es können keine keywords hinzugefügt werden.
    * Wird eine Kategorie **gelöscht**, wird der user gefragt ob er auch alle dazugehörigen Transaktionen löschen lassen möchte. Wenn nicht, werden sie der default-kategorie zugeordnet.
    * Beim **erstellen** einer neuen Kategorie werden alle zutreffenden automatischen änderungen der transaction-kategorie zuordnung zurückgegeben, aber noch nicht angewand. Der user wird gefragt. Treffen mehrere Kategorien auf eine Transaction, werden diese als liste zurückgegeben und der user MUSS eine auswählen.
* **Kategorien & Transaktionen**:
    * Eine Transaktion muss **genau**(!) eine Kategorie haben (standardmäßig default-category)
    * Wenn beim manuellen erstellen keine Category angegeben wird, wird **automatisch eine zugeordnet** (bei keinen treffer die default-category). Falls mehrere Kategorien passen, werden diese dem benutzter zur Auswahl vorgeschlagen.
* **Keywords**: Ein Keyword kann in einer Kategorie nur 1x vorkommen.
* **Kategorie Regeln**: Das Automatische kategoriesieren erfolg über das setzten von Title- bzw CounterParty- Schlüsselwörter. Bei mehreren Treffern gibt es folgende Lösung:
    * Ein Fenster erscheint, der User entscheidet welche Kategorie angenommen wird.
* **Beträge**: Positiv = Einnahme, Negativ = Ausgabe
* **CSV-Import**: Stimmen bereits existierende Transaktionen mit Transaktionen aus dem CSV-Import überein, werden diese nicht dupliziert.

## 4. Datenbankdesign

### 4.1 Users

| Id (pk) | Email | Password | 
| ----------- | ----------- | ----------- |
| 1 | example.mail@gmx.de | myPassword123! |

### 4.2 Transaction

| Id (pk) | Date | Amount | CounterParty | Title | UserId (fk) | CategoryId (fk) |
| ----------- | ----------- | ----------- | ----------- | ----------- | ----------- | --------- |
| 1 | 24.01.2006 | -53.67 | Finanzamt | Schulden für keineahnung | 1 | 5 |

### 4.3 Category

| Id (pk) | Name | Interval | isDefault (bit) | UserId (fk) |
| ----------- | ----------- | ----------- | ----------- | ----------- |
| 1 | Essen | Once | 0 | 1 |

### 4.4 Keyword

| Id (pk) | Value | CategoryId (fk) |
| ----------- | ----------- | ----------- |
| 2 | Rewe | 1 |

## 5. DTOs

### 5.1 User

**RegisterRequestDto**
* Email (required, email)
* Password (required, mind. 8 Zeichen)
* ConfirmPassword (required, mind. 8 Zeichen)

**LoginRequestDto**
* Email (required, email)
* Password (required)

**UserResponseDto**
* Id
* Email

**UserUpdateDto**
* Email (optional, email)

**ChangePasswordDto**
* CurrentPassword (required)
* NewPassword (required)
* ConfirmNewPassword (required)

### 5.2 Transaction

**TransactionResponseDto**
* Id
* Date
* Amount
* CounterParty
* Title
* CategoryId
* CategoryName

**TransactionCreateDto**
* Date      (required, DateOnly)
* Amount    (required, decimal(12,2))
* CounterParty  (optional, maxLength(255))
* Title     (optional, maxLength(500))
* CategoryId    (optional)

**TransactionUpdateDto**
* Date  (optional)
* Amount    (optional)
* CounterParty  (optional)
* Title     (optional)
* CategoryId    (optional)
* detectCategory    (bool, optional)    <---?? nicht sicher, seperater api call?

**TransactionImportResposneDto**
* ImportedRows
* Errors

### 5.3 Category

**CategoryResponseDto**
* Id
* Name
* Interval  (string)
* Keywords  (KeywordResponseDto[])
* isDefault (bool)

**CategoryCreateDto**
* Name      (required, maxLength(255))
* Interval  (optional, default: Once)
* Keywords  (optional, KeywordCreateDto[])

**CategoryUpdateDto**
* Name      (optional)
* Interval  (optional)

### 5.4 Keyword

**KeywordResponseDto**
* Id
* Value
* CategoryId

**KeywordCreateDto**
* Value

**KeywordUpdateDto**
* Value (optional)

### 5.5 Other

**SummaryResponseDto**
* LoadedTransactions
* Income
* Expenses
* CategorySummaryDto[]
* ??????...

**CategorySummaryDto**
* Name
* Amount
* ??????...

**ForecastResponseDto**
* Balance
* ??????...

## 6. API-Endpunkte

```
POST /api/auth/register
POST /api/auth/login
```

```
GET /api/users          (als Admin)
GET /api/users/me
PUT /api/users/me
DELETE /api/users/me
DELETE /api/users/{id}  (als Admin)
```

```
GET /api/transactions
GET /api/transactions/{id}
POST /api/transactions
PUT /api/transactions/{id}
DELETE /api/transactions
DELETE /api/transactions/{id}

POST /api/transactions/import
```

```
GET /api/categories
GET /api/categories/{id}
POST /api/categories
PUT /api/categories/{id}
DELETE /api/categories
DELETE /api/categories/{id}
```

```
POST /api/categories/{categoryId}/keywords
PUT /api/categories/{categoryId}/keywords/{keywordId}
DELETE /api/categories/{categoryId}/keywords
DELETE /api/categories/{categoryId}/keywords/{keywordId}
```

```
GET /api/analytics/summary/{days}
GET /api/analytics/forecast/{days}
```

## 7. Offene Entscheidungen
* Mehrere CSV formate?
* Title mit KI Kurzfassen
































## 4. Transaktionen

### 4.1 Alle Transaktionen abrufen
**GET** `/api/transactions`

Gibt alle Transaktionen des eingeloggten Benutzers zurück.

**Query-Parameter:**
- `categoryId` (optional): Filter nach Kategorie
- `startDate` (optional): Startdatum (YYYY-MM-DD)
- `endDate` (optional): Enddatum (YYYY-MM-DD)

**Response:**
```json
[
    {
        "id": 1,
        "date": "2023-01-01",
        "amount": -50.00,
        "counterParty": "Supermarkt",
        "title": "Einkauf",
        "categoryId": 1,
        "categoryName": "Essen"
    }
]
```

### 4.2 Einzelne Transaktion abrufen
**GET** `/api/transactions/{id}`

Gibt eine spezifische Transaktion zurück.

**Response:**
```json
{
    "id": 1,
    "date": "2023-01-01",
    "amount": -50.00,
    "counterParty": "Supermarkt",
    "title": "Einkauf",
    "categoryId": 1,
    "categoryName": "Essen"
}
```

**Fehler:**
- 404: Transaktion nicht gefunden

### 4.3 Transaktion erstellen
**POST** `/api/transactions`

Erstellt eine neue Transaktion.

**Request Body:**
```json
{
    "date": "2023-01-01",
    "amount": -50.00,
    "counterParty": "Supermarkt",
    "title": "Einkauf",
    "categoryId": 1
}
```

**Response:**
```json
{
    "id": 1,
    "date": "2023-01-01",
    "amount": -50.00,
    "counterParty": "Supermarkt",
    "title": "Einkauf",
    "categoryId": 1,
    "categoryName": "Essen"
}
```

**Fehler:**
- 400: Ungültige Eingabe
- 404: Kategorie nicht gefunden

### 4.4 Transaktion aktualisieren
**PUT** `/api/transactions/{id}`

Aktualisiert eine Transaktion.

**Request Body:**
```json
{
    "date": "2023-01-02",
    "amount": -60.00,
    "counterParty": "Supermarkt",
    "title": "Großeinkauf",
    "categoryId": 1
}
```

**Response:**
- Status 204: Erfolgreich aktualisiert

**Fehler:**
- 400: Ungültige Eingabe
- 404: Transaktion oder Kategorie nicht gefunden

### 4.5 Alle Transaktionen löschen
**DELETE** `/api/transactions`

Löscht alle Transaktionen des eingeloggten Benutzers.

**Response:**
- Status 204: Erfolgreich gelöscht

### 4.6 Einzelne Transaktion löschen
**DELETE** `/api/transactions/{id}`

Löscht eine spezifische Transaktion.

**Response:**
- Status 204: Erfolgreich gelöscht

**Fehler:**
- 404: Transaktion nicht gefunden

### 4.7 CSV-Import
**POST** `/api/transactions/import`

Importiert Transaktionen aus einer CSV-Datei.

**Request Body:** Multipart-Form mit Datei `file`

**Response:**
```json
{
    "importedRows": 10,
    "errors": []
}
```

## 5. Kategorien

### 5.1 Alle Kategorien abrufen
**GET** `/api/categories`

Gibt alle Kategorien des eingeloggten Benutzers zurück.

**Response:**
```json
[
    {
        "id": 1,
        "name": "Essen",
        "keywords": [
            {
                "id": 1,
                "value": "Supermarkt",
                "categoryId": 1
            }
        ],
        "interval": "Once",
        "isDefault": false
    }
]
```

### 5.2 Einzelne Kategorie abrufen
**GET** `/api/categories/{id}`

Gibt eine spezifische Kategorie zurück.

**Response:**
```json
{
    "id": 1,
    "name": "Essen",
    "keywords": [
        {
            "id": 1,
            "value": "Supermarkt",
            "categoryId": 1
        }
    ],
    "interval": "Once",
    "isDefault": false
}
```

**Fehler:**
- 404: Kategorie nicht gefunden

### 5.3 Kategorie erstellen
**POST** `/api/categories`

Erstellt eine neue Kategorie.

**Request Body:**
```json
{
    "name": "Transport",
    "keywords": [
        {
            "value": "Bahn"
        }
    ],
    "interval": "Monthly"
}
```

**Response:**
```json
{
    "id": 2,
    "name": "Transport",
    "keywords": [
        {
            "id": 2,
            "value": "Bahn",
            "categoryId": 2
        }
    ],
    "interval": "Monthly",
    "isDefault": false
}
```

**Fehler:**
- 400: Ungültige Eingabe

### 5.4 Kategorie aktualisieren
**PUT** `/api/categories/{id}`

Aktualisiert eine Kategorie.

**Request Body:**
```json
{
    "name": "Transport & Mobilität",
    "interval": "Weekly"
}
```

**Response:**
- Status 204: Erfolgreich aktualisiert

**Fehler:**
- 400: Ungültige Eingabe
- 404: Kategorie nicht gefunden

### 5.5 Alle Kategorien löschen
**DELETE** `/api/categories`

Löscht alle Kategorien des eingeloggten Benutzers (außer Default-Kategorie).

**Response:**
- Status 204: Erfolgreich gelöscht

### 5.6 Einzelne Kategorie löschen
**DELETE** `/api/categories/{id}`

Löscht eine spezifische Kategorie.

**Response:**
- Status 204: Erfolgreich gelöscht

**Fehler:**
- 404: Kategorie nicht gefunden
- 409: Default-Kategorie kann nicht gelöscht werden

## 6. Keywords

### 6.1 Keyword erstellen
**POST** `/api/categories/{categoryId}/keywords`

Erstellt ein neues Keyword für eine Kategorie.

**Request Body:**
```json
{
    "value": "Tankstelle"
}
```

**Response:**
```json
{
    "id": 3,
    "value": "Tankstelle",
    "categoryId": 2
}
```

**Fehler:**
- 400: Ungültige Eingabe
- 404: Kategorie nicht gefunden
- 409: Keyword bereits vorhanden

### 6.2 Keyword aktualisieren
**PUT** `/api/categories/{categoryId}/keywords/{keywordId}`

Aktualisiert ein Keyword.

**Request Body:**
```json
{
    "value": "Autotankstelle"
}
```

**Response:**
- Status 204: Erfolgreich aktualisiert

**Fehler:**
- 400: Ungültige Eingabe
- 404: Keyword oder Kategorie nicht gefunden
- 409: Keyword bereits vorhanden

### 6.3 Alle Keywords löschen
**DELETE** `/api/categories/{categoryId}/keywords`

Löscht alle Keywords einer Kategorie.

**Response:**
- Status 204: Erfolgreich gelöscht

**Fehler:**
- 404: Kategorie nicht gefunden

### 6.4 Einzelnes Keyword löschen
**DELETE** `/api/categories/{categoryId}/keywords/{keywordId}`

Löscht ein spezifisches Keyword.

**Response:**
- Status 204: Erfolgreich gelöscht

**Fehler:**
- 404: Keyword oder Kategorie nicht gefunden

## 7. Analysen

### 7.1 Zusammenfassung
**GET** `/api/analytics/summary/{days}`

Gibt eine finanzielle Zusammenfassung für die letzten X Tage.

**Response:**
```json
{
    "income": 1500,
    "expenses": 1200
}
```

### 7.2 Prognose
**GET** `/api/analytics/forecast/{days}`

Gibt eine finanzielle Prognose für die nächsten X Tage.

**Response:**
```json
{
    "balance": 300
}
```

