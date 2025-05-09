namespace Florence

open NetTopologySuite.Features
open NetTopologySuite.Operation.Distance
open NetTopologySuite.Algorithm.Locate
open NetTopologySuite.Geometries
open type System.Math
open Florence.NetTopologySuite

module Distance =

    let calc  ((lon1,lat1): float*float) ((lon2,lat2):float*float) =
        let r = 6371.0;
        let dLat = (lat2 - lat1) * PI / 180.0
        let dLon = (lon2 - lon1) * PI / 180.0
        let lat1 = lat1 * PI / 180.0
        let lat2 = lat2 * PI / 180.0
        
        let a = Sin(dLat/2.0) * Sin(dLat/2.0) +
                Sin(dLon/2.0) * Sin(dLon/2.0) * Cos lat1 * Cos lat2
        let c = 2.0 * Atan2(Sqrt(a), Sqrt(1.0-a))
        r * c * 1000.
        
    let distanceGeometry (p1: Place<_, Geometry>) (p2: Place<_, Geometry>) =
        DistanceOp.Distance(p1.Geometry, p2.Geometry)

    let real (place1: Place<_, Geometry>) (place2: Place<_, Geometry>) =

        let n = DistanceOp.NearestPoints(place1.Geometry.Boundary, place2.Geometry.Boundary)
        let a = n.[0]
        let b = n |> Array.last
        calc (a.X, a.Y) (b.X, b.Y)
        
    let ``to`` ((lon1,lat1): float*float) (p1:Place<_, Geometry>) =
        calc (lon1,lat1) (p1.Geometry.Centroid.X, p1.Geometry.Centroid.Y)

    let private distancePoint (p1:Place<_, Geometry>) (p2: Place<_, Geometry>) =
        calc (p1.Geometry.Centroid.X, p1.Geometry.Centroid.Y) (p2.Geometry.Centroid.X, p2.Geometry.Centroid.Y)

    let private distancePos (p2: float*float) (p1:Place<_, Geometry>) =
        calc (p1.Geometry.Centroid.X,p1.Geometry.Centroid.Y) p2

    let private closest (place: Place<_, Geometry>) (count: int) (places: seq<Place<_,_>>) (distance: Place<_,_> -> Place<_,_> -> float) =
        places   
        |> Seq.map ( fun p -> p, distance place p)  
        |> Seq.sortBy snd 
        |> Seq.take count 
        |> Seq.map (fun (x,y) -> {| Facility = x; Distance = y |})

    let filter (place: Place<_, Geometry>) min (places: seq<Place<_,Geometry>>) =
        places   
        |> Seq.map ( fun p -> p, calc (Place.position place) (Place.position p))  
        |> Seq.filter ( fun (p,d) -> d < min)
        |> Seq.map (fun (x,y) -> {| Facility = x; Distance = y |})    
    
    let private closestTo (place: float*float) (count: int) (places: seq<Place<_,_>> ) (distance: float*float -> Place<_,_> -> float) =
        places  
        |> Seq.map ( fun p -> p, distance place p)  
        |> Seq.sortBy snd 
        |> Seq.take count 
        |> Seq.map (fun (x,y) -> {| Facility = x; Distance = y |})

    let nearest (place: Place<_,_>) (count: int) (places: seq<Place<_,_>>) = 
        closest place count places distancePoint

    let nearestN (place: Place<_,_>) (count: int) (places: seq<Place<_,_>>) = 
        closest place count places distancePoint
        |> Seq.map _.Distance
        |> Seq.average
        
    let nearestGeometries (place: Place<_,_>) (count: int) (places: seq<Place<_,_>>) = 
        closest place count places distanceGeometry

    let first<'p,'a> (data: seq<Place<'p,_>>) (area: Place<'a,_>) =
        let n = nearest area 1 data |> Seq.head
        n.Distance

    let firstPlace<'p,'a> (data: seq<Place<'p,_>>) (area: Place<'a,_>) =
        nearest area 1 data |> Seq.head

    let firstTo<'p,'a> (pos:float*float) (data:seq<Place<'p,_>>) =
        let n = closestTo pos 1 data distancePos |> Seq.head
        n.Distance

    let to'<'p,'a> (pos: float*float) (data: Place<'p,_>) =
        distancePos pos data

    let isInBoundary (lon,lat) (feature: IFeature) = 

        let locator = IndexedPointInAreaLocator(feature.Geometry)
        let location = Coordinate(lon, lat) |> locator.Locate
        location = Location.Interior || location = Location.Boundary