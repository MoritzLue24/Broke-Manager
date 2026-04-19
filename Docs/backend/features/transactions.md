# Transactions
`/api/transactions` ist verantwortlich für das Verwalten von Transaktionen, also das Erstellen, Abrufen, Aktualisieren, Löschen, sowie automatische Kategorisierung.


- [1. Category auto-detect](#1-category-auto-detect)
    - [1.1 Wertung der Kategorien](#11-wertung-der-kategorien)
    - [1.2 Beim Erstellen einer Transaktion](#12-beim-erstellen-einer-transaktion)
    - [1.3 Nachträgliches Aktualisieren](#13-nachträgliches-aktualisieren-der-kategorie)
- [2. Endpunkte](#2-endpunkte)
    - [2.1 Alle Transaktionen abrufen & filtern](#21-alle-transaktionen-abrufen-und-filtern)
    - [2.2 Spezifische Transaktion abrufen](#22-spezifische-transaktion-abrufen)
    - [2.3 Transaktion erstellen](#23-transaktion-erstellen)
    - [2.4 Transaktion aktualisieren](#24-transaktion-aktualisieren)
    - [2.5 Transaktion löschen](#25-transaktion-löschen)
    - [2.6 Alle Transaktionen löschen](#26-alle-transaktionen-löschen)
- [3. Auto-detect Kategorie](#3-auto-detect-kategorie)
    - [3.1 Kategorie zuordnen für Transaktionen (mit filter)](#31-kategorie-zuordnen-für-transaktionen-mit-filter)
- [4. DTOs](#4-dtos)
    - [4.1 ResponseDTO](#41-responsedto)
    - [4.2 CreateDTO](#42-createdto)
    - [4.3 AutoDetectConflictDTO](#43-autodetectconflictdto)
    - [4.4 UpdateDTO](#44-updatedto)
    - [4.5 CategorizeRequestDTO](#45-categorizerequestdto)


## 1. Category auto-detect
Der Benutzter hat die Möglichkeit einer Transaktion automatisch eine Kategorie zuzuweisen. Dafür wird nach **Keywords** der Kategorien in den Transaktionstiteln gesucht. 

Standardmäßig werden manuell gesetzte Kategorien NICHT überschrieben, Auto-categorization ist deterministisch und basiert ausschließlich auf Keywords.

### 1.1 Wertung der Kategorien
Pro Transaktion wird eine Treffer-Liste erstellt, in absteigender Relevanz sortiert. Die **Relevanz** setzt sich zusammen aus:
- **Anzahl** der Keywords einer Kategorie, die in der Transaktion vorkommen
- **Anteil** der Keywords am Transaktionstitel. z.B. beim Keyword "Essen" und Transaktionstitel "Essen gehen" ist der Anteil 50%, da "Essen" 5 von 10 Not-Whitespace-Zeichen des Titels ausmacht.
- Es werden nur Kategorien zurückgegeben, die mindestens ein Keyword haben, das in der Transaktion vorkommt.
- Es wird IMMER die **default Kategorie** mit niedrigster Relevanz zurückgegeben, auch wenn sie keine Keywords hat, oder die Keywords nicht in der Transaktion vorkommen.

### 1.2 Beim Erstellen einer Transaktion
Beim Erstellen einer Transaktion durch **POST**[/api/transactions](#23-transaktion-erstellen) kann die Kategorie entweder explizit angegeben werden (categorySource=Manual), oder es wird `null` übergeben (bzw nichts), und die Kategorie wird automatisch ermittelt (categorySource=Auto):
- Transaktion wird noch nicht erstellt, es wurde keine Kategorie explizit angegeben.
- Es wird eine Liste von Kategorie-Empfehlungen erstellt (siehe [1.1 Wertung der Kategorien](#11-wertung-der-kategorien)). Der letzte Eintrag ist immer die default Kategorie.
- Der erste Eintrag dieser Liste wird zur Kategorie der Transaktion gesetzt.
- Die Transaktion wird erstellt.
- Sind die ersten n Einträge der Liste gleichwertig, werden diese über `"conflictingCategories"` zurückgegeben.

### 1.3 Nachträgliches aktualisieren der Kategorie
Der Benutzer kann auch nachträglich die Kategorien bestimmter Transaktionen (filter!) automatisch aktualisieren lassen (`"categorySource"=Auto`), siehe [/api/transactions/auto-categorize](#31-kategorie-zuordnen-für-transaktionen,-mit-filter).

Hierfür kann der Benutzter folgende Filter setzten (alle optional), damit nur bestimmte Transaktionen betroffen werden:
- from, Datum
- to, Datum
- transactionIds, um nur ganz bestimmte Transactions zu targetten.
- overwriteManual, bool, setzte auf true um manuell gesetzte Kategorien (categorySource=Manual) zu überschreiben

Es kann auch nur für bestimmte Kategorien "gescannt" werden, sodass nur bestimmte Kategorien angewandt werden.

Es wird zurückgegeben eine Liste von Transaction-, conflictingCategories-Paare.


## 2. Endpunkte

### 2.1 Alle Transaktionen abrufen und filtern

**GET** `/api/transactions`
Gibt eine Liste aller Transaktionen des aktuell eingeloggten Benutzers zurück.
Bei Angabe von den Query-Params `from` & `to`, werden Transaktionen zurückgegeben, die zwischen `startDate` und `endDate` liegen, inklusive der beiden Daten. 
Zusätzlich kann durch den Query-Param `categoryIds` nach Kategorien gefiltert werden.

**Query-Params:**

[FilterDTO](#46-filterdto)

**Responses:**
- http 200, data: List[[ResponseDTO](#41-responsedto)]
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird

### 2.2 Spezifische Transaktion abrufen

**GET** `/api/transactions/{id}`
Gibt die Daten einer spezifischen Transaktion zurück.

**Responses:**
- http 200, data: [ResponseDTO](#41-responsedto)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird

### 2.3 Transaktion erstellen

**POST** `/api/transactions`
Erstellt eine neue Transaktion. Wird keine CategoryId übergeben, wird automatisch eine Festgestellt, categorySource=Auto.

Wird keine Category übergeben, und es gibt n gleichwertige Category-Treffer, werden diese Treffer über "conflictingCategories" zurückgegeben. Es wird trotzden eine Category automatisch gesetzt.

**Request Body:**

[CreateDTO](#42-createdto)

**Responses:**
- http 201, data: [AutoDetectConflictDto](#43-autodetectconflictdto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Eingaben (z.B. fehlende Felder, ungültiges Datum)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die angegebene Kategorie, oder die default-category nicht gefunden wird

### 2.4 Transaktion aktualisieren

**PUT** `/api/transactions/{id}`
Aktualisiert die Daten einer spezifischen Transaktion. Bei übergabe einer categoryId wird categorySource auf Manual gesetzt.

**Request Body:**

[UpdateDTO](#44-updatedto)

**Responses:**
- http 200, data: [ResponseDTO](#41-responsedto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Eingaben
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, die Transaktion oder die angegebene Kategorie nicht gefunden wird

### 2.5 Transaktion löschen

**DELETE** `/api/transactions/{id}`
Löscht eine spezifische Transaktion.

**Responses:**
- http 204, wenn die Transaktion erfolgreich gelöscht wurde.
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die Transaktion nicht gefunden wird

### 2.6 Alle Transaktionen löschen

**DELETE** `/api/transactions`
Löscht alle Transaktionen des aktuell eingeloggten Benutzers.

**Responses:**
- http 204, wenn alle Transaktionen erfolgreich gelöscht wurden.
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird


## 3. Auto-detect Kategorie

### 3.1 Kategorie zuordnen für Transaktionen, mit Filter

**POST** `/api/transactions/auto-categorize`
Kategorisiert ALLE Transaktionen, welche die gegebenen Bedingungen erfüllen, automatisch. Es wird dementsprechend jeweils categorySource=Auto gesetzt.
Bei conflicts, also falls zwei Kategorien auf einer Transaktion die gleiche Wertung haben, wird dies in einer Liste zurückgegeben.

**Request-Body:**

[CategorizeRequestDTO](#45-categorizerequestdto)

**Responses:**
- http 200, data: List[[AutoDetectConflictDTO](#43-autodetectconflictdto)]
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die default-category nicht gefunden wurde


## 4. DTOs

### 4.1 ResponseDTO
**GET**[/api/transactions](#21-alle-transaktionen-abrufen-und-filtern)

**GET**[/api/transactions/{id}](#22-spezifische-transaktion-abrufen)

**PUT**[/api/transactions/{id}](#24-transaktion-aktualisieren)
```json
{
    "id": 2,
    "date": "2024-01-01",
    "amount": -20.5,
    "title": "Essen gehen",
    "counterParty": "John Doe",
    "categorySource": "Manual",
    "category": CategoryResponseDTO
}
```

### 4.2 CreateDTO
**POST**[/api/transactions](#23-transaktion-erstellen)
```json
{
    "categoryId": 1,
    "date": "2024-01-01",
    "amount": -20.5,
    "title": "Essen gehen",
    "counterParty": "John Doe",
}
```
**Validierung**
- `categoryId`
    - optional, default=null
    - non-negative
- `date`
    - optional, default=currentDate
    - DateOnly format, YYYY-MM-DD
- `amount`
    - required
    - 12 Stellen insgesamt, 2 Nachkomma (wird noch nicht validiert, TODO)
- `title`
    - required
    - max. 500 Zeichen
- `counterParty`
    - optional
    - max. 250 Zeichen

### 4.3 AutoDetectConflictDTO
**POST**[/api/transactions](#23-transaktion-erstellen)

**POST**[/api/transactions/auto-categorize](#31-kategorie-zuordnen-für-transaktionen-mit-filter)
```json
{
    "transaction": ResponseDTO,
    "conflictingCategories": [ CategoryResponseDTO ]
}
```

### 4.4 UpdateDTO
**PUT**[/api/transactions/{id}](#24-transaktion-aktualisieren)
```json
{
    "categoryId": 1,
    "date": "2024-01-01",
    "amount": -20.5,
    "title": "Essen gehen",
    "counterParty": "John Doe",
}
```
**Validierung**
- `categoryId`
    - optional
    - non-negative
- `date`
    - optional
    - DateOnly format, YYYY-MM-DD
- `amount`
    - optional
    - 12 Stellen insgesamt, 2 Nachkomma (wird noch nicht validiert, TODO)
- `title`
    - optional
    - max. 500 Zeichen
- `counterParty`
    - optional
    - max. 250 Zeichen

### 4.5 CategorizeRequestDTO
**POST**[/api/transactions/auto-categorize](#31-kategorie-zuordnen-für-transaktionen-mit-filter)
```json

{
    "applyCategoryIds": [1, 2, 3],  // Categories to apply
    "overwriteManual": false,   // false -> ignore transactions with CategorySource=Manual
    "filter": FilterDTO
}
```
**Validierung**
- `applyCategoryIds`
    - optional, default=alle
- `overwriteManual`
    - optional, default=false
- `filter`
    - [siehe FilterDTO](#46-filterdto)

### 4.6 FilterDTO
**GET**[/api/transactions](#21-alle-transaktionen-abrufen-und-filtern)

**POST**[/api/transactions/auto-categorize](#31-kategorie-zuordnen-für-transaktionen-mit-filter)

```json
{
    "from": "YYYY-MM-DD",
    "to": "YYYY-MM-DD",
    "transactionIds": [5, 6, 7],
    "categoryIds": [2, 6, 1],
    "excludeCategoryIds": [1],  // Categories to ignore
}
```
**Validierung**
- `from`
    - optional, default=alle
    - DateOnly format, YYYY-MM-DD
- `to`
    - optional, default=alle
    - DateOnly format, YYYY-MM-DD
- `transactionIds`
    - optional, default=alle
- `categoryIds`
    - optional, default=alle
- `excludeCategoryIds`
    - optional, default=[]