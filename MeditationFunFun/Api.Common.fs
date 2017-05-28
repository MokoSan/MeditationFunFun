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
        Name        : string
        GetAll      : unit                -> 'TController seq
        GetById     : int                 -> 'TController option 
        Create      : 'TController        -> 'TController
        Update      : 'TController        -> 'TController option 
        UpdateById  : int -> 'TController -> 'TController option
        Delete      : int                 -> unit
        IsExists    : int                 -> bool
    }

    let apiVersion    = "1" // TODO: Change this to read from a config
    let apiBaseString =  sprintf "/api/v%s/" apiVersion 

    let setCORSHeaders webPartToCombine = 
        setHeader  "Access-Control-Allow-Origin" "*" >=> 
        setHeader "Access-Control-Allow-Headers" "content-type" >=> webPartToCombine

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

    let getWebPartFromRestResource ( resource : RestResource<'TController> ) : WebPart = 

        let fullResourcePath = Path.Combine ( apiBaseString, resource.Name ) 
        let getAll           = warbler ( fun _ -> resource.GetAll() |> Jsonize )
        let badRequest       = BAD_REQUEST "Resource Not Found"

        let resourceIdPath =
            PrintfFormat<(int -> string),unit,string,string,int>(fullResourcePath + "/%d")

        let handleResource requestError = function 
            | Some r -> r |> Jsonize
            | None   -> requestError

        let getResourceById =
            resource.GetById >> handleResource ( NOT_FOUND "Resource Not Found" )

        let updateResourceById id = 
            request ( getResourceFromRequest >> ( resource.UpdateById id ) >> handleResource badRequest )

        let deleteResourceById id = 
            resource.Delete id
            NO_CONTENT

        let isResourceExists id =
            if resource.IsExists id then OK "" else NOT_FOUND ""

        choose [
            path fullResourcePath >=> choose [
                setCORSHeaders GET  >=> getAll
                setCORSHeaders POST >=> request ( getResourceFromRequest >> resource.Create >> Jsonize ) 
                setCORSHeaders PUT  >=> request ( getResourceFromRequest >> resource.Update >> handleResource badRequest )
            ]

            setCORSHeaders DELETE >=> pathScan resourceIdPath deleteResourceById 
            setCORSHeaders GET    >=> pathScan resourceIdPath getResourceById
            setCORSHeaders PUT    >=> pathScan resourceIdPath updateResourceById
            setCORSHeaders HEAD   >=> pathScan resourceIdPath isResourceExists
        ]
