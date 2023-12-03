using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Xml.Serialization;
using Iced.Intel;
using Microsoft.Diagnostics.Runtime.Utilities;

namespace AdventOfCode;

public class Day_03 : BaseDay
{
    private readonly string input;
    private string stripped;
    private int height;
    private int width;

    public Day_03() => input = File.ReadAllText(InputFilePath);
    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private int Part1()
    {
        Setup();

        var total = 0;

        foreach (var match in Regex.Matches(stripped, @"\d+").Cast<Match>())
        {
            var startCoords = GetCoords(match.Index);

            for (int y = startCoords.y - 1; y <= startCoords.y + 1; y++)
            {
                for (int x = startCoords.x - 1; x <= startCoords.x + match.Length; x++)
                {
                    if (GetCellType(GetCellValue(x, y)) == CellType.Symbol)
                    {
                        total += int.Parse(match.Value);
                    }
                }
            }
        }

        return total;
    }

    private int Part2()
    {
        Setup();

        var numbersWithGears = new List<(int x, int y, int numberValue)>();

        foreach (var match in Regex.Matches(stripped, @"\d+").Cast<Match>())
        {
            var startCoords = GetCoords(match.Index);

            for (int y = startCoords.y - 1; y <= startCoords.y + 1; y++)
            {
                for (int x = startCoords.x - 1; x <= startCoords.x + match.Length; x++)
                {
                    if (GetCellValue(x, y) == '*')
                    {
                        numbersWithGears.Add((x, y, int.Parse(match.Value)));
                    }
                }
            }
        }

        return numbersWithGears
            .GroupBy(gear => new { gear.x, gear.y })
            .Where(g => g.Count() == 2)
            .Sum(g => g.First().numberValue * g.Last().numberValue); ;
    }

    private void Setup()
    {
        height = input.ToCharArray().Count(x => x == '\n') + 1;
        width = input.IndexOf("\n");
        stripped = input.Replace("\n", "");
    }

    private char GetCellValue(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return '.';
        return stripped[x + (y * width)];
    }

    private static CellType GetCellType(char cell) => cell switch
    {
        '.' => CellType.Empty,
        (>= '0') and (<= '9') => CellType.NumberPart,
        _ => CellType.Symbol,
    };

    public (int x, int y) GetCoords(int index)
    {
        int y = index / height;
        int x = index - (y * height);
        return (x: x, y: y);
    }
}

public enum CellType
{
    Empty,
    Symbol,
    NumberPart
}