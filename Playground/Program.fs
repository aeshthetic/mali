open System
open Dixie.Util.Attributes
open Dixie.Util.Generic
open Dixie.Util

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

[<EntryPoint>]
// val main: string [] -> int
// prints out a formatted table for the User record type defined above
let main _ =
    mapType typeof<User>
    |> List.map (Table.format)
    |> List.iter (printfn "%s")

    Console.ReadLine()
    |> ignore
    0