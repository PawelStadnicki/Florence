namespace Florence

open System.Runtime.CompilerServices
    
type UtilsExtensions() =
     
    [<Extension>]
    static member trippleQuote(raw: string) =
        $"\"\"\"{raw}\"\"\""
 
 [<AutoOpen>]       
module Utils =
    let invert x = 1. / x
    let (>>=) binder (opt: 'T option) = Option.bind binder opt
    let (|>>) (opt: 'T option) mapper = Option.map mapper opt
    let (|?) (opt: 'T option) value = Option.defaultValue value opt
    let (|!) (opt: 'T option) thunk = Option.defaultWith thunk opt
        