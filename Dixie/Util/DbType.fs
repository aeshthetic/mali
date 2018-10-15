module Dixie.Util.DbType
open System
open Dixie.Util.Types
open Dixie.Util

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
    | "Time" -> Time
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

// val typeOf: DbType -> Type
// dbType: the DbType being converted to a System.Type
/// Converts a DbType value to a System.Type
let typeOf dbType =
    dbType
    |> stringOf
    |> sprintf ("System.%s") 
    |> Type.GetType

// val postgres: DbType -> string
// _: the DbType being converted to a PostgreSQL data-type
/// Converts a DbType value into a PostgreSQL data-type in string form
let postgres = function
    | Int -> "INT"
    | Float -> "REAL"
    | String -> "TEXT"
    | Bool -> "BOOL"
    | Char -> "CHAR(1)"
    | Date -> "TIMESTAMP"
    | Time -> "TIME"
    | Ref (t, _) -> sprintf "INT REFERENCES %s(id)" t.Name

// val ofProperty: System.Reflection.PropertyInfo -> DbType
// prop: the property of which a DbType is being generated
/// Returns a DbType corresponding to a property
let ofProperty prop =
    match (Attributes.tryFindAttribute<RefAttribute> prop) with
    | Some attr -> Ref (prop.PropertyType, prop.PropertyType.GetProperty(attr.Parent))
    | None -> fromName (prop.PropertyType.Name)