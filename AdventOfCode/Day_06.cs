using System.Text.RegularExpressions;
using BenchmarkDotNet.Engines;
using Spectre.Console;

namespace AdventOfCode;

public class Day_06 : BaseDay
{
    private readonly string[] input;

    public Day_06()
    {
        input = File.ReadAllLines(InputFilePath);

        var test = @"""
                    Time:      7  15   30
                    Distance:  9  40  200
                    """.Split("\n");

        input = test;
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private int Part1()
    {
        var races = new List<(int,int)>{(7,9), (15,40), (30, 200)};
        races = new List<(int,int)> {(40,215),(70,1051),(98,2147),(79,1005)};

        return races.Select(x => Enumerable.Range(0,x.Item1).Select(n => (x.Item1-n)* n).Where(s => s > x.Item2).Count())
            .Aggregate((a,x)=> a*x);
    }

    private int Part2()
    {
        int time = 71530;
        long record = 940200;

        time = 40709879;
        record = 215105121471005;
        return Enumerable.Range(0,time).Select(n => (time-n)* (long)n).Count(s => s > record);           
    }
}