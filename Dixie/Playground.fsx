module Orm.Testing
open System
open Orm.Util
open Orm.Util.Attributes

// Users
// | id | name | email
type User = 
    {id: int;
     name: string;
     email: string;
     [<OneToMany>]
     posts: Post list;}

// Posts
// | id | content | timeStamp | ref user
and Post =
    {id: int;
     content: string;
     timeStamp: DateTime;
     [<Ref(Parent="posts")>]
     poster: User;
     liked: User list}

// Liked
// | id | ref user | ref post

isRef (typeof<User>.GetProperty("posts")) (typeof<Post>.GetProperty("poster"))