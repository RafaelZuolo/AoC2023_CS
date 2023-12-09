namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly IList<string> input;
    private readonly char[] instructions;
    private readonly IList<Node> nodes;

    public Day08()
    {
        input = File.ReadAllText(InputFilePath).Split("\r\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        instructions = input.First().ToCharArray();
        nodes = input.Skip(1).Select(ParseNode).ToList();

        foreach (var node in nodes)
        {
            node.Left = nodes.First(n => n.Label == node.LeftLabel);
            node.Right = nodes.First(n => n.Label == node.RightLabel);
        }
    }

    private Node ParseNode(string line)
    {
        var lineSplit = line.Split('=', StringSplitOptions.TrimEntries);
        var label = lineSplit[0];
        var leftlabel = lineSplit[1].Substring(1, 3);
        var rightlabel = lineSplit[1].Substring(6, 3);

        return new Node(label, leftlabel, rightlabel);
    }

    public override ValueTask<string> Solve_1()
    {
        var numberOfSteps = 0;
        var currentStep = 0;
        var currentnode = nodes.First(n => n.Label == "AAA");

        while (currentnode.Label != "ZZZ")
        {
            numberOfSteps++;
            var step = instructions[currentStep++];

            if (step == 'L')
            {
                currentnode = currentnode.Left;
            }
            else
            {
                currentnode = currentnode.Right;
            }

            if (currentStep == instructions.Length)
            {
                currentStep = 0;
            }
        }

        return new ValueTask<string>(numberOfSteps.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var numberOfSteps = 0;
        var currentStep = 0;
        var currentNodes = nodes.Where(n => n.Label.EndsWith('A')).ToArray();
        var distancesToZ = currentNodes.Select(n => 0).ToArray();

        while (AllDistancesAreSet(distancesToZ))
        {
            numberOfSteps++;
            var step = instructions[currentStep++];

            if (step == 'L')
            {
                currentNodes = currentNodes.Select(n => n.Left).ToArray();
            }
            else
            {
                currentNodes = currentNodes.Select(n => n.Right).ToArray();
            }

            SetDistancesToZ(numberOfSteps, currentNodes, distancesToZ);

            if (currentStep == instructions.Length)
            {
                currentStep = 0;
            }
        }

        return new(CalculateMMC(distancesToZ).ToString());
    }

    private long CalculateMMC(int[] distancesToZ)
    {
        if (distancesToZ.Length > 2)
        {
            return CalculateMMC(distancesToZ[0], CalculateMMC(distancesToZ.Skip(1).ToArray()));
        }

        return CalculateMMC(distancesToZ[0], distancesToZ[1]);
    }

    private long CalculateMMC(long v1, long v2)
    {
        var mdc = CalculateMDC([v1, v2]);

        return v1 * v2 / mdc;
    }

    private long CalculateMDC(long[] distancesToZ)
    {
        if (distancesToZ.Length > 2)
        {
            return CalculateMDC(distancesToZ[0], CalculateMDC(distancesToZ.Skip(1).ToArray()));
        }

        return CalculateMDC(distancesToZ[0], distancesToZ[1]);
    }

    private long CalculateMDC(long x, long y)
    {
        while (x != 0)
        {
            var w = x;
            x = y % x;
            y = w;
        }

        return y;
    }

    private void SetDistancesToZ(int numberOfSteps, Node[] currentNodes, int[] distancesToZ)
    {
        for (int i = 0; i < currentNodes.Length; i++)
        {
            distancesToZ[i] = distancesToZ[i] != 0 
                ? distancesToZ[i] 
                : currentNodes[i].Label.EndsWith('Z') ? numberOfSteps : 0;
        }
    }

    private bool AllDistancesAreSet(int[] distancesToZ)
    {
        return !distancesToZ.All(d => d != 0);
    }

    private record Node(string Label, string LeftLabel, string RightLabel)
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}
