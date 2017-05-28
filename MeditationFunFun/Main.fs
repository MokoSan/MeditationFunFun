module MeditationFunFun.Main

open Suave 

open MeditationFunFun.App

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig app 
    0 