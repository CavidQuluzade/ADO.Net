CREATE DATABASE CountryAppDB
USE CountryAppDB
CREATE TABLE Countries(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL UNIQUE,
	Area DECIMAL(20,2) NOT NULL
)
CREATE TABLE Cities(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	Area DECIMAL(20,2) NOT NULL,
	CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)
DELETE Countries WHERE Id = 2
SELECT * FROM Countries
DELETE Cities WHERE CountryId = 1
SELECT * FROM Cities
SELECT SUM(Cities.Area) FROM Cities JOIN Countries ON  Countries.Id = 1
SELECT COUNT(Cities.CountryId) FROM Cities JOIN  Countries ON CountryId = Countries.Id
WHERE CountryId = 4
