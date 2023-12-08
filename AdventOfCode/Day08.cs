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
        return new ValueTask<string>();
    }

    private record Node(string Label, string LeftLabel, string RightLabel)
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}
