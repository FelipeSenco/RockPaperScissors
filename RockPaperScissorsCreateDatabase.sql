CREATE DATABASE RockPaperScissorsDatabase
GO

USE RockPaperScissorsDatabase
GO

CREATE TABLE Games(
GameID INT PRIMARY KEY IDENTITY(1,1),
PlayerName NVARCHAR(MAX),
PlayerTurnWins INT,
ComputerTurnWins INT,
GameResult NVARCHAR(MAX),
GameEndTime DATETIME
)
GO

CREATE TABLE Turns(
TurnID INT PRIMARY KEY IDENTITY(1,1),
PlayerName NVARCHAR(MAX),
PlayerChoice NVARCHAR(MAX),
ComputerChoice NVARCHAR(MAX),
TurnResult NVARCHAR(MAX),
TurnEndTime DATETIME,
GameID INT 
)
GO
