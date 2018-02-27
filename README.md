# Dixie
A Bi-Directional Type-Relational Mapper for F#

### What does "bi-directional" mean?
Dixie is _bi-directional_. This means that it supports two "modes" of use. One covers the situation in which the user has modelled their data in native F# types and wants to map these types to a relational database. The other covers the situation in which the user already has a relational database set up with their data modelled the way they'd like it to be, and wants to map these tables to native F# types.

C#'s Entity Framework calls the first situation a "code-first" approach, and the other a "database-first" approach.

### What's a "type-relational mapper"?
Many popular Object Oriented languages have tools within their ecosystem called ORMs. ORM stands for Object-Relational Mapping, and is a technique that converts objects in an OO language to records in a relational database. Type-Relational Mapping is very similar, but maps functional record types to database tables as opposed to mapping OO classes.

## Building & Contributing
### Contributing
Thank you so much for contributing to Dixie! While pull requests are welcome and encouraged, there are several other ways in which
you can contribute to Dixie's development, namely by opening or commenting on issues in order to:
* Request features
* Report bugs
* Give feedback regarding how an issue is being handled
* Ask for better documentation on a specific part of the codebase

Code quality is a priority, but my code is not perfect nor do I expect yours to be. However, when submitting pull requests keep these things in mind:
* Ensure that your code is well commented, and that only descriptions are XML Doc'd (i.e. triple-slashed)
* Try to comply with section 3 of [the F# Component Design Guidelines](http://fsharp.org/specs/component-design-guidelines/#3-guidelines-for-f-facing-libraries)
* If you can test your own code, great! If not, it might get pushed to the testing branch before the master branch until it's been effectively tested

#### Note about branching
It's a personal policy of mine to never have more than one dev working on a branch. This means that if you're on the Dev branch implementing a 
certain feature, you should make a branch for it. If you're implementing the feature with a friend then you should split the work among yourselves and create separate
branches from the feature branch. I understand that this can become annoying, especially if you forget, but hopefully the pay-off will become obvious with time.

### Building
At this time, Dixie does not use any dependencies besides the .NET Core 2 platform itself.
Once you [install .NET Core](https://www.microsoft.com/net/download), it should be easy to build and run the project. If you're using
Windows and Visual Studio, you should have the .NET Core tools installed along with Visual F#. Meanwhile if you're using Visual Studio on any platform,
just ensure that F# and the .NET Core SDK are installed. Once you've done so, you can run the existing demo with
```
$ cd Playground
$ dotnet run
```