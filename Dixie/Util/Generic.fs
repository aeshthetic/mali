module Dixie.Util.Generic
open System
open System.Reflection
open Dixie.Util.Attributes
open Dixie.Util.Types

// val typeName: Type -> string
// Generates a human-readable name of a Type object
// t: the type to generate a name for
let rec typeName (t: System.Type) =
    let name = (t.Name.Split '`').[0]

    let typeParameters =
        if t.IsGenericType then
            t.GetGenericArguments() |> Array.map typeName
        else [||]

    let parameterString =
        match typeParameters.Length with
        | 0 -> ""
        | 1 -> typeParameters.[0]
        | _ -> sprintf "(%s) " (typeParameters |> String.concat ",")

    match parameterString with
    | "" -> name
    | _ -> sprintf "%s %s" parameterString name

// val isList<'Record>: string -> bool
// Checks whether a particular member of a record type is a list
// 'Record: the record type whose member is being checked
// memberName: the name of the member being checked
let isList<'Record> (memberName: string) =
    let typeOfMember = typeof<'Record>.GetProperty(memberName).PropertyType in
    let typeInfo = typeOfMember.GetGenericTypeDefinition() in
    typeInfo = typedefof<list<_>>

// val listPropType: PropertyInfo -> Type
// Returns the type of a list property
// prop: the list property (e.g. 'T list) whose type is being returned
let listPropType (prop: PropertyInfo) =
    prop
        .PropertyType
        .GetGenericArguments()
        |> Array.head

// val mapType: Type -> Table list
// Generates a list of Tables from a record type
// Note: currently dysfunctional. This should essentially "map" a record type to
// a relational table schema that can be used to generate real RDB tables. This is
// currently a goal that has yet to be reached.
// t: the record type to map
let rec mapType (t: System.Type) =
    let props = t.GetProperties()

    let oneToManys = 
        props
        |> Array.filter hasAttribute<OneToManyAttribute>
        |> Array.toList

    let types =
        props
        |> removeAttribute<OneToManyAttribute>
        |> Array.map (fun it -> it.PropertyType |> typeName )

    let names =
        props
        |> removeAttribute<OneToManyAttribute>
        |> Array.map (fun it -> it.Name)

    let tables = [{name = t.Name; schema = Map.ofArray <| Array.zip names types}]

    // This is a result of the nature of One to Many relationships
    // in that they require a table containing the "many" objects
    // to have a column referencing the "one" object. Therefore
    // a table must be created for each type of OTM list
    let mapOneToMany = listPropType >> mapType

    match oneToManys with
    | [] -> tables
    | [x] -> (mapOneToMany x) @ tables
    | hd :: tl ->
        tl
        |> List.map mapOneToMany
        |> List.reduce (@)
        |> ((@) <| mapOneToMany hd)