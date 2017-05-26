module Api.Common 

    type RestResource<'TController> = {
        GetAll      : unit                -> 'TController seq
        GetById     : int                 -> 'TController option 
        IsExists    : int                 -> bool
        Create      : 'TController        -> 'TController
        Update      : 'TController        -> 'TController option 
        UpdateById  : int -> 'TController -> 'TController option
        Delete      : int                 -> unit
    }