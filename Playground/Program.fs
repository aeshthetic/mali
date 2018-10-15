open System
open Mali.Util.Types
open Mali.Mapping
// Users
// | id | name | email
type Member = 
    {[<PrimaryKey>]
     id: int;
     name: string;
     [<Unique>]
     [<NonNullable>]
     email: string;
     [<OneToMany>]
     posts: Post list;}

// Posts
// | id | content | timeStamp | ref user
and Post =
    {[<PrimaryKey>]
     id: int;
     content: string;
     timeStamp: DateTime;
     [<Ref(Parent="posts")>]
     poster: Member;}

// Liked
// | id | ref user | ref post

[<EntryPoint>]
// val main: string [] -> int
// prints out a formatted table for the User record type defined above
let main _ =
    fromType typeof<Member>
    |> List.map (createTable)
    |> List.iter (printfn "%s")

    Console.ReadLine()
    |> ignore
    0