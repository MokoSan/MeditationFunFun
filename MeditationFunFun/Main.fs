module MeditationFunFun.Main

open Suave 

open MeditationFunFun.App

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig api 
    0 