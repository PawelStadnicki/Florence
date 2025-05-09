// This is the definition of a thenable from ts2fable's generation VsCode API
namespace App
open Fable.Core
open Fable

type [<AllowNullLiteral>] Thenable<'T> =
    abstract ``then``: ?onfulfilled: ('T -> U2<'TResult, Thenable<'TResult>>) * ?onrejected: (obj option -> U2<'TResult, Thenable<'TResult>>) -> Thenable<'TResult>
    abstract ``then``: ?onfulfilled: ('T -> U2<'TResult, Thenable<'TResult>>) * ?onrejected: (obj option -> unit) -> Thenable<'TResult>
module Thenable =
    // Transform a thenable into a promise
    let toPromise (t: Thenable<'t>): JS.Promise<'t> =  unbox t
    let toUnitPromise (t: Async<unit>): JS.Promise<unit> =  unbox t
    type Promise.PromiseBuilder with

        member x.Source(t: Thenable<'t>): JS.Promise<'t> = toPromise t

        member _.Source(p: JS.Promise<'T1>): JS.Promise<'T1> = p
        member _.Source(ps: #seq<_>): _ = ps
