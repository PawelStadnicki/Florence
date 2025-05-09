namespace Florence

type Area =
    | Block
    | District
    | Statistical
    | Urban
    | Neighborhood
    | ZipCode  //can overlap 
    | Other of name: string

type Location<'p, 'g> =
    | Coordinates of Coordinates
    | Index of LocationIndex
    | Block of Place<'p, 'g> * Area 
    | Street of name: string * geo: Place<'p, 'g>
    | Address of address: Address * Coordinates