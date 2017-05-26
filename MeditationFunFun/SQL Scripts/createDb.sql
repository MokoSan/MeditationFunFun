IF NOT EXISTS ( SELECT name from master.sys.databases WHERE name = N'MeditationFunFun' )
BEGIN
CREATE DATABASE MeditationFunFun
END