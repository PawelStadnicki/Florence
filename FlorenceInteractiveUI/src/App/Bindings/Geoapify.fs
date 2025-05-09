module Geoapify

open Fable.Core

[<Import("GeocoderAutocomplete","@geoapify/geocoder-autocomplete")>]
[<Emit("new $0($1, $2, $3)")>]
let autocomplete(_target: obj, _key: string, _opt: obj) = jsNative