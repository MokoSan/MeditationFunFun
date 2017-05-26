module MeditationFunFun.Api.Controller.User

    open MeditationFunFun.Api.Common

    open System.Collections.Concurrent

    type User = {
        Id    : int
        Name  : string
        Email : string
    }

    let private userStorage = ConcurrentDictionary<int, User>() 

    let getAllUsers() = 
        userStorage.Values |> Seq.map(fun p -> p)

    let createUser user =
        let id = userStorage.Values.Count + 1
        let newUser = {
            Id    = id
            Name  = user.Name 
            Email = user.Email 
        }

        userStorage.GetOrAdd( id, user ) |> ignore
        newUser

    let userWebPart = getWebPartFromRestResource "user" {
        GetAll = getAllUsers
        Create = createUser
    }