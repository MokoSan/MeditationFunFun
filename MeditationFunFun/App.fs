﻿module MeditationFunFun.App

    open Api.Controller.User

    open Suave
    open Suave.Operators
    open Suave.Filters 

    let api = choose [ 
        userWebPart
    ]