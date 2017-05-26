module MeditationFunFun.Api.Controller.Journal 

    open MeditationFunFun.Api.Common

    open System

    type Journal = {
        Id          : int
        UserId      : int 
        Title       : string
        CreatedTime : DateTime 
        UpdatedTime : DateTime
        Content     : string
    }