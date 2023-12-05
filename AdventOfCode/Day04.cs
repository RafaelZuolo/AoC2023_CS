namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly IList<string> input;
    private readonly IList<Card> cards;
    private readonly IDictionary<int, Card> cardsById;

    public Day04()
    {
        cards = new List<Card>();
        cardsById = new Dictionary<int, Card>();
        input = File.ReadAllText(InputFilePath).Split("\r\n", StringSplitOptions.TrimEntries);

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) // last line
            {
                break;
            }

            var idSplit = line.Split(':');
            var id = int.Parse(idSplit[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);

            var numberSplit = idSplit[1].Split("|");
            var winningSplit = numberSplit[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var drawnSplit = numberSplit[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var card = new Card(
                id,
                winningSplit.Select(int.Parse).ToList(),
                drawnSplit.Select(int.Parse).ToList());

            cards.Add(card);
            cardsById.Add(id, card);
        }
    }

    public record Card(int Id, IList<int> Winning, IList<int> Drawn);

    public override ValueTask<string> Solve_1()
    {
        var totalPoints = cards.Select(GetCardPoints).Sum();
        
        return new(totalPoints.ToString());
    }

    private long GetCardPoints(Card card)
    {
        var numberOfWinningNumbers = NumberOfWinningNumbers(card);

        return numberOfWinningNumbers == 0 
            ? 0 
            : (long)Math.Pow(2, numberOfWinningNumbers - 1);
    }

    private long NumberOfWinningNumbers(Card card)
    {
        var numberOfWinningNumbers = card.Winning.Intersect(card.Drawn).Count();

        return numberOfWinningNumbers;
    }

    public override ValueTask<string> Solve_2()
    {
        var numberOfCardCopies = new Dictionary<Card, long>();
        foreach (var card in cards)
        {
            numberOfCardCopies.Add(card, 1);
        }

        foreach (var card in cards)
        {
            var cardValue = NumberOfWinningNumbers(card);
            for (var i = 1; i <= cardValue; i++)
            {
                var nextCardId = i + card.Id;
                if (nextCardId > cards.Count)
                {
                    break;
                }

                // each copy of the current card adds exactly one copy
                numberOfCardCopies[cardsById[nextCardId]] += numberOfCardCopies[card];
            }
        }

        return new(numberOfCardCopies.Values.Sum().ToString());
    }
}
