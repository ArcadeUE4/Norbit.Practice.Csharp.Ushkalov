USE GameStoreDB;
GO

-- Выборка с фильтрацией и сортировкой
SELECT Title, Price, ReleaseDate 
FROM Games 
WHERE Price > 500 AND ReleaseDate > '2018-01-01'
ORDER BY Price DESC;
GO

-- Обнуляем баланс забаненным игрокам
UPDATE Players SET Balance = 0 WHERE IsBanned = 1;
GO

-- Удаляем жанр 'Trash', если он вдруг есть (удаление)
DELETE FROM Genres WHERE GenreName = 'Trash';
GO

-- Считаем количество игр у каждой студии
SELECT StudioID, COUNT(*) AS TotalGames
FROM Games
GROUP BY StudioID;
GO

-- Выборка название игры, студии и её жанры (Пересечение)
SELECT g.Title AS Game, s.Name AS Studio, gn.GenreName AS Genre
FROM Games g
INNER JOIN Studios s ON g.StudioID = s.StudioID
INNER JOIN GameGenres gg ON g.GameID = gg.GameID
INNER JOIN Genres gn ON gg.GenreID = gn.GenreID;
GO

-- Левое соединение. просто выведем игроков и их регистрацию
SELECT Nickname, RegistrationDate, IsBanned
FROM Players
WHERE IsBanned = 0;
GO