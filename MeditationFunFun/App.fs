module MeditationFunFun.App

    open Suave
    open Suave.Operators
    open Suave.Filters 
    
    open Api.Controller.User
    open Api.Controller.Journal

    let api = choose [ userWebPart; journalWebPart ]