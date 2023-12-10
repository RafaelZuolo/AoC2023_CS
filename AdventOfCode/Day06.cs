namespace AdventOfCode;

internal class Day06 : BaseDay
{
    private IList<string> input;
    private long[] time;
    private long[] distance;
    private long timePart2;
    private long distPart2;
    public Day06()
    {
        input = File.ReadAllText(InputFilePath).Split(
            "\r\n",
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
        time = input.First()
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse)
            .ToArray();
        distance = input.Last()
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse)
            .ToArray();
        timePart2 = input.First()
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Aggregate("", (accumulate, next) => accumulate + next, long.Parse);
        distPart2 = input.Last()
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Aggregate("", (accumulate, next) => accumulate + next, long.Parse);
    }

    public override ValueTask<string> Solve_1()
    {
        var ways = new List<long>();
        for (int i = 0; i < time.Length; i++)
        {
            var currentTime = (double)time[i];
            var currentDist = (double)distance[i];
            
            var root1 = (currentTime - Math.Sqrt(Math.Pow(currentTime, 2) - 4 * currentDist)) / 2;
            var root1Ceil = (long)Math.Ceiling(root1);
            root1Ceil = root1 == root1Ceil ? root1Ceil + 1 : root1Ceil;
            
            var root2 = (currentTime + Math.Sqrt(Math.Pow(currentTime, 2) - 4 * currentDist)) / 2;
            var root2Ceil = (long)Math.Floor(root2);
            root2Ceil = root2 == root2Ceil ? root2Ceil - 1 : root2Ceil;

            ways.Add(root2Ceil - root1Ceil + 1);
        }
        var result = (long)1;
        ways.ForEach(w => result *= w);

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {

        var root1 = (timePart2 - Math.Sqrt(Math.Pow(timePart2, 2) - 4 * distPart2)) / 2;
        var root1Ceil = (long)Math.Ceiling(root1);
        root1Ceil = root1 == root1Ceil ? root1Ceil + 1 : root1Ceil;

        var root2 = (timePart2 + Math.Sqrt(Math.Pow(timePart2, 2) - 4 * distPart2)) / 2;
        var root2Ceil = (long)Math.Floor(root2);
        root2Ceil = root2 == root2Ceil ? root2Ceil - 1 : root2Ceil;

        return new((root2Ceil - root1Ceil + 1).ToString());
    }
}
