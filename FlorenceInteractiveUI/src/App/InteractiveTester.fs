module InteractiveTester

open Fable.Core
open Fable.Core.JS
open Sutil
open Sutil.CoreElements
open Fable.Core.JsInterop
open type Feliz.length
open Polyglot.Languages
open App
open Fable.Core.JsInterop
type State = { Values: List<string*string>; ToastEnabled: bool; Messages: string list }

type Msg =
    | Init
    | Done
    | ValueProduced of kernel: string * name: string * value: string
    | ClearLastMessage

let update msg state =
    match msg with
    | Init -> state, Cmd.none
    | Done -> state, Cmd.none
    | ValueProduced (kernel, name, value) ->
         { state
           with
            Values = (name,value) :: state.Values
            Messages  = $"{kernel} produced {name}: {value}" :: state.Messages }, Cmd.none
    | ClearLastMessage ->
        match state.Messages.Length with
        | 0 -> state
        | _ ->
            { state with Messages = state.Messages |> List.take (state.Messages.Length - 1 ) }
        , Cmd.none
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
            return ()
        } 
    fun () ->
        { Values = []; ToastEnabled = true; Messages = [] },
             Cmd.batch [
                 Cmd.ofEffect (fun dispatch ->
                     webview?compositeKernel?subscribeToKernelEvents( fun e ->
                         debugger()
                         if e?eventType = "ValueProduced" then dispatch (ValueProduced (e?command?command?targetKernelName, e?event?name, e?event?formattedValue?value)))

                 )
                 Cmd.ofEffect (fun dispatch ->
                    setInterval (fun () ->(dispatch ClearLastMessage)) 5000 |> ignore
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
        Html.div [
            Attr.id "messages"
            Attr.className "flex flex-col h-full"
            Bind.el(_model, fun m ->
                
                Html.table [
                    Attr.className "mt-auto"
                    m.Messages
                    |> List.map (fun msg -> Html.tr [Attr.text msg])
                    |> Html.tr
                ]
                
                
                )
        ]
    ]
    
let render(id: string, data: string) =
    Program.mount(id, view(data |> JSON.parse))