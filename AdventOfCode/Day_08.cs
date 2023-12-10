using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode;

public class Day_08 : BaseDay
{
    private readonly string[] input;

    public Day_08()
    {
        input = File.ReadAllLines(InputFilePath);

        var test1 =
@"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)";

        var test2 =
@"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)";

        var test3 =
@"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)";

        //input = test3.Split('\n');
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    public int Part1()
    {
        return 0;
        var instructions = input[0].ToCharArray();
        var nodes = input.Skip(2).Select(x => new { Key = x[0..3], Left = x[7..10], Right = x[12..15] });
        var currentKey = "AAA";
        var index = 0;
        var steps = 0;
        while (true)
        {
            var node = nodes.First(x => x.Key == currentKey);
            if (instructions[index] == 'L')
            {
                currentKey = node.Left;
            }
            else
            {
                currentKey = node.Right;
            }

            if (index < instructions.Length - 1)
            {
                index++;
            }
            else { index = 0; }
            if (node.Key == "ZZZ")
            {
                return steps;
            }
            steps++;
        }
    }

    public BigInteger Part2()
    {
        var instructions = input[0].ToCharArray();
        var nodes = input.Skip(2).Select(x => new { Key = x[0..3], Left = x[7..10], Right = x[12..15] }).ToArray();
        var currentKeys = nodes.Where(x => x.Key[^1] == 'A').Select(x => new KeyFirstSecond { Key = x.Key, FirstHit = 0, SecondHit = 0 }).ToArray(); ;
        var index = 0;
        var steps = 0;
        while (true)
        {
            //Console.WriteLine($"#{steps}");

            if (currentKeys.All(x => x.FirstHit > 0 && x.SecondHit > 0))
            {
                return LeastCommonMultiplierCalc.LCM(currentKeys.Select(x => (BigInteger)x.Diff).ToArray());
            }
            var ends = currentKeys.Where(x => x.Key[^1] == 'Z');
            if (ends.Any())
            {
                foreach (var end in ends)
                {
                    if (end.FirstHit == 0)
                    {
                        end.FirstHit = steps;
                    }
                    else if (end.SecondHit == 0)
                    {
                        end.SecondHit = steps;
                        Console.WriteLine($"{end.Key},{end.FirstHit},{end.SecondHit} = {end.SecondHit - end.FirstHit}");
                    }
                }
            }
            var instruction = instructions[index];
            for (int i = 0; i < currentKeys.Length; i++)
            {
                KeyFirstSecond key = currentKeys[i];
                //Console.WriteLine(key.Key);
                var node = nodes.First(x => x.Key == key.Key);
                if (instruction == 'L')
                {
                    key.Key = node.Left;
                }
                else
                {
                    key.Key = node.Right;
                }
            }

            if (index < instructions.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            steps++;
        }
    }
}

public class KeyFirstSecond
{
    public string Key;
    public long FirstHit;
    public long SecondHit;
    public long Diff => SecondHit - FirstHit;
}

public static class LeastCommonMultiplierCalc {
    public static BigInteger LCM(BigInteger[] values) {
        BigInteger retval = values[0];
        for (var i = 1; i < values.Length; i++) {
            retval = BigInteger.GreatestCommonDivisor(retval, values[i]);
        }
        return retval;
    }
}

