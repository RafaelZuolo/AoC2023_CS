namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly IList<string> input;

    public Day01()
    {
        input = File.ReadAllText(InputFilePath).Split("\r\n", StringSplitOptions.TrimEntries);
    }

    public override ValueTask<string> Solve_1()
    {
        var cummulativeSum = 0;

        foreach (var line in input)
        {
            var first = "";
            var second = "";

            foreach (var letter in line)
            {
                if ('0' <= letter && letter <= '9' && first == "")
                {
                    first = letter.ToString();
                }
                if ('0' <= letter && letter <= '9')
                {
                    second = letter.ToString();
                }
            }

            if (first != "")
            {
                cummulativeSum += int.Parse(first + second);
            }
        }

        return new(cummulativeSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var cummulativeSum = 0;
        var lookUp = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        foreach (var line in input)
        {
            var first = "";
            var second = "";
            var partialLine = "";

            foreach (var letter in line)
            {
                partialLine += letter;

                foreach (var key in lookUp.Keys)
                {
                    if ('0' <= letter && letter <= '9')
                    {
                        first = first == "" ? letter.ToString() : first;
                        second = letter.ToString();
                    }
                    else if (partialLine.EndsWith(key))
                    {
                        first = first == "" ? lookUp[key].ToString() : first;
                        second = lookUp[key].ToString();
                    }
                }
            }


            if (first != "")
            {
                cummulativeSum += int.Parse(first + second);
            }
        }

        return new(cummulativeSum.ToString());
    }
}
