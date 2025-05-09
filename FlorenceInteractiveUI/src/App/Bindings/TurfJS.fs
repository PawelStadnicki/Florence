namespace App

open Fable.Core
open Geojson

[<RequireQualifiedAccess>]
module TurfJS =
    /// <summary>
    /// Rewind (Multi)LineString or (Multi)Polygon outer ring counterclockwise and inner rings clockwise (Uses Shoelace Formula).
    /// </summary>
    [<ImportDefault("@turf/rewind")>]
    let rewind(_poly, _opt) = jsNative
    
    [<ImportDefault("@turf/polygon-to-line")>]
    let polygonToLine(_f: Feature<Polygon, _>): Feature<LineString, _> = jsNative
    
    [<Import("quantile", "simple-statistics")>]
    let quantile'(_data: ResizeArray<float>, _q: ResizeArray<float> ): float [] = jsNative

    let quantile(data: float [], q: float [] ): float [] = quantile' (ResizeArray<float>(data), ResizeArray<float>(q))

    [<Import("center", "@turf/center")>]
    let center(_fc: FeatureCollection<GeoJsonGeometryTypes,_>): Feature<Point,_> = jsNative

    [<ImportDefault("@turf/bbox")>]
    let bbox<'T,'P>(_fc: FeatureCollection<'T,'P>): float[] = jsNative
    
    let first (fc: FeatureCollection<_,_>) = fc.features.[0]
    
    [<Import("point", "@turf/helpers")>]
    let point(_p: ResizeArray<float>): Feature<Point, _> = jsNative
    
    [<Import("degreesToRadians", "@turf/helpers")>]
    let degreesToRadians _number: float<rad> = jsNative
    
    [<Import("radiansToDegrees", "@turf/helpers")>]
    let radianToDegrees (_number: float<rad>): float<deg> = jsNative
    
    [<Import("lineString", "@turf/helpers")>]
    let lineString(_p: float[][]): Feature<LineString, _> = jsNative
    
    [<Import("lineString", "@turf/helpers")>]
    let lineStringProps(_p: float[][], _props: obj): Feature<LineString, _> = jsNative
    
    [<Import("multiLineString", "@turf/helpers")>]
    let multiLineStringProps(_p: float[][][], _props: obj): Feature<MultiLineString, _> = jsNative
    
    [<ImportDefault("@turf/distance")>]
    let distancePoints(_p1: Feature<Point, _>, _p2:  Feature<Point, _>): float<km> = jsNative

    let distance ((p1a, p1b): float * float) ((p2a, p2b): float * float): float<km> = distancePoints(point (ResizeArray[|p1a ; p1b |]), point (ResizeArray[|p2a ; p2b |]))

    [<ImportDefault("@turf/midpoint")>]
    let midpointPoints(_p1: Feature<Point, _>, _p2:  Feature<Point, _>): Feature<Point, _> = jsNative
    
    [<Import("combine", "@turf/combine")>]
    let combine(_fc: FeatureCollection<Polygon,_>): FeatureCollection<MultiPolygon,_> = jsNative
    let midpoint ((p1a, p1b): float * float) ((p2a, p2b): float * float): Feature<Point, _> = midpointPoints(point (ResizeArray[|p1a ; p1b |]), point (ResizeArray[|p2a ; p2b |]))

    [<ImportDefault("@turf/bearing")>]
    let bearingPoints(_p1: Feature<Point, _>, _p2:  Feature<Point, _>): float<deg> = jsNative

    let bearing (x1: float, y1: float) (x2: float, y2: float): float<deg> =
        bearingPoints(point (ResizeArray [| x1;y1 |]), point (ResizeArray [| x2;y2 |]))

    /// <summary>
    /// Takes a line, a start Point, and a stop point and returns a subsection of the line in-between those points. The start & stop points don't need to fall exactly on the line.
    /// This can be useful for extracting only the part of a route between waypoints.
    /// </summary>
    [<Import("pointGrid","@turf/point-grid")>]
    let pointGrid(_bbox: float[], _cellSize: float<km>): FeatureCollection<Point,_> = jsNative

    [<Import("randomPoint","@turf/random")>]
    let randomPoint(_nr: int, _opt: {| bbox: float[] |}): FeatureCollection<Point,_> = jsNative

    [<Import("lineSlice","@turf/line-slice")>]
    let lienSlice(_start: Feature<Point, _>, _stop:Feature<Point, _>, _line: Feature<LineString,_>): Feature<LineString,_> = jsNative


    [<ImportDefault("@turf/rhumb-destination")>]
    let rhumbDestinationPoint(_start: Feature<Point, _>, _distance: float<km>, _bearing: float<deg>): Feature<Point, _> = jsNative

    let rhumbDestination((s1,s2): float*float, distance: float<km>, bearing: float<deg>): Feature<Point, _> = rhumbDestinationPoint(point (ResizeArray[|s1 ; s2 |]), distance, bearing)
    /// <summary>
    /// Takes a ring and return true or false whether or not the ring is clockwise or counter-clockwise.
    /// </summary>
    [<ImportDefault("@turf/boolean-clockwise")>]
    let booleanClockwise(_fc: Feature<_,_>): bool = jsNative

    [<Import("bezierSpline", "@turf/bezier-spline")>]
    let bezierSpline(_fc: Feature<LineString,_>, _opts: obj): Feature<LineString,_> = jsNative

    [<Import("lineSliceAlong", "@turf/line-slice-along")>]
    let lineSliceAlong(_fc: Feature<LineString,_>, _start: float<km>, _stop: float<km>): Feature<LineString,_> = jsNative

    [<Import("length", "@turf/length")>]
    let length(_fc: Feature<LineString,_>): float<km>  = jsNative

    [<Import("nearestPointOnLine ", "@turf/nearest-point-on-line")>]
    let nearestPointOnLine (_fc: Feature<LineString,_>, _p: Feature<Point, _>): Feature<Point, _>  = jsNative

    [<Import("pointOnFeature", "@turf/point-on-feature")>]
    let pointOnFeature (_fc: Feature<_,_>): Feature<Point, _>  = jsNative

    [<Import("nearestPoint ", "@turf/nearest-point")>]
    let nearestPoint (_source: Feature<Point, _>, _target: FeatureCollection<Point,_>): Feature<Point, _>  = jsNative

    [<Import("lineChunk ", "@turf/line-chunk")>]
    let lineChunk (_fc: Feature<LineString,_>, _chunks: int): FeatureCollection<LineString, _>  = jsNative

    [<Import("along ", "@turf/along")>]
    let along (_fc: Feature<LineString,_>, _dist: float): Feature<Point, _>  = jsNative

    [<Import("clustersKmeans", "@turf/clusters-kmeans")>]
    let clustersKmeans (_points: Feature<Point,_> [], _opts: obj): Feature<Point, _> [] = jsNative

    [<Import("booleanPointInPolygon", "@turf/boolean-point-in-polygon")>]
    let booleanPointInPolygon(_p: Position, _poly: Feature<Polygon,_>): bool = jsNative