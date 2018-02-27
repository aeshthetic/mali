module Dixie.Util.Table
open Dixie.Util.Types

// val format: Table -> string
// table: the table to be formatted
// Formats a table for pretty printing
let format (table: Table) =
    let schema =
        table.schema
        |> Map.toSeq
        |> Seq.map (fun (cName, cType) -> sprintf "%s: %s" cName cType)
        |> String.concat " | "
    sprintf "%s:\n%s" table.name schema
