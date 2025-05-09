namespace Florence 

open NetTopologySuite.Features
open NetTopologySuite.Geometries

module NetTopologySuite =

    module Geometry =
        let position (geometry: Geometry) =
            geometry.Centroid.X, geometry.Centroid.Y
    
    module Feature =
        let position (feature: IFeature) =
            feature.Geometry
            |> Geometry.position
            
    module Place =
           
        let position (place: Place<_, Geometry>) =
            place.Geometry |> Geometry.position
            
        let fromFeature (feature: IFeature) =
            {
                Properties =
                    if feature.Attributes.Exists "types" then
                        {|
                           Categories = feature.Attributes.Item("types").ToString().Split(';')|}
                    else
                        {|
                           Categories = [||]
                        |}
                Geometry = feature.Geometry
            }