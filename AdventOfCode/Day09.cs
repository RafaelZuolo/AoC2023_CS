
namespace AdventOfCode;

internal class Day09 : BaseDay
{
    private IList<string> input;
    private IList<long[]> history;

    public Day09()
    {
        input = File.ReadAllText(InputFilePath).Split(
            "\r\n",
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        history = input.Select(line => line.Split(' ').Select(x => long.Parse(x)).ToArray()).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var extrapolatedValues = history.Select(ExtrapolateValue);

        return new(extrapolatedValues.Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var extrapolatedValues = history.Select(ExtrapolatePreviousValue);

        return new(extrapolatedValues.Sum().ToString());
    }

    private long[] GetDiscreteDerivative(long[] history)
    {
        var derivative = new long[history.Length - 1];
        for (int i = 0; i < history.Length - 1; i++)
        {
            derivative[i] = history[i + 1] - history[i];
        }

        return derivative;
    }

    private long ExtrapolateValue(long[] history)
    {
        var derivativeList = new List<long[]> { history };
        while (!derivativeList.Last().All(value => value == 0)) 
        {
            derivativeList.Add(GetDiscreteDerivative(derivativeList.Last()));
        }

        var extrapolatedValues = new long[derivativeList.Count];

        for (int i = derivativeList.Count - 2; i >= 0; i--)
        {
            var historyToExtrapolate = derivativeList.ElementAt(i);
            extrapolatedValues[i] = extrapolatedValues[i + 1] + historyToExtrapolate.Last();
        }

        return extrapolatedValues[0];
    }

    private long ExtrapolatePreviousValue(long[] history)
    {
        var derivativeList = new List<long[]> { history };
        while (!derivativeList.Last().All(value => value == 0))
        {
            derivativeList.Add(GetDiscreteDerivative(derivativeList.Last()));
        }

        var extrapolatedValues = new long[derivativeList.Count];

        for (int i = derivativeList.Count - 2; i >= 0; i--)
        {
            var historyToExtrapolate = derivativeList.ElementAt(i);
            extrapolatedValues[i] = historyToExtrapolate.First() - extrapolatedValues[i + 1];
        }

        return extrapolatedValues[0];
    }
}
