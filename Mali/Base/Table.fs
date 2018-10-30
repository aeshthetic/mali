module Mali.Base.Table
open Mali.Base.Types
open Mali.Base

// val format: Table -> string
// table: the table to be formatted
/// Formats a table for pretty printing
let format (table: Table) =
    let schema =
        table.schema
        |> List.map (Column.stringOf PostgreSQL)
        |> String.concat "\n"
    sprintf "%s:\n%s" table.name schema