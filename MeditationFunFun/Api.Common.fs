// Base functionality obtained from: http://blog.tamizhvendan.in/blog/2015/06/11/building-rest-api-in-fsharp-using-suave/

module MeditationFunFun.Api.Common 

    open System.IO
    open System.Text

    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization

    open Suave
    open Suave.RequestErrors
    open Suave.Http
    open Suave.Filters
    open Suave.Operators
    open Suave.Successful
    open Suave.Writers

    [<AutoOpen>]
    type RestResource<'TController> = {
        GetAll      : unit                -> 'TController seq
        //GetById     : int                 -> 'TController option 
        //IsExists    : int                 -> bool
        Create      : 'TController        -> 'TController
        //Update      : 'TController        -> 'TController option 
        //UpdateById  : int -> 'TController -> 'TController option
        //Delete      : int                 -> unit
    }


    let apiVersion    = "1" // TODO: Change this to read from a config
    let apiBaseString =  sprintf "/api/v%s/" apiVersion 

    let Jsonize ( objectToSerialize : obj ) : WebPart = 
        let jsonSerializerSettings = JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject( objectToSerialize, jsonSerializerSettings )
        |> OK
        >=> setMimeType "application/json; charset=utf-8"

    let UnJsonize<'a> ( json : string ) : 'a =
        JsonConvert.DeserializeObject< 'a >( json ) 

    let getResourceFromRequest<'TController>( request : HttpRequest ) = 
        let getString ( raw : byte[] ) : string = 
            Encoding.UTF8.GetString( raw )
        request.rawForm |> getString |> UnJsonize<'TController>

    let getWebPartFromRestResource ( resourceName : string ) ( resource : RestResource<'TController> ) : WebPart = 
        let fullResourcePath = Path.Combine ( apiBaseString, resourceName ) + "/" 
        let getAll = warbler ( fun _ -> resource.GetAll() |> Jsonize )

        path fullResourcePath >=> choose [
            GET  >=> getAll
            POST >=> request ( getResourceFromRequest >> resource.Create >> Jsonize ) 
        ]
