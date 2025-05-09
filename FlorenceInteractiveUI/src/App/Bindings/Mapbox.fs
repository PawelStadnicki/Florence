[<AutoOpen>]
module MapboxBinding

open Fable.Core
   
[<ImportAll("mapbox-gl")>]
let mapboxgl: obj = jsNative

[<Import("Map","mapbox-gl")>]
[<Emit("new $0($1)")>]
let mapbox(_p:obj): obj = jsNative

[<Import("Deck", "@deck.gl/core")>]
[<Emit("new $0($1)")>]
let Deck(_opt: obj) = jsNative

[<Import("GeoJsonLayer", "@deck.gl/layers")>]
[<Emit("new $0($1)")>]
let GeoJsonLayer(_options: obj) = jsNative