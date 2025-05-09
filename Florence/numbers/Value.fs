namespace Florence

type Value =
    | Number of Number
    | Description of string
    | Stars of int

and Number =
    | Rank of float  // 0.0 .. 1.0 linear values
    | Ratio of float // 0.0 .. 1.0 real values
    | Arbitrary of float