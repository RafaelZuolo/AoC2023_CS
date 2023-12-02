namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly IList<string> input;

    private enum Cube
    {
        red,
        green, 
        blue,
    }

    public Day02()
    {
        input = File.ReadAllText(InputFilePath).Split("\r\n", StringSplitOptions.TrimEntries);
    }

    public override ValueTask<string> Solve_1()
    {
        var partialIdSum = (long) 0;

        foreach (var line in input) 
        {
            var currentGameId = ParseGameId(line);
            if (currentGameId == -1) 
            {
                break;
            }

            var currentGame = ParseGame(line);

            var isCurrentGameValid = ValidateGame(currentGame);

            if (isCurrentGameValid)
            {
                partialIdSum += currentGameId;
            }
        }

        return new(partialIdSum.ToString());
    }

    private static bool ValidateGame(IList<IDictionary<Cube, int>> currentGame)
    {
        const long maxRed = 12;
        const long maxGreen = 13;
        const long maxBlue = 14;

        foreach (var game in currentGame)
        {
            if (game.TryGetValue(Cube.red, out var quantity))
            {
                if (quantity > maxRed)
                {
                    return false;
                }
            }

            if (game.TryGetValue(Cube.green, out quantity))
            {
                if (quantity > maxGreen)
                {
                    return false;
                }
            }

            if (game.TryGetValue(Cube.blue, out quantity))
            {
                if (quantity > maxBlue)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static int ParseGameId(string line)
    {
        var words = line.Split(':', StringSplitOptions.TrimEntries);
        if (words.Length < 2) 
        {
            return -1;
        }

        return int.Parse(words[0].Split(' ', StringSplitOptions.TrimEntries)[1]);
    }

    private static IList<IDictionary<Cube, int>> ParseGame(string line)
    {
        var lineWithoutId = line.Split(':', StringSplitOptions.TrimEntries)[1];
        var drawnSets = lineWithoutId.Split(";", StringSplitOptions.TrimEntries);
        var gameList = new List<IDictionary<Cube, int>>();

        foreach (var set in drawnSets)
        {
            gameList.Add(ParseSet(set));
        }

        return gameList;
    }

    private static IDictionary<Cube, int> ParseSet(string set)
    {
        var setByColors = set.Split(",", StringSplitOptions.TrimEntries);
        var colorSet = new Dictionary<Cube, int>();

        foreach (var colorEntry in setByColors)
        {
            var splittedColorEntry = colorEntry.Split(' ', StringSplitOptions.TrimEntries);
            var quantity = splittedColorEntry[0];
            var color = splittedColorEntry[1];

            colorSet.Add(CubeParser(color), int.Parse(quantity));
        }

        return colorSet;
    }

    private static Cube CubeParser(string color)
    {
        if (color == "red")
        {
            return Cube.red;
        }
        else if (color == "green")
        {
            return Cube.green;
        }

        return Cube.blue;        
    }

    public override ValueTask<string> Solve_2()
    {
        

        return new("not solved");
    }
}

