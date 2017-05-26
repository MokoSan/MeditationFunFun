module MeditationFunFun.Api.Controller.Journal 

    open MeditationFunFun.Database
    open MeditationFunFun.Api.Common

    open System

    type Journal = {
        Title            : string
        DateOfMeditation : DateTime 
        Content          : string
    }

    type JournalDbo = sql.dataContext.``dbo.JournalsEntity``

    // Helpers
    let getJournalFromJournalDbo ( journal : JournalDbo ) : Journal = 
       { Title = journal.Title; DateOfMeditation = journal.DateOfMeditation; Content = journal.Content } 

    let getJournalDboFromId ( journalId : int ) : JournalDbo option = 
        let journal = query {
            for j in dbo.Journals do
            where ( j.Id = journalId )
            select j 
            headOrDefault
        }

        match journal with
        | null -> None
        | j    -> Some j 

    let createJournalDboFromJournal ( journal : Journal ) : JournalDbo = 
        let newJournalDbo = dbo.Journals.Create()
        newJournalDbo.Title            <- journal.Title
        newJournalDbo.DateOfMeditation <- journal.DateOfMeditation
        newJournalDbo.Content          <- journal.Content
        newJournalDbo

    // GET
    let getAllJournals() : seq < Journal > =
        dbo.Journals |> Seq.map getJournalFromJournalDbo

    let getJournalById ( journalId : int ) : Journal option = 
        let journalDbo = getJournalDboFromId journalId
        if journalDbo.IsNone then None
        else Some ( getJournalFromJournalDbo journalDbo.Value )

    // POST
    let createNewJournal ( journal : Journal ) : Journal = 
        let newJournal = {
            Title            = journal.Title
            DateOfMeditation = journal.DateOfMeditation
            Content          = journal.Content
        }

        let newJournalDbo = createJournalDboFromJournal journal
        submitToDbAsync |> ignore
        newJournal 

    // PUT 
    let updateJournalWithId ( journalId : int ) ( journalToUpdate : Journal ) : Journal option = 
        let journalById = getJournalDboFromId journalId
        if journalById.IsNone then None
        else
            let j = journalById.Value
            j.Title            <- journalToUpdate.Title
            j.DateOfMeditation <- journalToUpdate.DateOfMeditation
            j.Content          <- journalToUpdate.Content
            submitToDbAsync |> ignore
            Some journalToUpdate

    let updateJournal ( journalToUpdate : Journal ) : Journal option = 
        failwith "Not implemented construct"

    // DELETE
    let deleteJournal ( journalId : int ) : unit = 
        let journal = getJournalDboFromId journalId
        match journal with
        | Some j ->
            j.Delete()
            submitToDbAsync |> ignore
        | None   -> ()

    // HEAD
    let isJournalExists ( journalId : int ) = ( getJournalDboFromId journalId ).IsSome

    let journalWebPart = getWebPartFromRestResource {
        Name       = "journal"
        GetAll     = getAllJournals 
        GetById    = getJournalById
        Create     = createNewJournal
        Update     = updateJournal 
        UpdateById = updateJournalWithId
        Delete     = deleteJournal 
        IsExists   = isJournalExists 
    }