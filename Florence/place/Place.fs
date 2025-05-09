namespace Florence

(*

Place, in general, is identical to the notion of "Feature" in a GeoJSON format,
but its main goal is just to be a transient structure for .NET libraries that can feed it in a strongly-type manner:
- geometries can be parsed from a raw geojson with NetTopologySuite
- properties are always different for most of the geojson files,
  in interactive programming we don't want to create types on every data exploration,
  as they can be provided with JsonProvider
*)

type Place<'P, 'G> =
    {
        Geometry: 'G
        Properties: 'P
    }