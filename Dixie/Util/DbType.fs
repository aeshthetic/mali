module Dixie.Util.DbType
open Dixie.Util.Types

// val fromName: string -> DbType
// _: string to convert to DbType
/// Parses a type name to convert to a DbType. If the string is not a valid type,
/// it is assumed that the column is an integer ID.
/// Note: This does not work with references. References
/// are not intended to be represented as strings except for
/// display purposes
let fromName = function
    | "Int32" -> Int
    | "String" -> DbType.String
    | "DateTime" -> Date
    | "Boolean" -> Bool
    | "Char" -> DbType.Char
    | "Double" -> Float
    | _ -> Int

// val stringOf: DbType -> string
// dbType: DbType to convert to string
/// Generates a string name for a DbType
let stringOf dbType =
    (* Instead of creating a look-up match like in fromName,
    we can use a list of potential inputs to fromName to see
    if their outputs match the input dbType. We can do some
    extra checking if they don't, mainly to handle references. *)
    ["Int32"; "String"; "DateTime"; "Boolean"; "Char"; "Double"]
    |> List.tryFind (fun it -> (fromName it) = dbType)
    |> (fun typeStr ->
        match typeStr with
        | Some str -> str
        | None ->
            match dbType with
            | Ref (t, p) -> sprintf "Int32 %s.%s Ref" t.Name p.Name
            | _ -> "Int32")
    