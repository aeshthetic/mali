module Mali.Base.Util
open System.Reflection

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