module MeditationFunFun.View

    open Suave
    open Suave.Filters
    open Suave.Operators

    let view = choose [
        GET >=> path "/" >=> Files.file "Web/index.html" 
    ]