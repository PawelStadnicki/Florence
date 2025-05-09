namespace App

type Msg =
    | Init
    | PositionChange of longitude: float * lat: float
    | NameChange of change: string
    | GroupNameChange of change: string
    | AddPerson
    | LoadSampleData
    | Continue

    
