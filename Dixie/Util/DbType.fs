module Dixie.Util.DbType
open Dixie.Util.Types

let fromName = function
    | "Int32" -> Int
    | "String" -> DbType.String
    | "DateTime" -> Date
    | "Boolean" -> Bool
    | "Char" -> DbType.Char
    | "Double" -> Float
    | _ -> Int

let stringOf dbType =
    ["Int32"; "String"; "DateTime"; "Boolean"; "Char"; "Double"]
    |> List.tryFind (fun it -> (fromName it) = dbType)
    |> (fun typeStr ->
        match typeStr with
        | Some str -> str
        | None -> "Int32")
    