namespace Florence

open MathNet.Numerics.Statistics
open NetTopologySuite.Features
open NetTopologySuite.Geometries
open Florence.NetTopologySuite

module Ranker =

    let private round (x: float) (n: int) = System.Math.Round(x, n)

    let inline private run<'properties, 'target when 'target: comparison> 
        (f: Place<'properties, Geometry> -> 'targe) 
        (places: seq<Place<'properties, Geometry>>)  =
 
        let scores = places |> Array.ofSeq |> Array.Parallel.map (fun place -> place, f place)  
             
        let all = scores |> Seq.map snd
        let max = all |> Seq.max

        scores 
        |> Array.Parallel.map ( fun (place, score) ->

            let ratio = score / max
            let rank = Statistics.QuantileRank(all, float score)
            {
                Geometry = place.Geometry
                Properties =
                    {
                        Ranks =
                            {
                                Ratio = if  ratio = nan then 0. else ratio
                                Rank = if  rank = nan then 0. else rank
                                Score = if  score = nan then 0. else score
                                Position = 0
                            }
                        Properties = place.Properties
                    }
            }
        )  
        |> Seq.sortBy ( fun r -> r.Properties.Ranks.Score)
        |> Seq.rev
        |> Seq.mapi ( fun i result -> 
            { result with Properties.Ranks.Position = i + 1 }
        )
        
    let rankedFeature<'P>(f: Place<'P, Geometry> -> float) (palette: Palette) (areas: seq<Place<'P, Geometry>>) =
        let color = palette.color
        run f areas
        |> Seq.mapi ( fun _i d -> 
            let feature = Feature()
            feature.Geometry <- d.Geometry
            feature.Attributes <- AttributesTable()
            feature.Attributes.upsert("rank",  round d.Properties.Ranks.Rank  4)
            feature.Attributes.upsert("ratio", round d.Properties.Ranks.Ratio 4)
            feature.Attributes.upsert("score", round d.Properties.Ranks.Score 12)
            feature.Attributes.upsert("place", d.Properties.Ranks.Position)
            feature.Attributes.upsert("color", color d.Properties.Ranks.Rank)
            feature.Attributes.upsert("fill", color d.Properties.Ranks.Ratio)
            feature.Attributes.upsert("fill-opacity", 1.) // 0.8)
            feature.Attributes.upsert("stroke-width", 1)
            feature.Attributes.upsert("stroke", color d.Properties.Ranks.Ratio)
            feature
        )
        
    let ranker<'P>(f: Place<'P, Geometry> -> float) (areas: seq<Place<'P, Geometry>>) =
        run f areas

    let fill<'properties>(f: Place<'properties, 'g> -> float) (palette: Palette) (fill: Place<RankedProperties<_>,Geometry>->float) (areas: seq<Place<'properties, 'g>>) =
        let color = palette.color
        run f areas
        |> Seq.mapi ( fun _i d -> 
            let feature = Feature()
            feature.Geometry <- d.Geometry
            feature.Attributes <- AttributesTable()
            feature.Attributes.upsert("rank",  round d.Properties.Ranks.Rank  4)
            feature.Attributes.upsert("ratio", round d.Properties.Ranks.Ratio 4)
            feature.Attributes.upsert("score", round d.Properties.Ranks.Score 12)
            feature.Attributes.upsert("place", d.Properties.Ranks.Position)
            feature.Attributes.upsert("color", color d.Properties.Ranks.Rank)
            //geojson.io support
            feature.Attributes.upsert("fill", color (fill d))
            feature.Attributes.upsert("fill-opacity", 0.8) // 0.8)
            feature.Attributes.upsert("stroke-width", 1)
            feature.Attributes.upsert("stroke", color (fill d))
            feature
        )
        
    let toRankedFeatures<'properties>(f: float*float -> float) (areas: seq<Place<'properties, 'g>>) = 
        run (Place.position >> f) areas
