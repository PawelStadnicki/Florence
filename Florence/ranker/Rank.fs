namespace Florence

type Ranks =
    {
        Score: float   // the pure result of a rank function
        Rank: float    // quantile rank (percentile of the normal distribution), from .0 to 1.0
        Ratio: float   // rank function score divided by its maximum result       
        Position: int  // ordered place among all features being ranked (from 1 to n)
    }

type RankedProperties<'p> =
    {
        Properties: 'p
        Ranks: Ranks
    }

type RankedPlace<'p,'g> = Place<RankedProperties<'p>, 'g>
