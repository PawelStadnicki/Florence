namespace Florence

open FSharp.Data
open NetTopologySuite.Geometries

module Parser = 

    let geometry (json: JsonValue) =
        json.ToString().parse<Geometry>()

    let get (content: string) (name: string) =
        
        let name = name.Replace("-","_").Replace("'","").Replace(".","_").Replace(" ","")

        $"""
        let {name} = 
            JsonProvider<{content.trippleQuote()}, InferTypesFromValues = false>.GetSample().Features 
            |> Seq.map (fun i -> 
                {{ 
                    Place.Geometry = i.Geometry.JsonValue |> Florence.Parser.geometry
                    Place.Properties = i.Properties
                }}
            )            
          """  