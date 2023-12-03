using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day_04 : BaseDay
{
    private readonly string[] input;

    public Day_04()
    {
        input = File.ReadAllLines(InputFilePath);
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new();

    [Benchmark]
    public override ValueTask<string> Solve_2() => new();
}