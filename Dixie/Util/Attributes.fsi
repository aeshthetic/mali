namespace Dixie.Util
  module Attributes = begin
    type OneToManyAttribute =
      class
        inherit System.Attribute
        new : unit -> OneToManyAttribute
      end
    type RefAttribute =
      class
        inherit System.Attribute
        new : unit -> RefAttribute
        member Parent : string
        member Parent : string with set
      end
    val hasAttribute<'T> : prop:System.Reflection.PropertyInfo -> bool
    val removeAttribute<'T> :
      props:System.Reflection.PropertyInfo [] ->
        System.Reflection.PropertyInfo []
    val tryFindAttribute :
      prop:System.Reflection.PropertyInfo -> #System.Attribute option
    val isRef :
      propT:System.Reflection.PropertyInfo ->
        propA:System.Reflection.PropertyInfo -> bool
    val hasRef<'A> : propT:System.Reflection.PropertyInfo -> bool
  end

