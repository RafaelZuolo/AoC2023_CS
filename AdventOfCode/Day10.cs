namespace AdventOfCode;

internal class Day10 : BaseDay
{
    private readonly IList<string> input;
    private readonly Node[,] maze;
    private Node start;

    public Day10()
    {
        input = File.ReadAllText(InputFilePath)
            .Split(
                "\r\n",
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        maze = new Node[input.Count, input[0].Length];
        InitializeMaze();
    }

    private void InitializeMaze()
    {
        for (int i = 0; i < input.Count; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                var element = input[i].ElementAt(j);
                maze[i, j] = element == '.' ? null : new(input[i].ElementAt(j));

                if (element == 'S')
                {
                    start = maze[i, j];
                    start.Dist = 0;
                }
            }
        }

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] is null) { continue; }

                if (i > 0 && CanGoUp(maze[i, j])) // case not in upper border
                {
                    maze[i, j].Up = CanGoDown(maze[i - 1, j]) ? maze[i - 1, j] : null;
                }
                if (i < maze.GetLength(0) - 1 && CanGoDown(maze[i, j])) // case not in lower border
                {
                    maze[i, j].Down = CanGoUp(maze[i + 1, j]) ? maze[i + 1, j] : null;
                }
                if (j > 0 && CanGoLeft(maze[i, j])) // case not in left border
                {
                    maze[i, j].Left = CanGoRight(maze[i, j - 1]) ? maze[i, j - 1] : null;
                }
                if (j < maze.GetLength(1) - 1 && CanGoRight(maze[i, j])) // case not in left border
                {
                    maze[i, j].Right = CanGoLeft(maze[i, j + 1]) ? maze[i, j + 1] : null;
                }
            }
        }
    }

    private bool CanGoUp(Node node)
    {
        return node is not null && node.Label switch
        {
            '|' => true,
            'J' => true,
            'L' => true,
            'S' => true,
            _ => false,
        };
    }

    private bool CanGoDown(Node node)
    {
        return node is not null && node.Label switch
        {
            '|' => true,
            'F' => true,
            '7' => true,
            'S' => true,
            _ => false,
        };
    }

    private bool CanGoLeft(Node node)
    {
        return node is not null && node.Label switch
        {
            '-' => true,
            'J' => true,
            '7' => true,
            'S' => true,
            _ => false,
        };
    }

    private bool CanGoRight(Node node)
    {
        return node is not null && node.Label switch
        {
            '-' => true,
            'L' => true,
            'F' => true,
            'S' => true,
            _ => false,
        };
    }

    public override ValueTask<string> Solve_1()
    {
        var foo = FindLoop(start);
        FindDist(start, foo);

        return new(FindMaxDist(start).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new();
    }

    private class Node(char Label)
    {
        public char Label { get; } = Label;

        public Node Up { get; set; }
        public Node Down { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public bool Visited { get; set; } = false;
        public int Flag { get; set; } = 0;
        public int Dist { get; set; } = -1;
    }

    private int FindMaxDist(Node start)
    {
        var maxValue = -1;
        foreach (var node in maze)
        {
            if (node is null) { continue; }

            maxValue = node.Dist > maxValue ? node.Dist : maxValue;
        }

        return maxValue;
    }

    private void FindDist(Node start, int flag)
    {
        foreach (var node in maze)
        {
            if (node is null) { continue; }

            node.Visited = false;
        }

        var queue = new Queue<Node>();
        queue.Enqueue(start);
        BFS(queue, flag);
    }

    private void BFS(Queue<Node> queue, int flag)
    {
        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(out var node))
            {
                return;
            };

            node.Visited = true;

            if (node.Up?.Flag == flag && !node.Up.Visited)
            {
                node.Up.Dist = node.Dist + 1;
                queue.Enqueue(node.Up);
            }
            if (node.Down?.Flag == flag && !node.Down.Visited)
            {
                node.Down.Dist = node.Dist + 1;
                queue.Enqueue(node.Down);
            }
            if (node.Left?.Flag == flag && !node.Left.Visited)
            {
                node.Left.Dist = node.Dist + 1;
                queue.Enqueue(node.Left);
            }
            if (node.Right?.Flag == flag && !node.Right.Visited)
            {
                node.Right.Dist = node.Dist + 1;
                queue.Enqueue(node.Right);
            }
        }
    }

    private int FindLoop(Node start)
    {
        var stack = new Stack<Node>();

        start.Visited = true;
        if (start.Up is not null)
        {
            stack.Push(start.Up);
            DFS(1, stack);
        }
        if (start.Down is not null)
        {
            stack.Push(start.Down);
            DFS(2, stack);
        }
        if (start.Left is not null)
        {
            stack.Push(start.Left);
            DFS(3, stack);
        }
        if (start.Right is not null)
        {
            stack.Push(start.Right);
            DFS(4, stack);
        }

        start.Flag = new Node[] { start.Up, start.Down, start.Left, start.Right }
            .Where(n => n is not null)
            .ToLookup(n => n.Flag)
            .Where(l => l.Count() == 2)
            .Select(l => l.First().Flag)
            .First();

        return start.Flag;
    }

    private void DFS(int flag, Stack<Node> stack)
    {
        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (node.Visited) { continue; }

            node.Visited = true;
            node.Flag = flag;

            if (node.Up is not null) stack.Push(node.Up);
            if (node.Down is not null) stack.Push(node.Down);
            if (node.Left is not null) stack.Push(node.Left);
            if (node.Right is not null) stack.Push(node.Right);
        }
    }
}
