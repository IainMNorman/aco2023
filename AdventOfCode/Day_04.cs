using System.Text.RegularExpressions;
using CommandLine;

namespace AdventOfCode;

public class Day_04 : BaseDay
{
    private readonly string[] input;

    public Day_04()
    {
        input = File.ReadAllLines(InputFilePath);

        // var testInput = """
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        // Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
        // Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
        // Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
        // Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
        // Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
        // """;
        // input = testInput.Split('\n');
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    private int Part1()
    {
        var stripped = input.Select(x => Regex.Match(x, @"Card\s*\d+:\s(.*)").Groups[1].Value);
        var games = stripped.Select(x => new
        {
            Wins = x.Split('|', StringSplitOptions.RemoveEmptyEntries).First().Split(' ', StringSplitOptions.RemoveEmptyEntries),
            Mine = x.Split('|', StringSplitOptions.RemoveEmptyEntries).Skip(1).First().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        });
        var matches = games.Select(x => x.Wins.Intersect(x.Mine));
        return (int)matches.Select(x => x.Any() ? Math.Pow(2, x.Count() - 1) : 0).Sum();
    }

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private object Part2()
    {
        var stripped = input.Select(x => Regex.Match(x, @"Card\s*\d+:\s(.*)").Groups[1].Value);
        var games = stripped.Select(x => new
        {
            Wins = x.Split('|', StringSplitOptions.RemoveEmptyEntries).First().Split(' ', StringSplitOptions.RemoveEmptyEntries),
            Mine = x.Split('|', StringSplitOptions.RemoveEmptyEntries).Skip(1).First().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        });
        var processedGames = games.Select(x => new Game { MatchCount = x.Wins.Intersect(x.Mine).Count(), CardCount = 1 }).ToArray();

        for (int i = 0; i < processedGames.Length; i++)
        {
            var game = processedGames[i];

            Console.WriteLine($"Card #{i+1}-{game.MatchCount}-{game.CardCount}");
            
            for (int j = 0; j < game.MatchCount; j++)
            {
                if (j+i+1 < processedGames.Length){
                    processedGames[j+i+1].CardCount+= game.CardCount;
                }
            }
        }

        return processedGames.Sum(x => x.CardCount);
    }
}

public class Game() {
    public int MatchCount { get; set; }
    public int CardCount { get; set; }
}