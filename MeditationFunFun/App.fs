module MeditationFunFun.App

    open Suave
    open Suave.Operators
    open Suave.Filters 
    open Suave.Writers
    
    open Api.Controller.User
    open Api.Controller.Journal

    let api = choose [ journalWebPart; userWebPart  ]