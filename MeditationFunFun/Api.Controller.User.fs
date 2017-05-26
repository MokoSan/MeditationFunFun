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

    let getUserById id =
        if userStorage.ContainsKey( id ) then
            Some userStorage.[id]
        else
            None

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

    let deleteUser userId = 
        userStorage.Remove( userId ) |> ignore

    let isUserExists = userStorage.ContainsKey
        
    // Combined web part
    let userWebPart = getWebPartFromRestResource "user" {
        GetAll     = getAllUsers
        GetById    = getUserById
        Create     = createUser
        Update     = updateUser
        UpdateById = updateUserWithId
        Delete     = deleteUser 
        IsExists   = isUserExists 
    }