module Mali.Base.Types

[<AbstractClass>]
type Constraint() =
    inherit System.Attribute()

// type OneToManyAttribute
/// Attribute type to represent OTMs in record type members
type OneToManyAttribute() =
    inherit System.Attribute()

// type RefAttribute
/// Attribute type representing record type members that refer to another record type
/// often for the purposes of modelling certain relations
type RefAttribute() =
    inherit System.Attribute()
    member val Parent: string = "" with get, set

type NonNullableAttribute() =
    inherit Constraint()
    override this.ToString() = "NOT NULL"

type UniqueAttribute() =
    inherit Constraint()
    override this.ToString() = "UNIQUE"

type PrimaryKeyAttribute() =
    inherit Constraint()
    override this.ToString() = "PRIMARY KEY"

type CheckAttribute<'T>(predicate: ('T -> bool)) =
    inherit Constraint()
    member this.Run = predicate
 

type DbType =
    | Int
    | Float
    | String
    | Bool
    | Char
    | Time
    | DateTime
    | Ref of (System.Type * System.Reflection.PropertyInfo)

type Column = {name: string; dataType: DbType; constraints: Constraint []}

type Table = {name: string; schema: Column list}

type DBMS =
    | PostgreSQL