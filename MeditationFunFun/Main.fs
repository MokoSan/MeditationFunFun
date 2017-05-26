module MeditationFunFun.Main

open Suave 

open MeditationFunFun.App 
open View

let app = choose [ api; view ]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig app 
    0 