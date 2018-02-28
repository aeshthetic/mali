module Dixie.Util.Generic
open System.Reflection
open Dixie.Util.Attributes
open Dixie.Util.Types

// val typeName: Type -> string
// t: the type to generate a name for
/// Generates a human-readable name of a Type object
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
// 'Record: the record type whose member is being checked
// memberName: the name of the member being checked
/// Checks whether a particular member of a record type is a list
let isList<'Record> (memberName: string) =
    let typeOfMember = typeof<'Record>.GetProperty(memberName).PropertyType in
    let typeInfo = typeOfMember.GetGenericTypeDefinition() in
    typeInfo = typedefof<list<_>>

// val listPropType: PropertyInfo -> Type
// prop: the list property (e.g. 'T list) whose type is being returned
/// Returns the type of a list property
let listPropType (prop: PropertyInfo) =
    prop
        .PropertyType
        .GetGenericArguments()
        |> Array.head

// val getOneToManys: PropertyInfo[] -> PropertyInfo lista
/// Returns a list of all fields of a record type that have the
/// OneToMany attribute
let getOneToManys = 
    Array.filter hasAttribute<OneToManyAttribute>
    >> Array.toList

// val getTypes: PropertyInfo [] -> string []
/// Returns an array of strings representing the names of
/// types of record type fields
let getTypes =
    removeAttribute<OneToManyAttribute>
    >> Array.map (fun it -> it.PropertyType |> typeName )

// val getNames: PropertyInfo [] -> string []
/// Returns an array of strings representing the given names
/// of record fields
let getNames =
    removeAttribute<OneToManyAttribute>
    >> Array.map (fun it -> it.Name)

// val triMap: (('a -> 'b) * ('a -> 'c) * ('a -> 'd)) -> 'a -> ('b * 'c * 'd)
// x: first function to be applied 
// y: second function to be applied
// z: third function to be applied
// v: value to which x, y and z are being applied
/// Takes a tri-tuple of functions and applies them to a common input
/// and returns a tuple of the results
let triMap (x, y, z) v = (x v, y v, z v)

// val mapType: Type -> Table list
// t: the record type to map
/// Generates a list of Tables from a record type
/// Note: currently dysfunctional. This should essentially "map" a record type to
/// a relational table schema that can be used to generate real RDB tables. This is
/// currently a goal that has yet to be reached.
let rec mapType (t: System.Type) =
    let (oneToManys, types, names) =
        t.GetProperties()
        |> triMap (getOneToManys, getTypes, getNames)

    let tables = [{name = t.Name; schema = Map.ofArray (Array.zip names types)}]

    (* This is a result of the nature of One to Many relationships
     in that they require a table containing the "many" objects
     to have a column referencing the "one" object. Therefore
     a table must be created for each type of OTM list *)
    let mapOneToMany = listPropType >> mapType

    match oneToManys with
    | [] -> tables
    | [x] -> (mapOneToMany x) @ tables
    | hd :: tl ->
        tl
        |> List.map mapOneToMany
        |> List.reduce (@)
        |> ((@) <| mapOneToMany hd)