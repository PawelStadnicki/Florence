namespace Florence

open MathNet.Numerics.Statistics

    module Points =
        let points (maxDist: float) (maxPoint: float ) (actual: float) =

            let meters = actual
            let max = maxDist
            let count = maxPoint

            count - Statistics.QuantileRank([1. .. max], meters) * count
