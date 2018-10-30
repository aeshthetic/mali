module Mali.Mapping
open Mali.Base.Attributes
open Mali.Base.Util
open Mali.Base.Types
open Mali.Base

// val getOneToManys: PropertyInfo[] -> PropertyInfo lista
/// Returns a list of all fields of a record type that have the
/// OneToMany attribute
let getOneToManys = 
    Array.filter hasAttribute<OneToManyAttribute>
    >> Array.toList

// val getTypes: PropertyInfo -> DbType
/// prop: property to get DbType of
/// Returns a DbType representing the type of a property
let getType prop =
    match (tryFindAttribute<RefAttribute> prop) with
    | Some attr -> Ref (prop.PropertyType, prop.PropertyType.GetProperty(attr.Parent))
    | None -> prop.PropertyType.Name |> Base.DbType.fromName

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

// val getColumn: PropertyInfo -> Column
// prop: The property to generate a column for
/// Generates a column value corresponding to a record property
let getColumn (prop: System.Reflection.PropertyInfo) =
    {name = prop.Name;
     dataType = (DbType.ofProperty prop)
     constraints = (getConstraints prop)}

// val mapType: Type -> Table list
// t: the record type to map
/// Generates a list of Tables from a record type
let rec fromType (t: System.Type) =
    let oneToManys =
        getOneToManys (t.GetProperties())
        
    let columns =
        t.GetProperties()
        |> Array.filter (fun it -> not <| hasAttribute<OneToManyAttribute> it)
        |> Array.map getColumn
        |> Array.toList

    let tables = [{name = t.Name; schema = columns}]

    (* This is a result of the nature of One to Many relationships
     in that they require a table containing the "many" objects
     to have a column referencing the "one" object. Therefore
     a table must be created for each type of OTM list *)
    let mapOneToMany = listPropType >> fromType

    match oneToManys with
    | [] -> tables
    | [x] -> (mapOneToMany x) @ tables
    | hd :: tl ->
        tl
        |> List.map mapOneToMany
        |> List.reduce (@)
        |> ((@) <| mapOneToMany hd)

// val createTable: Table -> string
// table: Table value to generate a CREATE statement for
/// Generates a CREATE statement given a Table
let createTable table =
    table.schema
    |> List.map (Column.stringOf PostgreSQL)
    |> String.concat ", "
    |> sprintf "CREATE TABLE %s (%s)" (table.name)

