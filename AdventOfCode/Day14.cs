namespace AdventOfCode;

internal class Day14 : BaseDay
{
    private IList<string> input;
    private char[,] rocks;

    public Day14()
    {
        input = File.ReadAllText(InputFilePath).Split(
            "\r\n",
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        rocks = new char[input.Count, input.First().Length];
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                rocks[i, j] = input[i][j];
            }
        }
    }
    public override ValueTask<string> Solve_1()
    {
        TiltNorth(rocks);
        var exit = "";
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                exit += rocks[i, j];
            }
            exit += "\r\n";
        }

        return new(CalculateLoad(rocks).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var totalOfCycles = 1000000000;
        var clones = new List<char[,]>();
        for (var i = (long)1; i <= totalOfCycles; i++)
        {
            Cycle(rocks);
            if (clones.Any(r => isEqual(r, rocks)))
            {
                break;
            }
            clones.Add(Clone(rocks));
        }
        var startOfCycle = clones.IndexOf(clones.First(r => isEqual(r, rocks)));
        var cycleSize = clones.Count - startOfCycle;
        var magicIndex = ((totalOfCycles - startOfCycle) % cycleSize) + (startOfCycle - 1);

        return new(CalculateLoad(clones[magicIndex]).ToString());
    }

    private void TiltNorth(char[,] rocks)
    {
        for (var j = 0; j < rocks.GetLength(1); j++)
        {
            var emptySpaceQueue = new Queue<int>();

            for (var i = 0; i < rocks.GetLength(0); i++)
            {
                switch (rocks[i, j])
                {
                    case '#':
                        emptySpaceQueue = new Queue<int>();
                        break;
                    case '.':
                        emptySpaceQueue.Enqueue(i);
                        break;
                    case 'O':
                        TrySwitchRocksColumn(i, j, emptySpaceQueue, rocks);
                        break;
                };
            }
        }
    }

    private void TrySwitchRocksColumn(int i, int j, Queue<int> queue, char[,] rocks)
    {
        if (queue.Count == 0) { return; }

        var emptySpaceAdress = queue.Dequeue();
        rocks[emptySpaceAdress, j] = 'O';
        rocks[i, j] = '.';
        queue.Enqueue(i);
    }

    private void TiltSouth(char[,] rocks)
    {
        var rowSize = rocks.GetLength(1) - 1;
        var columnSize = rocks.GetLength(0) - 1;
        for (var j = 0; j < rocks.GetLength(1); j++)
        {
            var emptySpaceQueue = new Queue<int>();

            for (var i = 0; i < rocks.GetLength(0); i++)
            {
                switch (rocks[columnSize - i, rowSize - j])
                {
                    case '#':
                        emptySpaceQueue = new Queue<int>();
                        break;
                    case '.':
                        emptySpaceQueue.Enqueue(columnSize - i);
                        break;
                    case 'O':
                        TrySwitchRocksColumn(columnSize - i, rowSize - j, emptySpaceQueue, rocks);
                        break;
                };
            }
        }
    }

    private void TiltEast(char[,] rocks)
    {
        for (var i = 0; i < rocks.GetLength(0); i++)
        {
            var emptySpaceQueue = new Queue<int>();

            for (var j = 0; j < rocks.GetLength(1); j++)
            {
                switch (rocks[i, j])
                {
                    case '#':
                        emptySpaceQueue = new Queue<int>();
                        break;
                    case '.':
                        emptySpaceQueue.Enqueue(j);
                        break;
                    case 'O':
                        TrySwitchRocksRow(i, j, emptySpaceQueue, rocks);
                        break;
                };
            }
        }
    }

    private void TiltWest(char[,] rocks)
    {
        var rowSize = rocks.GetLength(1) - 1;
        var columnSize = rocks.GetLength(0) - 1;
        for (var i = 0; i < rocks.GetLength(0); i++)
        {
            var emptySpaceQueue = new Queue<int>();

            for (var j = 0; j < rocks.GetLength(1); j++)
            {
                switch (rocks[columnSize - i, rowSize - j])
                {
                    case '#':
                        emptySpaceQueue = new Queue<int>();
                        break;
                    case '.':
                        emptySpaceQueue.Enqueue(rowSize - j);
                        break;
                    case 'O':
                        TrySwitchRocksRow(columnSize - i, rowSize - j, emptySpaceQueue, rocks);
                        break;
                };
            }
        }
    }

    private void TrySwitchRocksRow(int i, int j, Queue<int> queue, char[,] rocks)
    {
        if (queue.Count == 0) { return; }

        var emptySpaceAdress = queue.Dequeue();
        rocks[i, emptySpaceAdress] = 'O';
        rocks[i, j] = '.';
        queue.Enqueue(j);
    }

    private void Cycle(char[,] rocks)
    {
        TiltNorth(rocks);
        TiltEast(rocks);
        TiltSouth(rocks);
        TiltWest(rocks);
    }

    private bool isEqual(char[,] past, char[,] current)
    {
        for (var i = 0; i < past.GetLength(0); i++)
        {
            for (var j = 0; j < past.GetLength(1); j++)
            {
                if (past[i, j] != current[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private char[,] Clone(char[,] current)
    {
        var clone = new char[current.GetLength(0), current.GetLength(1)];
        for (var i = 0; i < current.GetLength(0); i++)
        {
            for (var j = 0; j < current.GetLength(1); j++)
            {
                clone[i, j] = current[i, j];
            }
        }

        return clone;
    }

    private long CalculateLoad(char[,] rocks)
    {
        var totalLoad = (long)0;
        for (var i = 0; i < rocks.GetLength(0); i++)
        {
            var partialLoad = (long)0;
            for (var j = 0; j < rocks.GetLength(1); j++)
            {
                partialLoad += rocks[i, j] == 'O' ? rocks.GetLength(0) - i : 0;
            }

            totalLoad += partialLoad;
        }

        return totalLoad;
    }
}
