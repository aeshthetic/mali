module Mali.Util.Table
open Mali.Util.Types
open Mali.Util

// val format: Table -> string
// table: the table to be formatted
/// Formats a table for pretty printing
let format (table: Table) =
    let schema =
        table.schema
        |> List.map (Column.stringOf PostgreSQL)
        |> String.concat "\n"
    sprintf "%s:\n%s" table.name schema