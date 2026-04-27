# Analytics
`/api/analytics/` beschäftigt sich mit der Analyse der Transaktionsdaten.

- [1. Overview](#1-overview)
- [2. Endpoints](#2-endpoints)
    - [2.1 Fehlende Transaktionen](#21-fehlende-transaktionen)
    - [2.2 Summary](#22-summary)
    - [2.3 Timeline](#23-timeline)
    - [2.4 Forecast](#24-forecast)
    - [2.5 Durchschnittliche Ausgaben und Einnahmen von Kategorien](#25-durchschnittliche-ausgaben-und-einnahmen-von-kategorien)
- [3. DTOs](#3-dtos)
    - [3.1 AnalysisPeriodDTO](#31-analysisperioddto)
    - [3.2 MissingTransactionResponseDTO](#32-missingtransactionresponsedto)
    - [3.3 SummaryResponseDTO](#33-summaryresponsedto)
    - [3.4 TimelineResponseDTO](#34-timelineresponsedto)
    - [3.5 CategoryAveragesResponseDTO](#35-categoryaveragesresponsedto)


## 1. Overview

## 2. Endpoints

### 2.1 Fehlende Transaktionen
**GET** `/api/analytics/missing-transactions`
Erkennt erwartete wiederkehrende Transaktionen auf Basis von Kategorieintervallen & Transaktionsdaten und erkennt fehlende Vorkommen.
Fehlt eine Transaktion für den heutigen Tag, wird diese noch nicht als fehlend zurückgegeben.

Beispiele:
- Kategorie "Miete", Intervall "MONTHLY" hat 4 Transaktionen, jeweils mit Datum "2026-XX-07". Aktuelles Datum ist "2026-XX-09" -> Transaktion fehlt.
- Kategorie "Abos", Intervall "WEEKLY" hat 2 Transaktionen, mit Daten "2026-01-01" und "2026-01-15" (beides Donnerstags). Backend erkennt, dass die Transaktion für "2026-01-08" fehlt -> Transaktion fehlt.

**Query-Params:**
- [AnalysisPeriodDTO](#31-analysisperioddto), für den Zeitraum, in dem nach fehlenden Transaktionen gesucht werden soll
- `categoryIds: number[]`, optional, um die Suche auf bestimmte Kategorien zu beschränken

**Responses:**
- http 200, data: List[[MissingTransactionResponseDTO](#32-missingtransactionresponsedto)]
- http 400, status: VALIDATION_ERROR, bei ungültigen Query-Parameter
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird


### 2.2 Summary
**GET** `/api/analytics/summary`
Fässt alle Transaktionen & ihre Kategorien, im gegebenen Zeitraum, in gesamte Ausgaben und Einnahmen zusammen. Berechnet außerdem den prozentualen Anteil der Ausgaben pro Kategorie.

**Query-Params:**

[AnalysisPeriodDTO](#31-analysisperioddto)

**Responses:**
- http 200, data: [SummaryResponseDTO](#33-summaryresponsedto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Query-Parameter
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird


### 2.3 Timeline
**GET** `/api/analytics/timeline`
Gibt eine Timeline basierend von allen Transaktionen im gegebenen Zeitraum zurück. Nützlich für die Visualisierung der finanziellen Entwicklung über einen Zeitraum, z.B. in einem Diagramm. Kategorien spielen hier keine Rolle.

**Query-Params:**
- [AnalysisPeriodDTO](#31-analysisperioddto)
- `grouping: "DAILY" | "WEEKLY" | "MONTHLY" | "QUARTERLY" | "YEARLY"`
    Bestimmt über welchen Zeitraum Ausgaben & Einnahmen gruppiert werden sollen
- `categoryIds: number[]`
    - Um zusätzlich auch die Entwicklung von bestimmten Kategorien zu erhalten. 
    - Die Einnahmen & Ausgaben dieser Kategorien werden auf das jeweilige Kategorie-Intervall gruppiert. Falls das Kategorie-Intervall `Once` ist, wird die Transaktion einfach auf das `grouping` Intervall gruppiert.

**Responses:**
- http 200, data: [TimelineResponseDTO](#34-timelineresponsedto)
- http 400, status: VALIDATION_ERROR, bei ungültigen Query-Parameter
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird


### 2.4 Forecast
**GET** `/api/analytics/forecast`
Rechnet die Ausgaben & Einnahmen in der gegebenen Periode zusammen, 
indem die average-expenses & incomes aller Kategorien berechnet werden, und in die Zukunft projiziert werden.
Dadurch wird eine Timeline erstellt, beginnend mit heute, bis zum angegebenen `until`.

**Query-Params:**
- [AnalysisPeriodDTO](#31-analysisperioddto)
- `grouping: "DAILY" | "WEEKLY" | "MONTHLY" | "QUARTERLY" | "YEARLY"`
    Bestimmt über welchen Zeitraum Ausgaben & Einnahmen gruppiert werden sollen
- `until: "END_OF_WEEK" | "END_OF_MONTH" | "END_OF_QUARTER" | "END_OF_YEAR"`
    Bis zu wann die Ausgaben & Einnahmen prognostiziert werden sollen

**Responses:**
- http 200, data: [TimelineResponseDTO](#34-timelineresponsedto) (Die Timeline enthält hier keine Kategorie-Perioden)
- http 400, status: VALIDATION_ERROR, bei ungültigen Query-Parameter.
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird


### 2.5 Durchschnittliche Ausgaben und Einnahmen von Kategorien
**GET** `/api/analytics/averages`
Gibt die durchschnittlichen Ausgaben & Einnahmen je Kategorie, normalisiert auf ein angegebenes Intervall, zurück.
Gibt außerdem den Anteil der Kategorie an den gesamten normalisierten Einnahmen / Ausgaben zurück.

**Query-Params:**
- [AnalysisPeriodDTO](#31-analysisperioddto)
- `interval: "MONTHLY" | "WEEKLY" | "QUARTERLY" | "YEARLY"`
    - Bestimmt auf welchen Zeitraum die Ausgaben normalisiert werden
    - Optional bei fixierten Kategorien, für diese wird dann automatisch das Intervall der Kategorie verwendet. Wird `interval` trotzdem angegeben, werden die Ausgaben auf dieses Intervall normalisiert, auch bei fixierten Kategorien.
- `categoryIds: number[]`, optional, um die Suche auf bestimmte Kategorien zu beschränken

**Responses:**
- http 200, data: List[[AveragesResponseDTO](#35-categoryaveragesresponsedto)]
- http 400, status: VALIDATION_ERROR, bei ungültigen Query-Parameter
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird


## 3. DTOs

### 3.1 AnalysisPeriodDTO
**GET** [/api/analytics/missing-transactions](#21-fehlende-transaktionen)

**GET** [/api/analytics/summary](#22-summary)

**GET** [/api/analytics/timeline](#23-timeline)

**GET** [/api/analytics/forecast](#24-forecast)

**GET** [/api/analytics/averages](#25-durchschnittliche-ausgaben-und-einnahmen-von-kategorien)
```json
{
    "range": "CUSTOM",
    "from": "YYYY-MM-DD",
    "to": "YYYY-MM-DD"
}
```
**Validierung**
- `range`
    - required
    - in ("CUSTOM", LAST_30_DAYS, LAST_90_DAYS, THIS_MONTH, THIS_YEAR)
- `from`
    - optional, required wenn `range="CUSTOM"`
    - DateOnly format
- `to`
    - optional, required wenn `range="CUSTOM"`
    - DateOnly format

### 3.2 MissingTransactionResponseDTO
**GET** [/api/analytics/missing-transactions](#21-fehlende-transaktionen)
```json
{
    "category": CategoryResponseDTO,
    "missingDates": [ "YYYY-MM-DD" ]
}
```

### 3.3 SummaryResponseDTO
**GET** [/api/analytics/summary](#22-summary)
```json
{
    "income": 2149.21,
    "expenses": 1421.59,
    "balance": 2149.21 - 1421.59,
    "categoryBreakdown": [
        {
            "category": CategoryResponseDTO,
            "income": 0.00,
            "expenses": 500.00,
            "net": 500.00,
            "percentage": 35.15
        }
    ]
}
```

### 3.4 TimelineResponseDTO
**GET** [/api/analytics/timeline](#23-timeline)

**GET** [/api/analytics/forecast](#24-forecast)
```json
{
    "grouping": "MONTHLY",
    "start": "YYYY-MM-DD",
    "end": "YYYY-MM-DD",
    "periods": [
        {
            "from": "YYYY-MM-DD",
            "to": "YYYY-MM-DD",
            "income": 2149.21,
            "expenses": 1421.59,
            "net": 2149.21 - 1421.59,
            "startBalance": 45000.00,
            "endBalance": 45000.00 + 2149.21 - 1421.59
        }
    ],
    "categoryPeriods": [
        {
            "category": CategoryResponseDTO,
            "periods": [
                {
                    "from": "YYYY-MM-DD",
                    "to": "YYYY-MM-DD",
                    "income": 2149.21,
                    "expenses": 1421.59,
                    "net": 2149.21 - 1421.59
                }
            ]
        }
    ]
}
```

### 3.5 CategoryAveragesResponseDTO
**GET** [/api/analytics/averages](#25-durchschnittliche-ausgaben-und-einnahmen-von-kategorien)
```json
{
    "category": CategoryResponseDTO,
    "averageIncome": 0.00,
    "averageExpenses": 500.00,
    "averageNet": 35.15,

    "incomeShare": 0.00,
    "expensesShare": 35.15,
    "netShare": 35.15
}
```