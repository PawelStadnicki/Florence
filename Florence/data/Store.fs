namespace Florence

module Store =

    open System.IO
    open System.Reflection
    
    let private readEmbeddedResource (resourceName: string) =
        let assembly = Assembly.GetExecutingAssembly()
        use stream = assembly.GetManifestResourceStream($"Florence.data.{resourceName}")
        if isNull stream then
            failwithf $"Resource not found: %s{resourceName}"
        use reader = new StreamReader(stream)
        reader.ReadToEnd()
        
    let tram_fermate =
        readEmbeddedResource "tram_fermate.geojson"
    let sez_censimento2011 =
        readEmbeddedResource "sez_censimento2011Polygon.geojson"
        
    let cento_luoghi =
        readEmbeddedResource "cento_luoghiPoint.geojson"
    let famousPeople =
        readEmbeddedResource "famous.geojson"
        
    module En =
        let tramStations = tram_fermate
        let census2011Polygon = sez_censimento2011
        let one_hundred_places = cento_luoghi
