Create Database IDSDB;
GO

USE IDSDB;
GO

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(256) NOT NULL, -- hash-ul parolei
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Analyst', 'Admin', 'User')),
    Email NVARCHAR(100), -- pentru notificări
    IsActive BIT NOT NULL DEFAULT 1 -- pentru dezactivarea conturilor
);

CREATE TABLE NetworkTraffic (
    TrafficID INT PRIMARY KEY IDENTITY(1,1),
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    SourceIP NVARCHAR(50) NOT NULL,
    DestinationIP NVARCHAR(50) NOT NULL,
    Protocol NVARCHAR(20) NOT NULL, -- ex. TCP, UDP
    PacketSize INT NOT NULL,
    IsSuspicious BIT NOT NULL DEFAULT 0 -- marcare pentru trafic suspect
);

CREATE TABLE Alerts (
    AlertID INT PRIMARY KEY IDENTITY(1,1),
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    TrafficID INT FOREIGN KEY REFERENCES NetworkTraffic(TrafficID),
    AlertType NVARCHAR(50) NOT NULL, -- ex. DDoS, Malware, Port Scan
    Message NVARCHAR(255),
    Resolved BIT NOT NULL DEFAULT 0 -- dacă alerta a fost rezolvată
);

CREATE TABLE BlockedIPs (
    BlockID INT PRIMARY KEY IDENTITY(1,1),
    IPAddress NVARCHAR(50) NOT NULL,
    BlockedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UnblockedAt DATETIME NULL, -- NULL dacă IP-ul încă este blocat
    Reason NVARCHAR(255)
);

CREATE TABLE Reports (
    ReportId INT IDENTITY(1,1) PRIMARY KEY,       -- ID unic auto-generat
    Title NVARCHAR(200) NOT NULL,                -- Titlul raportului, obligatoriu
    Description NVARCHAR(MAX) NULL,              -- Descriere opțională
	Category NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),      -- Data creării, implicit data curentă
    CreatedBy INT NOT NULL,                      -- ID-ul utilizatorului, foreign key
    Status NVARCHAR(50) DEFAULT 'Open',          -- Starea raportului, implicit 'Open'
    Priority INT DEFAULT 3,                      -- Nivelul de prioritate (implicit 3)
    CONSTRAINT FK_Reports_Users FOREIGN KEY (CreatedBy) REFERENCES Users(UserId) ON DELETE CASCADE
);

INSERT INTO Users (Username, Password, Role, Email, IsActive)
VALUES 
('admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Admin', 'admin@example.com', 1),
('analyst','f44ceb062e35dfeea6ed7f8524d53bb0bff19f553e25cae7ef4850e4185ccbba', 'Analyst', 'analyst@example.com', 1),
('user','04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb', 'User', 'user@example.com', 1);