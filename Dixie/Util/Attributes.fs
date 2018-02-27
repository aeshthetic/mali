module Dixie.Util.Attributes
open System

type OneToManyAttribute() = inherit System.Attribute()
type RefAttribute() =
    inherit System.Attribute()
    member val Parent: string = "" with get, set

let hasAttribute<'T> (prop: Reflection.PropertyInfo) = Attribute.IsDefined(prop, typeof<'T>)

let removeAttribute<'T> (props: Reflection.PropertyInfo []) =
    props
    |> Array.filter (fun it -> not <| hasAttribute<'T> it)

let tryFindAttribute<'T when 'T :> System.Attribute> (prop: Reflection.PropertyInfo) =
    prop.GetCustomAttributes(typeof<'T>, true)
    |> Array.tryHead
    |> (fun head ->
        match head with
        | Some hd -> Some (hd :?> 'T)
        | None -> None)
    

// val isRef: System.Reflection.PropertyInfo -> System.Reflection.PropertyInfo -> bool
// propA: the property which 
// Check's whether propA is referencing propT via the Ref attribute's parent member
let isRef (propT: Reflection.PropertyInfo) propA =
    if hasAttribute<RefAttribute> propA then
        match (tryFindAttribute<RefAttribute> propA) with
        | Some attr -> attr.Parent = propT.Name
        | None -> false
    else
        false

// val hasRef<'A>: System.Reflection.PropertyInfo -> bool
// Check's whether a type contains a reference to a certain property
let hasRef<'A> propT =
    if hasAttribute<OneToManyAttribute> propT then
        typeof<'A>.GetProperties()
        |> Array.exists (isRef propT)
    else
        false


