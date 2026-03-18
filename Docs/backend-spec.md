# Finance App Backend Spec

## 1. Ziele

Der Benutzter kommuniziert mit einer REST-API und kann:
* Einen Account / User erstellen, mit Password + Email
* Transaktionen erstellen, bearbeiten, löschen
* Transaktionen automatisch kategorisieren
* Kategorien erstellen, bearbeiten, löschen
* Standart Kategorie ist "Anderes", und wird dem User nicht zum bearbeiten bzw löschen angezeigt 
* Transaktionen kategoriesieren, indem nach Sender/Empfänger- bzw Titel-Schlüsselwörter gesucht werden
* Finanzübersichten einsehen
* Transaktionen durchsuchen & nach Kategorie filtern 
* Banktransaktionen als CSV importieren
* Zukünftige Ausgaben prognositzieren, indem
  * Kategorien als Wiederholend (Interval: Wöchentlich, Monatlich, Vierteljährlich, Jährlich) markiert werden können
  * Durchschnitt von den restlichen kosten der Kategorien (Interval: Einmalig)

Die RESTful-API wird mit ASP.NET Core implementiert und nutzt den Microsoft SQL-Server als Datenbank.

## 2. Domänenmodell

```mermaid
classDiagram
    class User {
        +id: Int
        +email: String
        +password: String
    }

    class Transaction {
        +id: Int
        +date: Date
        +amount: Decimal
        +title: String
        +counterParty: String
    }

    class Category {
        +id: Int 
        +name: String
        +intervall: Intervall
    }

    class Interval {
        <<enumeration>>
        ONCE
        WEEKLY
        MONTHLY
        QUARTERLY
        YEARLY
    }

    class Keyword {
        +id: Int
        +value: String
        +KeywordType: KeywordType
    }

    class KeywordType {
        <<enumeration>>
        TitleKeyword,
        CounterPartyKeyword
    }

    class CSVImport {
        datei: Datei
    }

    

    User "One" --> "Many" Transaction : defines
    User "One" --> "Many" Category : defines

    Category "One" --> "One" Interval : has
    Category "One" --> "Many" Transaction : categorises
    Category "One" --> "Many" Keyword : has

    Keyword "One" --> "One" KeywordType : has

    Transaction "One" --> "One" Category : has
    User "One" --> "One" CSVImport : runs
    CSVImport "One" --> "Many" Transaction : imports
```

## 3. Geschäftsregeln

* **Ownership**: Ein User darf nur seine eigenen Daten einsehen & modifizieren
* **Kategorien & Transaktionen**: Eine Transaktion muss genau(!) eine Kategorie haben (standardmäßig "Anderes")
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

| Id (pk) | UserId (fk) | Date | Amount | CounterParty | Title | CategoryId (fk) |
| ----------- | ----------- | ----------- | ----------- | ----------- | ----------- | --------- |
| 1 | 1 | 24.01.2006 | -53.67 | Finanzamt | Schulden für keineahnung | 5 |

### 4.3 Category

| Id (pk) | UserId (fk) | Name | Interval |
| ----------- | ----------- | ----------- | ----------- |
| 1 | 1 | Essen | Once |

### 4.4 Keyword

| Id (pk) | CategoryId (fk) | Value | KeywordType |
| ----------- | ----------- | ----------- | ----------- |
| 2 | 1 | Rewe | CounterParty |

## 5. DTOs

### 5.1 User

| UserCreate | UserUpdate | UserResponse |
| ----------- | ----------- | ----------- |
| Email | Email | Id |
| Password | Password | Email |

### 5.2 Transaction

| TransactionCreate | TransactionUpdate | TransactionResponse |
| ----------- | ----------- | ----------- |
| Date | Date | Id |
| Amount | Amount | Date |
| CounterParty | CounterParty | Amount |
| Title | Title | CounterParty |
|  |  | Title |

### 5.3 Category

| CategoryCreate | CategoryUpdate | CategoryResponse |
| ----------- | ----------- | ----------- |
| Name | Name | Id |
| TitleKeywords | TitleKeywords | Name |
| CounterPartyKeywords | CounterPartyKeywords | TitleKeywords |
| Interval | Interval | CounterPartyKeywords |
|  |  | Interval |

## 6. API-Endpunkte

**User**
```
GET /api/users        --> UserResponse[]
GET /api/users/{id}   --> UserResponse
POST /api/users       <-- UserCreate
PUT /api/users/{id}   <-- UserUpdate
DELETE /api/users/{id}
```

**Transaction**
```
GET /api/users/{userId}/transactions  --> TransactionResponse[]
GET /api/transactions/{id}            --> TransactionResponse
POST /api/users/{userId}/transactions <-- TransactionCreate
PUT /api/transactions/{id}            <-- TransactionUpdate
DELETE /api/transactions/{id}
```

**Category**
```
GET /api/users/{userId}/categories  --> CategoryResponse[]
GET /api/categories/{id}            --> CategoryResponse
POST /api/users/{userId}/categories <-- CategoryCreate
PUT /api/categories/{id}            <-- CategoryUpdate
DELETE /api/categories/{id}
```

**CSV-Import**
```
TODO
```

## 7. Offene Entscheidungen
* CSV-Format ? 
* Title mit KI Kurzfassen
