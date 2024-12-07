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