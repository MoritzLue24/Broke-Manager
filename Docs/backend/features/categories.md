# Categories
`/api/categories` handelt die Verwaltung von Kategorien, die für Transaktionen verwendet werden können. Benutzer können eigene Kategorien erstellen, bearbeiten und löschen.

Hier werden Categories & Keywords als ein feature gesehen.

Der Name einer Kategorie ist für einen User einzigartig.


- [1. Endpoints](#1-endpoints)
    - [1.1 Alle Kategorien abrufen](#11-alle-kategorien-abrufen)
    - [1.2 Spezifische Kategorie abrufen](#12-spezifische-kategorie-abrufen)
    - [1.3 Neue Kategorie erstellen](#13-neue-kategorie-erstellen)
    - [1.4 Kategorie aktualisieren](#14-eine-kategorie-aktualisieren)
    - [1.5 Spezifische Kategorie löschen](#15-spezifische-kategorie-löschen)
    - [1.6 Alle Kategorien des Benutzers löschen](#16-alle-kategorien-des-benutzers-löschen)
- [2. Keywords](#2-keywords)
    - [2.1 Neues Keyword erstellen](#21-neues-keyword-erstellen)
    - [2.2 Keyword updaten](#22-keyword-updaten)
    - [2.3 Keyword löschen](#23-keyword-löschen)
- [3. DTOs](#3-dtos)
    - [3.1 ResponseDTO](#31-responsedto)
    - [3.2 CreateDTO](#32-createdto)
    - [3.3 UpdateDTO](#33-updatedto)
    - [3.4 KeywordResponseDTO](#34-keywordresponsedto)
    - [3.5 KeywordCreateDTO](#35-keywordcreatedto)
    - [3.6 KeywordUpdateDTO](#36-keywordupdatedto)



## 1. Endpoints

### 1.1 Alle Kategorien abrufen

**GET** `/api/categories`
Gibt eine Liste aller Kategorien des aktuell eingeloggten Benutzers zurück.

**Responses:**
- http 200, data: List[[ResponseDTO](#31-responsedto)]
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird

### 1.2 Spezifische Kategorie abrufen

**GET** `/api/categories/{id}`
Gibt eine bestimmte Kategorie zurück

**Responses:**
- http 200, data: [ResponseDTO](#31-responsedto)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die Kategorie nicht gefunden wurde

### 1.3 Neue Kategorie erstellen

**POST** `/api/categories`
Erstellt eine neue Kategorie

**Request-Body:**

[CreateDTO](#23-createdto)

**Responses:**
- http 201, data: [ResponseDTO](#31-responsedto)
- http 400, status: VALIDATION_ERROR, wenn das Request-Format fehlerhaft ist
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 409, status: ALREADY_EXISTS_ERROR, falls eine Kategorie mit diesen Namen bereits existiert

### 1.4 Eine Kategorie aktualisieren

**PUT** `/api/categories/{id}`
Aktualisiert eine kategorie

**Request-Body**

[UpdateDTO](#33-updatedto)

**Responses:**
- http 200, data: [ResponseDTO](#31-responsedto)
- http 400, status: VALIDATION_ERROR, wenn das Request-Format fehlerhaft ist
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die Kategorie nicht existiert
- http 409, status: ALREADY_EXISTS_ERROR, falls eine Kategorie mit diesen Namen bereits existiert

### 1.5 Spezifische Kategorie löschen

**DELETE** `/api/categories/{id}`
Löscht eine Kategorie und all ihre keywords.

**Responses:**
- http 204, wenn die Kategorie erfolgreich gelöscht wurde
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die Kategorie nicht existiert

### 1.6 Alle Kategorien des Benutzers löschen

**DELETE** `/api/categories`
Löscht alle Kategorien des Users und die Keywords.

**Responses:**
- http 204, wenn die Kategorien erfolgreich gelöscht wurden
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird

## 2. Keywords

### 2.1 Neues Keyword erstellen

**POST** `/api/categories/{categoryId}/keywords`
Fügt ein neues Keyword einer Kategorie hinzu.

**Request-Body:**

[KeywordCreateDTO](#35-keywordcreatedto)

**Responses:**
- http 201, data: [KeywordResponseDTO](#34-keywordresponsedto)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, wenn die Kategorie nicht gefunden wurde.
- http 409, status: ALREADY_EXISTS_ERROR, wenn das Keyword bereits in der Kategorie existiert. 

### 2.2 Keyword updaten

**PUT** `/api/categories/{categoryId}/keywords/{keywordId}`
Aktualisiert ein Keyword.

**Request-Body:**

[KeywordUpdateDTO](#36-keywordupdatedto)

**Responses:**
- http 200, data: [KeywordResponseDTO](#34-keywordresponsedto)
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, das Keyword nicht gefunden wurde.
- http 409, status: ALREADY_EXISTS_ERROR, wenn das Keyword bereits in der Kategorie existiert. 

### 2.3 Keyword löschen

**DELETE** `/api/categories/{categoryId}/keywords/{keywordId}`
Löscht ein keyword

**Responses:**
- http 204, wenn das Keyword erfolgreich gelöscht wurde.
- http 401, status: UNAUTHORIZED_ERROR, wenn kein gültiges JWT-Token bereitgestellt wird
- http 404, status: NOT_FOUND_ERROR, das Keyword nicht gefunden wurde.


## 3. DTOs

### 3.1 ResponseDTO
**GET** [/api/categories](#11-alle-kategorien-abrufen)

**GET** [/api/categories/{id}](#12-spezifische-kategorie-abrufen)

**POST** [/api/categories](#13-neue-kategorie-erstellen)

**PUT** [/api/categories/{id}](#14-eine-kategorie-aktualisieren)

```json
{
    "id": 1,
    "name": "Essen",
    "interval": "Once",
    "isDefault": false,
    "keywords": [
        "Aldi"
    ]
}
```

### 3.2 CreateDTO
**POST** [/api/categories](#13-neue-kategorie-erstellen)

```json
{
    "name": "Essen",
    "interval": "Once",  // optional, default="Once"
    "keywords": [ "string" ]
}
```

### 3.3 UpdateDTO
**PUT** [/api/categories/{id}](#14-eine-kategorie-aktualisieren)

```json
{
    "name": "Essen", // optional
    "interval": "Once"  // optional
}
```

### 3.4 KeywordResponseDTO
**POST** [/api/categories/{categoryId}/keywords](#21-neues-keyword-erstellen)

**PUT** [/api/categories/{categoryId}/keywords/{keywordId}](#22-keyword-updaten)

```json
{
    "id": 1,
    "categoryId": 1,
    "value": "Rewe",
}
```

### 3.5 KeywordCreateDTO
**POST** [/api/categories/{categoryId}/keywords](#21-neues-keyword-erstellen)

```json
{
    "value": "Edeka"
}
```

### 3.6 KeywordUpdateDTO
**PUT** [/api/categories/{categoryId}/keywords/{keywordId}](#22-keyword-updaten)

```json
{
    "value": "Aldi" // optional
}
```