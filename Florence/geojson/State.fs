namespace Florence

type GeoJSONHolder<'GeoJSON> =
    | RawGeoJSON of featureCollection: string
    | TypeGeoJSON of featureCollection: 'GeoJSON

type SpatialData<'GeoJSON> =
    | GeoJSON of featureCollection: GeoJSONHolder<'GeoJSON>
    // Parquet ?

type Geojson = Geojson of string