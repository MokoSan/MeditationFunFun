module MeditationFunFun.Api.Controller.User

    open MeditationFunFun.Api.Common
    open MeditationFunFun.Database

    open System.Collections.Generic

    // User Data Model
    type User = {
        Name  : string
        Email : string
    }

    type UserDbo = sql.dataContext.``dbo.UsersEntity`` 

    // Helpers 
    let getUserFromUserDbo ( user : UserDbo ) : User = 
        { Name = user.Name; Email = user.Email }

    let createUserDboFromUser ( user : User ) : UserDbo = 
        let newUserDbo = dbo.Users.Create()
        newUserDbo.Name  <- user.Name
        newUserDbo.Email <- user.Email
        newUserDbo

    let getUserDboFromId ( userId : int ) : UserDbo option =
        let user = query {
            for user in dbo.Users do
            where ( user.Id = userId ) 
            select user 
            headOrDefault
        } 

        match user with
        | null -> None
        | u    -> Some ( u )
        
    // Corresponding User Control Functions 
    // GET 
    let getAllUsers() : seq< User > = 
        dbo.Users |> Seq.map getUserFromUserDbo

    let getUserById ( userId : int ) : User option =
        let userById = getUserDboFromId userId  
        if userById.IsNone then None
        else getUserFromUserDbo userById.Value |> Some

    // POST
    let createUser ( user : User ) : User =
        let newUser = {
            Name  = user.Name 
            Email = user.Email 
        }

        let newUserDbo = createUserDboFromUser user
        databaseContext.SubmitUpdates()
        newUser

    // PUT 
    let updateUserWithId ( userId : int ) ( userToUpdate : User ) : User option = 
        let userById = getUserDboFromId userId  
        if userById.IsNone then None
        else 
            let u = userById.Value 
            u.Name  <- userToUpdate.Name
            u.Email <- userToUpdate.Email
            databaseContext.SubmitUpdates()
            Some userToUpdate
            
    let updateUser ( userToUpdate : User ) : User option =
        failwith "Not Implemented Construct"

    // DELETE
    let deleteUser ( userId : int ) : unit = 
        let user = getUserDboFromId userId 
        match user with 
        | Some u ->
            u.Delete() 
            databaseContext.SubmitUpdates()
        | None   -> () 

    // HEAD
    let isUserExists ( userId : int ) = ( getUserDboFromId userId ).IsSome
        
    // Combined web part
    let userWebPart = getWebPartFromRestResource {
        Name       = "users"
        GetAll     = getAllUsers
        GetById    = getUserById
        Create     = createUser
        Update     = updateUser
        UpdateById = updateUserWithId
        Delete     = deleteUser 
        IsExists   = isUserExists 
    }