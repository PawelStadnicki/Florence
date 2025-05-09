namespace Florence

open System.Runtime.CompilerServices
open NetTopologySuite.Features
open NetTopologySuite.IO.Converters
open NetTopologySuite.Geometries
open NetTopologySuite.Algorithm
open System.Text.Json
open System.Text.Json.Serialization
open System.Text.Encodings.Web
open System.Text.Unicode

type NetTopologySuiteExtensions() =
    [<Extension>]
    static member parse<'Geometry>(geojson: string) =
        let factory = GeoJsonConverterFactory()
        let indentedOptions = 
            JsonSerializerOptions(
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true)
        indentedOptions.Converters.Add factory

        JsonSerializer.Deserialize<'Geometry>(geojson, indentedOptions)

    [<Extension>]
    static member toGeojson<'P, 'G when 'G :> Geometry>(places: Place<'P, 'G> seq, ?attrs: AttributesTable) =
        let at = defaultArg attrs (AttributesTable())
        let fc = FeatureCollection()
        places |> Seq.map (fun p -> 
            let f = Feature() 
            f.Geometry <- p.Geometry
            f.Attributes <- at
            f
        ) 
        |> Seq.iter fc.Add

        let factory = GeoJsonConverterFactory()
        let indentedOptions = 
            JsonSerializerOptions(
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true)
                
        indentedOptions.Converters.Add factory
        indentedOptions.Encoder <- JavaScriptEncoder.Create([| UnicodeRanges.All |])  

        JsonSerializer.Serialize(fc, indentedOptions)

    [<Extension>]
    static member toGeometry(point: float * float) =
        point |> Coordinate |> Point 
 
    [<Extension>]
    static member position(feature: IFeature) =
        match feature.Geometry.GeometryType with
        | "Point" ->
            let point = feature.Geometry :?> Point
            point.Coordinate.X, point.Coordinate.Y
        | _ -> 
            let c = Centroid.GetCentroid(feature.Geometry)
            c.X, c.Y

    [<Extension>]
    static member upsert (feature: IAttributesTable, key, value: obj) =
        if feature.Exists key then 
            feature.[key] <- value
        else 
            feature.Add(key, value)
