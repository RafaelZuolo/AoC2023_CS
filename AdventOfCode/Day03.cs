namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly IList<string> input;

    public Day03()
    {
        input = File.ReadAllText(InputFilePath).Split("\r\n", StringSplitOptions.TrimEntries);
    }

    public override ValueTask<string> Solve_1()
    {

        var partialSum = (long)0;

        for (var i = 0; i < input.Count; i++)
        {            
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != '.')
                {
                    if ( '0' <= input[i][j] && input[i][j] <= '9')
                    {
                        var lastNumberIndex = GetLastIndexOf(input[i], j);
                        var number = ParseNumber(input[i], j, lastNumberIndex);
                        partialSum += CheckBoundary(i, j, lastNumberIndex, number);
                        j = lastNumberIndex + 1;
                    }
                }
            }
        }

        return new(partialSum.ToString());
    }

    private long CheckBoundary(int i, int j, int lastNumberIndex, int number)
    {
        // upper line
        if (i > 0)
        {
            if (j > 0)
            {
                if (LineHasSymbol(i - 1, j-1, lastNumberIndex))
                {
                    return number;
                }
            } 
            else
            {
                if (LineHasSymbol(i - 1, j, lastNumberIndex))
                {
                    return number;
                }
            }
        }

        // lower line
        if (i < input.Count - 1)
        {
            if (j > 0)
            {
                if (LineHasSymbol(i + 1, j - 1, lastNumberIndex))
                {
                    return number;
                }
            }
            else
            {
                if (LineHasSymbol(i + 1, j, lastNumberIndex))
                {
                    return number;
                }
            }
        }

        // sides
        if (j > 0)
        {
            if (input[i][j - 1] != '.' && !IsNumber(input[i][j - 1])) { return number; }
        }
        if (lastNumberIndex < input[i].Length - 1)
        {
            if (input[i][lastNumberIndex + 1] != '.' && !IsNumber(input[i][lastNumberIndex + 1])) { return number; }
        }

        return 0;
    }

    private bool LineHasSymbol(int i, int j, int lastNumberIndex)
    {
        if (lastNumberIndex < input[0].Length - 1)
        {
            lastNumberIndex++;
        }

        foreach (char c in input[i].Substring(j, lastNumberIndex - j + 1)) 
        { 
            if (c != '.' && !IsNumber(c))
            {
                return true;
            }
        }

        return false;
    }

    private int ParseNumber(string v, int j, int lastNumberIndex)
    {
        return int.Parse(v.Substring(j, lastNumberIndex - j + 1));
    }

    private int GetLastIndexOf(string v, int j)
    {
        var k = j;
        while (k < input[0].Length) 
        { 
            if (!IsNumber(v[k]))
            {
                return k - 1;
            }

            k++;
        }

        return k - 1;
    }

    private bool IsNumber(char v)
    {
        return '0' <= v && v <= '9';
    }

    public override ValueTask<string> Solve_2()
    {
        return new("Waiting to be solved");
    }
}
