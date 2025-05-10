module InteractiveTester

open Fable.Core.JS
open Sutil
open Sutil.CoreElements
open Fable.Core.JsInterop
open type Feliz.length
open Polyglot.Languages
open App

type State = { data: obj }
type Msg = Init | Done

let update msg state =
    match msg with
    | Init -> state, Cmd.none
    | Done -> state, Cmd.none

let init (container,data) =
    let command =
        {|
            commandType = "RequestValue"
            command =
                {|
                    name = "x"
                    targetKernelName = "fsharp"
                |}
        |}
    let p () = 
        promise {
            let! _ = webview?compositeKernel?send(fs "let x = 15") |> Thenable.toPromise
            let! r = webview?compositeKernel?send(command) |> Thenable.toPromise
            console.log $"RequestValue: {JSON.stringify r}"
            return ()
        } 

    fun () ->
        { data = data },
            Cmd.OfPromise.perform p () (fun _ -> Done)

let view(data: obj) =
    let container = "container"
    let _model, _dispatch = () |> Store.makeElmish (init (container, data)) update ignore
    Html.div [
        Attr.id container
        Attr.style [ Css.height 500 ]
        headStylesheet "https://api.mapbox.com/mapbox-gl-js/v3.2.0/mapbox-gl.css"
    ]
    
let render(id: string, data: string) =
    Program.mount(id, view(data |> JSON.parse))