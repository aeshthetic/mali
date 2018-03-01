module Dixie.Util.Types

type DbType =
    | Int
    | Float
    | String
    | Bool
    | Char
    | Date
    | Ref of (System.Type * System.Reflection.PropertyInfo)

type Table = {name: string; schema: Map<string, DbType>}