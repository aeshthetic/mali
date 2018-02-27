open System
open Dixie.Util.Attributes
open Dixie.Util.Generic
open Dixie.Util.Types

// Users
// | id | name | email
type User = 
    {id: int;
     name: string;
     email: string;
     [<OneToMany>]
     posts: Post list;}

// Posts
// | id | content | timeStamp | ref user
and Post =
    {id: int;
     content: string;
     timeStamp: DateTime;
     [<Ref(Parent="posts")>]
     poster: User;
     liked: User list}

// Liked
// | id | ref user | ref post

// val formatTable: Table -> string
// Formats a table for pretty printing
// table: the table to be formatted
let formatTable (table: Table) =
    let schema =
        table.schema
        |> Map.toSeq
        |> Seq.map (fun (cName, cType) -> sprintf "%s: %s" cName cType)
        |> String.concat " | "
    sprintf "%s:\n%s" table.name schema

[<EntryPoint>]
// val main: string [] -> int
// prints out a formatted table for the User record type defined above
let main _ =
    mapType typeof<User>
    |> List.map formatTable
    |> List.iter (printfn "%s")

    Console.ReadLine()
    |> ignore
    0