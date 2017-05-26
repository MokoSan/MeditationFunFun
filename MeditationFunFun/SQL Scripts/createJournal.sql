GO
USE MeditationFunFun
GO 
CREATE TABLE Journals (
	Id int IDENTITY(1,1) NOT NULL, 
	UserId int NOT NULL,
	Title nvarchar(500) NULL,
	DateOfMeditation Date NOT NULL,
	Content nvarchar(1200) NULL
	Primary Key (Id)
	Foreign Key (UserId) REFERENCES Users(Id)
)


