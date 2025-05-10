module App2

open App
open System
open Fable.Core
open Fable.Core.JS
open Feliz
open Sutil
open Sutil.Core
open Sutil.Styling
open Sutil.CoreElements
open Sutil.DomHelpers
open Sutil.PseudoCss
open Geojson
open Fable.Core
open Fable.Core.JsInterop
open type Feliz.length
open Sutil.DomHelpers
open Fable.React.ReactDomBindings
open Browser.Types


let init () =
    fun () ->
        let state = {
            Name = ""
            Position = None
            People = []
            GroupName = "Family" 
            Autocomplete = null
        }
 
        state,  
            Cmd.batch [
                Cmd.ofMsg Init
            ]

let update msg state =
    match msg with
    | AddPerson ->
        {
          state with
            Name = ""
            People = { Name = state.Name; Position = state.Position.Value } :: state.People
        }, Cmd.none
    | PositionChange (lon, lat) ->
        { state with Position = Some (lon, lat) }, Cmd.none
    | NameChange change ->
        { state with Name = change }, Cmd.none
    | GroupNameChange change ->
        { state with GroupName = change }, Cmd.none
    | LoadSampleData ->
        let loader () =
          async {
              JS.console.log $"loadeding sample data"
              let data = $"\"\"\"{App.Store.famousFlorencePeople}\"\"\""
              let code = $"""type SamplePeople = DistanceProvider.GeojsonContent<{data}>"""
              do! Polyglot.Languages.sendFs code
          } 
        state, Cmd.OfAsync.perform loader () (fun _ -> Continue)
    | Init ->       
        let autocomplete = Geoapify.autocomplete(Browser.Dom.document.getElementById "autocomplete", API_KEYS.geo, {| lang = "pl"; text = "Wrocław"; placeholder= "wprowadź adres " |})
        state,
        Cmd.batch [
            Cmd.ofEffect (fun dispatch ->
              autocomplete?on("select", fun location -> dispatch (PositionChange (location?properties?lon, location?properties?lat)))
            )
            Cmd.ofMsg LoadSampleData
        ]
            
    | Continue -> state, Cmd.none

let view () =

    let model, dispatch = () |> Store.makeElmish (init ()) update ignore
    Styles.geoapify() |> ignore

    Html.div [
        Attr.className "app h-screen max-h-screen"
    
        disposeOnUnmount [ model ]
    
        Attr.style [
            Css.fontFamily "Arial, Helvetica, sans-serif"
        ]
        Html.div [
            Attr.className "flex w-full"
            Html.div [
              Attr.className "navbar bg-base-100 shadow-sm"
              fragment [
                Html.div [
                  Attr.className "navbar-start"
                  fragment [
                    Html.div [
                      Attr.style [
                        Css.displayFlex  
                      ]
                      Html.a [ Attr.className "btn btn-ghost text-xl"; Attr.text "Florence Entry" ]
                      Html.div [
                        Attr.id "autocomplete"
                        Attr.style [
                          Css.width 300
                        ]
                        Attr.className "autocomplete-container"
                      ]         
                      Bind.el(model, fun m ->
                        Html.input [
                        Attr.typeText
                        Attr.text "name"
                        Attr.className "w-full px-4 py-2 border border-gray-300 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        Ev.onChange (NameChange >> dispatch) 
                      ])    
                      Html.button [
                          Attr.text "+"
                          Attr.className "btn btn-secondary"
                          Ev.onClick (fun _ -> (dispatch AddPerson))
                      ]
                    ]
                  ]
                ]
              ]
            ]
        ]
        Bind.el( model , fun m ->
            m.People |> List.map (fun p ->
              Html.p p.Name
              ) |> Html.ul
          )
        Bind.el( model , fun m ->
          fragment [
            Bind.el(model, fun m ->
                Html.input [
                    Attr.typeText
                    Attr.value m.GroupName
                    Attr.text "name"
                    Attr.className "w-full px-4 py-2 rounded-2xl border border-gray-300 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    Ev.onChange (fun t -> dispatch (GroupNameChange t)) 
              ])
            Html.button [
                Attr.className "btn btn-info"
                Attr.text "Send to interactive"
                Ev.onClick ( fun _ ->
                    let raw = $"\"\"\"{People.serialize m.People}\"\"\""
                    let code = $"""type {m.GroupName} = DistanceProvider.GeojsonContent<{raw}>"""
                    async {
                        JS.console.log($"""code {code}""")
                        let! _ = Polyglot.Languages.sendFs code 
                        return ()
                    } |> Async.StartImmediate
                )
            ]
            //Renderer.view(App.Store.famousFlorencePeople |> JSON.parse)
            InteractiveTester.view(obj())
          ])
      ]
//let render () =
view() |> Program.mount
 
//let renderCommand (id, data: string) = Renderer.render(id, data)
