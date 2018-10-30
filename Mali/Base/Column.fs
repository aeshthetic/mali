module Mali.Base.Column
open Mali.Base.Types

// val stringOf: DBMS -> Column -> string
// dbms: the DBMS of the connection for which the string representation should be formatted
// col: the column being represented by the output string
/// Returns a string representing a column in the way it would be in a CREATE statement
let stringOf dbms col =
    let stringFunction =
        match dbms with
        | PostgreSQL -> DbType.postgres
    let typeString = stringFunction col.dataType
    let constraints =
        col.constraints
        |> Array.map (fun it -> it.ToString())
        |> String.concat " "

    sprintf "%s %s %s" (col.name) (typeString) (constraints)