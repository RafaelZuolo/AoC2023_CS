namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly IList<string> input;
    private readonly IList<CamelHand> hands;

    public Day07()
    {
        input = File.ReadAllText(InputFilePath).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        hands = input.Select(line =>
        {
            var lineSplitted = line.Split(' ');
            return new CamelHand(lineSplitted[0].ToCharArray(), long.Parse(lineSplitted[1]));
        }).ToList();
    }

    internal enum HandType
    {
        FiveOfAKind = 6,
        FourOfAKind = 5,
        FullHouse = 4,
        ThreeOfAKind = 3,
        TwoPair = 2,
        OnePair = 1,
        HighCard = 0,
    }

    private class CamelHand(char[] hand, long bid) : IComparable<CamelHand>, IEquatable<CamelHand>
    {
        public char[] Hand { get; set; } = hand;
        public long Bid { get; set; } = bid;

        public virtual HandType GetHandType()
        {
            if (IsFiveOfAKind()) { return HandType.FiveOfAKind; }
            if (IsFourOfAKind()) { return HandType.FourOfAKind; }
            if (IsFullHouse()) { return HandType.FullHouse; }
            if (IsThreeOfAKind()) { return HandType.ThreeOfAKind; }
            if (IsTwoPair()) { return HandType.TwoPair; }
            if (IsOnePair()) { return HandType.OnePair; }
            return HandType.HighCard;
        }

        public bool IsFiveOfAKind()
        {
            return Hand.Distinct().Count() == 1;
        }

        public bool IsFourOfAKind()
        {
            var equalToFirst = Hand.Count(c => c == Hand[0]);
            var notEqualToFirst = Hand.Count(c => c != Hand[0]);

            var t = Hand.Distinct().Count() == 2
                && (equalToFirst == 4 || notEqualToFirst == 4);

            return t;
        }

        public bool IsFullHouse()
        {
            var isTwoSet = Hand.Distinct().Count() == 2;
            return isTwoSet && !IsFourOfAKind();
        }

        public bool IsThreeOfAKind()
        {
            var sortedHand = Hand.Order().ToArray();
            var sizeOfEqualToMiddle = Hand.Where(c => c == sortedHand[2]).Count();

            return sizeOfEqualToMiddle == 3 && Hand.Distinct().Count() == 3;
        }

        public bool IsTwoPair()
        {
            var sortedHand = Hand.Order().ToArray();
            var sizeOfEqualToFirst = Hand.Where(c => c == sortedHand[0]).Count();
            var sizeOfEqualToMiddle = Hand.Where(c => c == sortedHand[2]).Count();
            var sizeOfEqualToLast = Hand.Where(c => c == sortedHand[4]).Count();

            return sizeOfEqualToFirst == 2 && sizeOfEqualToMiddle == 2 && sizeOfEqualToLast == 1
                || sizeOfEqualToFirst == 2 && sizeOfEqualToMiddle == 1 && sizeOfEqualToLast == 2
                || sizeOfEqualToFirst == 1 && sizeOfEqualToMiddle == 2 && sizeOfEqualToLast == 2;
        }

        public bool IsOnePair()
        {
            return Hand.Distinct().Count() == 4;
        }

        public bool IsHighCard()
        {
            return Hand.Distinct().Count() == 5;
        }

        public int CompareTo(CamelHand other)
        {
            var typeOther = other.GetHandType();
            var typeThis = GetHandType();
            var result = typeThis.CompareTo(typeOther);

            if (result != 0)
            {
                return result;
            }

            for (var i = 0; i < Hand.Count(); i++)
            {
                if (Hand[i] != other.Hand[i])
                {
                    return CompareCard(Hand[i], other.Hand[i]);
                }
            }

            return 0;
        }

        internal virtual int CompareCard(char a, char b)
        {
            if (a <= '9' || b <= '9')
            {
                return a.CompareTo(b);
            }

            return MapCharToInt(a).CompareTo(MapCharToInt(b));
        }

        private int MapCharToInt(char a)
        {
            return a switch
            {
                'T' => 0,
                'J' => 1,
                'Q' => 2,
                'K' => 3,
                'A' => 4,
                _ => -1,
            };
        }

        public bool Equals(CamelHand other)
        {
            if (other == null)
            {
                return false;
            }

            return Hand.SequenceEqual(other.Hand);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CamelHand);
        }

        public override int GetHashCode()
        {
            return Hand.GetHashCode();
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var types = hands.Select(h => h.GetHandType()).ToList();
        var orderedHands = hands.Order().ToArray();

        var result = (long)0;

        for (var i = 0; i < orderedHands.Length; i++)
        {
            var partial = orderedHands[i].Bid * (i + 1);
            result += partial;
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var jokerHands = hands.Select(h => new JokerCamelHand(h.Hand, h.Bid)).ToArray();
        var test = jokerHands[1].CompareTo(jokerHands[3]);
        var orderedHands = jokerHands.Order().ToArray();

        var result = (long)0;


        for (var i = 0; i < orderedHands.Length; i++)
        {
            var partial = orderedHands[i].Bid * (i + 1);
            result += partial;
        }

        return new(result.ToString());
    }

    private class JokerCamelHand(char[] hand, long bid) : CamelHand(hand, bid)
    {
        public int CompareTo(JokerCamelHand other)
        {
            var typeOther = other.GetHandType();
            var typeThis = GetHandType();
            var result = typeThis.CompareTo(typeOther);

            if (result != 0)
            {
                return result;
            }

            for (var i = 0; i < Hand.Length; i++)
            {
                if (Hand[i] != other.Hand[i])
                {
                    return CompareCard(Hand[i], other.Hand[i]);
                }
            }

            return 0;
        }

        public override HandType GetHandType()
        {
            var baseType = base.GetHandType();
            if (!Hand.Contains('J') || baseType == HandType.FiveOfAKind)
            {
                return baseType;
            }

            var highestCard = '-';
            var highestCardCount = 0;
            foreach (var card in Hand)
            {
                var partialCount = Hand.Count(c => c == card);
                if (partialCount > highestCardCount && card != 'J')
                {
                    highestCard = card;
                    highestCardCount = partialCount;
                }
            }

            var notJokerHand = Hand.Select(c => c == 'J' ? highestCard : c).ToArray();

            return new CamelHand(notJokerHand, Bid).GetHandType();
        }

        internal override int CompareCard(char a, char b)
        {
            return MapCharToIntModified(a).CompareTo(MapCharToIntModified(b));
        }

        private int MapCharToIntModified(char a) => a switch
        {
            'J' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'Q' => 11,
            'K' => 12,
            'A' => 13,
            _ => -1
        };
    }
}
