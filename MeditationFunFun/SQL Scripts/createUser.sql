GO
USE MeditationFunFun
GO 
CREATE TABLE Users (
	Id int IDENTITY(1,1) NOT NULL, 
	Name nvarchar(120) NULL,
	Email nvarchar(200) NULL
	Primary Key (Id)
)

