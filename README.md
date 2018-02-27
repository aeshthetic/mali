# Dixie
## bi-directional type-relational mapper for F#

### What does "bi-directional" mean?
Dixie is _bi-directional_. This means that it supports two "modes" of use. One covers the situation in which the user has modelled their data in native F# types and wants to map these types to a relational database. The other covers the situation in which the user already has a relational database set up with their data modelled the way they'd like it to be, and wants to map these tables to native F# types.

C#'s Entity Framework calls the first situation a "code-first" approach, and the other a "database-first" approach.

### What's a "type-relational mapper"?
Many popular Object Oriented languages have tools within their ecosystem called ORMs. ORM stands for Object-Relational Mapping, and is a technique that converts objects in an OO language to records in a relational database. Type-Relational Mapping is very similar, but maps functional record types to database tables as opposed to mapping OO classes.
