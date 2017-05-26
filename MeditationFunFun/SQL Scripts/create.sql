IF NOT EXISTS ( SELECT name from master.sys.databases WHERE name = N'MeditationFunFun' )
BEGIN
CREATE DATABASE MeditationFunFun
END

GO
USE MeditationFunFun
GO 
CREATE TABLE Users (
	Id int NOT NULL, 
	Name nvarchar(120) NULL,
	Email nvarchar(200) NULL
	Primary Key (Id)
)
