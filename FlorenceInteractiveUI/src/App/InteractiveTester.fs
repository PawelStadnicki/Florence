module InteractiveTester

open Fable.Core.JS
open Sutil
open Sutil.CoreElements
open Fable.Core.JsInterop
open type Feliz.length
open Polyglot.Languages
open App

type State = { Values: List<string*string>}
type Msg =
    | Init
    | Done
    | FSharpValueProduced of name: string * value: string

let update msg state =
    match msg with
    | Init -> state, Cmd.none
    | Done -> state, Cmd.none
    | FSharpValueProduced (n,value) ->
         { state with Values = (n,value) :: state.Values  }, Cmd.none

let init (container,data) =
    let command name =
        {|
            commandType = "RequestValue"
            command =
                {|
                    name = name
                    targetKernelName = "fsharp"
                |}
        |}
    let p () = 
        promise {
            let! _ = webview?compositeKernel?send(fs "let x = 15") |> Thenable.toPromise
            let! _ = webview?compositeKernel?send(fs """let y = "abra" """) |> Thenable.toPromise
            let! r = webview?compositeKernel?send(command "x") |> Thenable.toPromise
            let! r = webview?compositeKernel?send(command "y") |> Thenable.toPromise
            debugger()
            return ()
        } 
    fun () ->
        { Values = [] },
             Cmd.batch [
                 Cmd.ofEffect (fun dispatch ->
                     webview?compositeKernel?subscribeToKernelEvents( fun e ->
                         if e?eventType = "ValueProduced" then dispatch (FSharpValueProduced (e?event?name, e?event?formattedValue?value)))

                 )
                 Cmd.OfPromise.perform p () (fun _ -> Done)
             ]

let view(data: obj) =
    let container = "container"
    let _model, _dispatch = () |> Store.makeElmish (init (container, data)) update ignore
    Html.div [
        Attr.id container
        Attr.style [ Css.height 500 ]
        headStylesheet "https://api.mapbox.com/mapbox-gl-js/v3.2.0/mapbox-gl.css"
        Bind.el(_model,  fun m ->
            m.Values
            |> List.map (fun (n,v) ->
                 Html.li [ Attr.name n; Attr.text v ])
            |> Html.ul 
            )
    ]
    
let render(id: string, data: string) =
    Program.mount(id, view(data |> JSON.parse))