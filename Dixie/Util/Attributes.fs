module Dixie.Util.Attributes
open System

// type OneToManyAttribute
/// Attribute type to represent OTMs in record type members
type OneToManyAttribute() = inherit System.Attribute()

// type RefAttribute
/// Attribute type representing record type members that refer to another record type
/// often for the purposes of modelling certain relations
type RefAttribute() =
    inherit System.Attribute()
    member val Parent: string = "" with get, set

// val hasAttribute<'T>: System.Reflection.PropertyInfo -> bool
// 'T: the attribute being checked for
// prop: the property being checked
/// Checks whether a property has a certain attribute
let hasAttribute<'T> (prop: Reflection.PropertyInfo) = Attribute.IsDefined(prop, typeof<'T>)

// val removeAttribute<'T>: System.Reflection.PropertyInfo [] -> System.Reflection.PropertyInfo []
// 'T: the attribute of which properties having it are to be removed
/// Returns a property array where properties with attribute 'T have been removed
let removeAttribute<'T> = Array.filter (fun it -> not <| hasAttribute<'T> it)

// val tryFindAttribute<System.Attribute>: System.Reflection.PropertyInfo -> #Attribute option
// 'T: the attribute to search for
// prop: the property of which an attribute is being found
/// Attempts to find an attribute among a property, returns None if not found
let tryFindAttribute<'T when 'T :> System.Attribute> (prop: Reflection.PropertyInfo) =
    prop.GetCustomAttributes(typeof<'T>, true)
    |> Array.tryHead
    |> (fun head ->
        match head with
        | Some hd -> Some (hd :?> 'T)
        | None -> None)

// val isRef: System.Reflection.PropertyInfo -> System.Reflection.PropertyInfo -> bool
// propT: the property with the OneToMany attribute
// propA: the property which is a reference to propT
/// Check's whether propA is referencing propT
let isRef (propT: Reflection.PropertyInfo) propA =
    if hasAttribute<RefAttribute> propA then
        match (tryFindAttribute<RefAttribute> propA) with
        | Some attr -> attr.Parent = propT.Name
        | None -> false
    else
        false

// val hasRef<'A>: System.Reflection.PropertyInfo -> bool
// 'A: the type which potentially contains a reference property to propT
// propT: the OneToMany property to which 'A potentially contains a reference
// Check's whether a type contains a reference to a certain property
let hasRef<'A> propT =
    if hasAttribute<OneToManyAttribute> propT then
        typeof<'A>.GetProperties()
        |> Array.exists (isRef propT)
    else
        false