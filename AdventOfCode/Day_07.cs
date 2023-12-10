using System.Text.RegularExpressions;
using Perfolizer.Mathematics.Distributions;

namespace AdventOfCode;

public class Day_07 : BaseDay
{
    private readonly string[] input;

    public Day_07()
    {
        input = File.ReadAllLines(InputFilePath);

        var test = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483".Split('\n', StringSplitOptions.RemoveEmptyEntries);
        //input = test;
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    private int Part1()
    {
        return input.Select(x => new { Hand = x.Split(' ')[0].Replace('A', 'Z').Replace('T', 'B').Replace('K', 'R'), Bid = int.Parse(x.Split(' ')[1]) })
            .OrderBy(x => PokerScore(x.Hand))
            .ThenBy(x => x.Hand)
            .Select((x, i) => x.Bid * (i + 1))
            .Sum();
    }

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private int Part2()
    {
        return input.Select(x => new { Hand = x.Split(' ')[0].Replace('J', '.').Replace('A', 'Z').Replace('T', 'B').Replace('K', 'R'), Bid = int.Parse(x.Split(' ')[1]) })
            .OrderBy(x => PokerScoreWithJokers(x.Hand))
            .ThenBy(x => x.Hand)
            .Select((x, i) => x.Bid * (i + 1))
            .Sum();
    }

    private int PokerScore(string hand)
    {
        return hand switch
        {
            var h when IsFiveOfAKind(h) => 7,
            var h when IsFourOfAKind(h) => 6,
            var h when IsFullHouse(h) => 5,
            var h when IsThreeOfAKind(h) => 4,
            var h when IsTwoPair(h) => 3,
            var h when IsOnePair(h) => 2,
            var h when IsHighCard(h) => 1,
            _ => throw new NotImplementedException()
        };
    }

    private int PokerScoreWithJokers(string hand)
    {
        var best = 0;
        foreach (var card in new char[] { '2', '3', '4', '5', '6', '7', '8', '9', 'B', 'Q', 'R', 'Z' })
        {
            var score = PokerScore(hand.Replace('.', card));
            if (score > best)
            {
                best = score;
            }
        }

        return best;
    }

    private bool IsFiveOfAKind(string hand)
    {
        return hand.ToCharArray().ToList().Distinct().Count() == 1;
    }

    private bool IsFourOfAKind(string hand)
    {
        return hand.ToCharArray().GroupBy(x => x).OrderBy(x => x.Count()).Last().Count() == 4;
    }

    private bool IsFullHouse(string hand)
    {
        return hand.ToCharArray()
            .GroupBy(x => x)
            .OrderBy(x => x.Count())
            .First()
            .Count() == 2

            &&

            hand.ToCharArray()
            .GroupBy(x => x)
            .OrderBy(x => x.Count())
            .Skip(1)
            .First()
            .Count() == 3;
    }

    private bool IsThreeOfAKind(string hand)
    {
        return hand.ToCharArray().GroupBy(x => x).OrderBy(x => x.Count()).Last().Count() == 3;
    }

    private bool IsTwoPair(string hand)
    {
        return hand.ToCharArray().ToList().Distinct().Count() == 3;
    }

    private bool IsOnePair(string hand)
    {
        return hand.ToCharArray().ToList().Distinct().Count() == 4;
    }

    private bool IsHighCard(string hand)
    {
        return hand.ToCharArray().ToList().Distinct().Count() == 5;
    }


}