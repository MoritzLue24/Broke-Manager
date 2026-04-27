# Architecture

## 1. Technologie-Stack
* ASP.NET Web Api
* Entity Framework Core
* Microsoft SQL Server. Falls was nicht klappt auch SQLite
* JWT-Basierte Authentifizierung

## 2. Datenbanksystem

### 2.1 Beziehungen
* Jede Tabelle hat ein pk / Id, autoincrement
* Beziehungen über fk's
* Ein User hat viele Transactions (1:n)
* Ein User hat viele Categories (1:n)
* Eine Category hat viele Transations (1:n)
* Jede Transaction gehört zu genau einer Category (n:1)

### 2.2 Löschverhalten
* User löschen -> löscht alle Transactions & Categories -> löscht alle Keywords.
* Category löschen -> löscht alle Keywords & Transactions.
* Beim sonstigen Löschen passiert nichts.

### 2.3 Tables
**Users**
```SQL
CREATE TABLE Users (
	Id	            int IDENTITY(1,1),
	Email	        nvarchar(255) NOT NULL,
	PasswordHash	nvarchar(255) NOT NULL,
    Role            nvarchar(20) NOT NULL,
	CreatedAt		datetime2(3) NOT NULL,

	CONSTRAINT PK_Users PRIMARY KEY (Id),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT CK_Users_Role CHECK (Role IN ('ADMIN', 'USER')),
	CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME() FOR CreatedAt
);
```

**Transactions**
```SQL
CREATE TABLE Transactions (
    Id              int IDENTITY(1,1),
    UserId          int NOT NULL,
    CategoryId      int NOT NULL,
    Date            date NOT NULL,
    Amount          decimal(12,2) NOT NULL,
    Title           nvarchar(500) NOT NULL,
    CounterParty    nvarchar(255),
    CategorySource  nvarchar(20) NOT NULL,

    CONSTRAINT PK_Transactions PRIMARY KEY (Id),
    CONSTRAINT FK_Transactions_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_Transactions_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(Id)
        ON DELETE CASCADE,
    CONSTRAINT CK_Transactions_CategorySource
        CHECK (CategorySource IN ('Manual', 'Auto'))
)
```

**Categories**
```SQL
CREATE TABLE Categories (
    Id          int IDENTITY(1,1),
    UserId      int NOT NULL,
    Name        nvarchar(255) NOT NULL,
    Interval    nvarchar(20) NOT NULL,
    IsDefault   bit NOT NULL,
    CreatedAt   datetime2(3) NOT NULL,

    CONSTRAINT PK_Categories PRIMARY KEY (Id),
    CONSTRAINT UQ_Categories_UserId_Name UNIQUE (UserId, Name),
    CONSTRAINT CK_Categories_Interval 
        CHECK (Interval IN ('Once', 'Weekly', 'Monthly', 'Quarterly', 'Yearly')),
    CONSTRAINT DF_Categories_CreatedAt DEFAULT SYSUTCDATETIME() FOR CreatedAt,
    CONSTRAINT FK_Categories_Users 
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE
)
```

**Keywords**
```SQL
CREATE TABLE Keywords (
    Id          int IDENTITY(1,1),
    CategoryId  int NOT NULL,
    Value       nvarchar(500) NOT NULL,
    CreatedAt   datetime2(3) NOT NULL,

    CONSTRAINT PK_Keywords PRIMARY KEY (Id),
    CONSTRAINT UQ_Keywords_CategoryId_Value UNIQUE (CategoryId, Value),
    CONSTRAINT DF_Keywords_CreatedAt DEFAULT SYSUTCDATETIME() FOR CreatedAt
    CONSTRAINT FK_Keywords_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(Id)
        ON DELETE CASCADE
)
```

