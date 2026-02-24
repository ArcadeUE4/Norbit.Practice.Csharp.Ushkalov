
Create DATABASE GameStoreDB;
GO

USE GameStoreDB;
GO
-- Создание таблицы "Студии"
CREATE TABLE Studios (
	studioID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Name NVARCHAR(100) NOT NULL,
	FoundationDate DATETIME2,
	AnnualRevenue DECIMAL (18,2),
	IsIndependent BIT DEFAULT 1
);
-- Создание таблицы "Игры"
CREATE TABLE Games (
	GameID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Title NVARCHAR(200) NOT NULL,
	ReleaseDate DATETIME2,
	Price DECIMAL (10, 2),
	IsMultiplayer BIT,
	StudioID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Studios(StudioID)
);
-- Создание табилцы "Жанры"
CREATE TABLE Genres (
	GenreID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	GenreName NVARCHAR(50) NOT NULL
);
-- Создание таблицы "Жанр игр"
CREATE TABLE GameGenres (
	GameID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Games(GameID),
	GenreID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Genres(GenreID),
	PRIMARY KEY (GameID, GenreID)
);
-- Создание таблицы "Игроки"
CREATE TABLE Players (
	PlayerID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	NickName NVARCHAR(50) NOT NULL,
	Balance DECIMAL(15,2) DEFAULT 0.00,
	RegistrationDate DATETIME2 DEFAULT GETDATE(),
    IsBanned BIT DEFAULT 0
);
-- Создание уникального индекса NickName
CREATE UNIQUE INDEX UIX_Player_Nickname ON Players(Nickname);