module MeditationFunFun.Database

    open FSharp.Data.Sql

    // TODO: Add config for the connection string
    [<Literal>]
    let connectionString = @"Data Source=.\SQLExpress;Initial Catalog=MeditationFunFun;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    type sql             = SqlDataProvider< ConnectionString = connectionString > 
    let databaseContext  = sql.GetDataContext() 
    let dbo              = databaseContext.Dbo