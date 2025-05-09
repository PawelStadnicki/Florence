namespace App

open Fable.Core
type Person = { Name: string; Position: float * float }

type State = {
    Position: (float * float) option
    People: Person list
    Name: string
    GroupName: string
    Autocomplete: obj
}

module People =
    let serialize (people: Person list) =
        {|
           ``type`` = "FeatureCollection"
           features =
                people
                |> List.toArray
                |> Array.map ( fun person ->
                    let lon, lat = person.Position
                    {|
                        ``type`` = "Feature"
                        properties =
                            {|
                                name = person.Name
                            |}
                        geometry =
                            {|
                                ``type`` = "Point"
                                coordinates = [ lon; lat ]
                            |}
                    |}
                )
        |} |> JS.JSON.stringify
