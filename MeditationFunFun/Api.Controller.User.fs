module MeditationFunFun.Api.Controller.User

    open MeditationFunFun.Api.Common

    open System.Collections.Generic

    // User Data Model
    type User = {
        Id    : int
        Name  : string
        Email : string
    }

    let private userStorage = Dictionary<int, User>() 

    // Corresponding User Control Functions 
    let getAllUsers() = 
        userStorage.Values |> Seq.map(fun p -> p)

    let createUser ( user : User ) =
        let id = userStorage.Values.Count + 1
        let newUser = {
            Id    = id
            Name  = user.Name 
            Email = user.Email 
        }

        userStorage.Add( id, newUser ) |> ignore
        newUser

    let updateUserWithId ( userId : int ) ( userToUpdate : User ) : User option = 
        if userStorage.ContainsKey( userId ) then
            let updatedUser = {
                Id    = userToUpdate.Id
                Name  = userToUpdate.Name
                Email = userToUpdate.Email 
            }

            userStorage.[ updatedUser.Id ] <- updatedUser 
            updatedUser |> Some
        else
            None

    let updateUser ( userToUpdate : User ) : User option =
        updateUserWithId userToUpdate.Id userToUpdate 
        
    // Combined web part
    let userWebPart = getWebPartFromRestResource "user" {
        GetAll = getAllUsers
        Create = createUser
        Update = updateUser
    }