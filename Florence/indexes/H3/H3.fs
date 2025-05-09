namespace Florence

open H3.Extensions
open H3.Algorithms
open NetTopologySuite.Features
open NetTopologySuite.Geometries
open Florence

module H3 =

    let fromResolutionAndGeojson<'p> (geojson: string) (resolution: int)  = 
        
        let source = geojson.parse<FeatureCollection>().Item 0
        
        let fs =
            source.Geometry.Fill(resolution, VertexTestMode.Any)
            |> Seq.map ( fun index -> 
                let attrs = AttributesTable()
                attrs.upsert("h3", string index)
                {
                    Properties = string index  
                    Geometry = index.GetCellBoundary()
                }
            )

        fs.toGeojson<string, Polygon>()

    let fromGeojson<'p> (resolution: int)  (geojson: string)   = 
        
        let source = geojson.parse<FeatureCollection>().Item 0
        
        let fs =
            source.Geometry.Fill(resolution, VertexTestMode.Any)
            |> Seq.map ( fun index -> 
                let attrs = AttributesTable()
                attrs.upsert("h3", string index)
                {
                    Properties = string index
                    Geometry = index.GetCellBoundary() :> Geometry//, attrs).Geometry
                }
            )

        fs
    let fromResolutionAndGeojsonCount<'p> (geojson: string) (resolution: int)  = 
        
        let source = geojson.parse<FeatureCollection>().Item 0
        
        let fs =
            source.Geometry.Fill(resolution, VertexTestMode.Any)
            |> Seq.map ( fun index -> 
                let attrs = AttributesTable()
                attrs.upsert("h3", string index)
                {
                    Properties = string index
                    Geometry = index.GetCellBoundary()
                }
            )

        fs |> Seq.length

    let fromResolution<'p> (resolution: int) (places: seq<Place<'p, Geometry>>): seq<Place<string, Geometry>> = 
        
        let source = places |> Seq.exactlyOne
        
        source.Geometry.Fill(resolution, VertexTestMode.Any)
        |> Seq.map ( fun index -> 
            let attrs = AttributesTable()
            attrs.upsert("h3", string index)
            {
                Properties = string index
                Geometry = index.GetCellBoundary() :> Geometry 
            }
        )

    let index (resolution: int) (point: float*float) =
        resolution 
        |> point.toGeometry().Centroid.Coordinate.ToH3Index
        |> string