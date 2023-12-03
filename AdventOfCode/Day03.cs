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
        var numbers = new List<Number>();
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != '.' && IsNumber(input[i][j]))
                {
                    var lastNumberIndex = GetLastIndexOf(input[i], j);
                    var number = ParseNumber(input[i], j, lastNumberIndex);
                    numbers.Add(new Number(number, i, j, lastNumberIndex));
                    j = lastNumberIndex + 1;
                }
            }
        }

        var partialSum = (long)0;
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '*')
                {
                    partialSum += GearRation(i, j, numbers);
                }
            }
        }

        return new(partialSum.ToString());
    }

    private long GearRation(int i, int j, List<Number> numbers)
    {
        var partialProduct = (long)1;
        var numberOfNeighbours = 0;
        foreach(var number in numbers)
        {
            if (number.I - 1 <= i && i <= number.I + 1
                && number.J - 1 <= j && j <= number.LastNumberIndex + 1)
            {
                if (numberOfNeighbours == 2)
                {
                    return 0;
                }
                numberOfNeighbours++;

                partialProduct *= number.Value;
            }
        }

        if (numberOfNeighbours != 2)
        {
            return 0;
        }

        return partialProduct;
    }

    public record Number(int Value, int I, int J, int LastNumberIndex);   
}
