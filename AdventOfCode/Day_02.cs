using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day_02 : BaseDay
{
    private readonly string[] input;

    public Day_02()
    {
        input = File.ReadAllLines(InputFilePath);
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private int Part1()
    {
        var games = input.Select(line => new Game
        {
            Id = int.Parse(Regex.Match(line, @"(\d*):").Groups[1].Value),
            Hands = line.Substring(8).Split(";", StringSplitOptions.RemoveEmptyEntries).Select(h => new Hand
            {
                Red = ParseInt(Regex.Match(h, @"(\d*)\sr").Groups[1].Value),
                Green = ParseInt(Regex.Match(h, @"(\d*)\sg").Groups[1].Value),
                Blue = ParseInt(Regex.Match(h, @"(\d*)\sb").Groups[1].Value),
            }).ToList()
        });

        return games.Where(g =>
            g.Hands.All(r => r.Red <= 12) &&
            g.Hands.All(r => r.Green <= 13) &&
            g.Hands.All(r => r.Blue <= 14))
        .Sum(x => x.Id);
    }

    private int Part2()
    {
        return input.Select(line => new Game
        {
            Id = int.Parse(Regex.Match(line, @"(\d*):").Groups[1].Value),
            Hands = line.Substring(8).Split(";", StringSplitOptions.RemoveEmptyEntries).Select(h => new Hand
            {
                Red = ParseInt(Regex.Match(h, @"(\d*)\sr").Groups[1].Value),
                Green = ParseInt(Regex.Match(h, @"(\d*)\sg").Groups[1].Value),
                Blue = ParseInt(Regex.Match(h, @"(\d*)\sb").Groups[1].Value),
            }).ToList()
        }).Sum(x =>
        x.Hands.Max(m => m.Red) *
        x.Hands.Max(m => m.Green) *
        x.Hands.Max(m => m.Blue));
    }

    private int ParseInt(string input)
    {
        int.TryParse(input, out int result);
        return result;
    }
}

public class Game()
{
    public int Id { get; set; }
    public List<Hand> Hands { get; set; }
}

public class Hand()
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
}
