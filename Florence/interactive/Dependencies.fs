namespace Florence.Interactive

module Dependencies =
    let private nugets = 
        [
            "DistanceTypeProvider"
            "FSharp.Data"
            "Microsoft.DotNet.Interactive, 1.0.0-beta.25177.1"
            "Microsoft.DotNet.Interactive.FSharp, 1.0.0-beta.25177.1"
            "FSharp.Data.LiteralProviders"
        ]

    let private usings =
        [
            "FSharp.Data"
            "Florence"
            "Florence.Ranker"
            "Florence.NetTopologySuite"
            "FSharp.Data.LiteralProviders"
            "Microsoft.DotNet.Interactive.Commands"
            "Microsoft.DotNet.Interactive"
            "Microsoft.DotNet.Interactive.FSharp"
        ]

    let packages =
        ("", nugets) 
        ||> List.fold (fun acc item -> $"""{acc}#r "nuget:{item}"{System.Environment.NewLine}""") 
    let ``open`` =
        ("", usings)
        ||> List.fold (fun acc item -> $"""{acc}open {item}{System.Environment.NewLine}""") 

