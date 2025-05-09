module Renderer

open Fable.Core.JS
open Sutil
open Sutil.CoreElements
open Fable.Core.JsInterop
open type Feliz.length

type State = { data: obj }
type Msg = Init

let update msg state =
    match msg with
    | Init -> state, Cmd.none

let private renderGeojson(container: string, map: obj, geojson: obj) =
   let _deckgl =
     Deck({|
         container = container
         map = map
         layers = ResizeArray[|
           GeoJsonLayer(
             {|
               id = "geojson-layer"
               data = geojson
               pickable = true
               stroked = true
               filled = true
               pointRadiusMinPixels = 5
               getFillColor = ResizeArray[|255; 0; 0; 120|]
               getLineColor = ResizeArray[|0; 255; 0; 120|]
            |}
          )
         |]
     |})
   ()

let private mapbox (container: string, geojson: obj) =
      let map =
         MapboxBinding.mapbox({|
             container = container
             style = "mapbox://styles/mapbox/light-v10"
             center = ResizeArray[|11.2558; 43.7696|]
             zoom = 12
             accessToken = "pk.eyJ1Ijoib2RzaWV3IiwiYSI6ImNsODBtNjZpcTAwMTUzd280cmo2Z2RuMGIifQ.lmNP8DKuzOBjv8U6Jprw1Q"
           |})
         
      map?on("load", fun () ->
         renderGeojson(container, map, geojson))

let init (container,data) =
    fun () ->
        { data = data },
            Cmd.ofEffect (fun _dispatch ->
              mapbox (container, data)
            )

let view(data: obj) =
    let container = "map-container"
    let _model, _dispatch = () |> Store.makeElmish (init (container, data)) update ignore
    Html.div [
        Attr.id container
        Attr.style [ Css.height 500 ]
        headStylesheet "https://api.mapbox.com/mapbox-gl-js/v3.2.0/mapbox-gl.css"
    ]
    
let render(id: string, data: string) =
    Program.mount(id, view(data |> JSON.parse))