namespace AdventOfCode;

public class Day_01 : BaseDay
{
    private readonly string[] input;
    private readonly List<string> numbers;

    public Day_01()
    {
        input = File.ReadAllLines(InputFilePath);

        numbers = new List<string> {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    [Benchmark]    
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private int Part1() => input
        .Select(x => x.ToCharArray().Where(c => c <= 0x39))
        .Select(x => 10 * (x.First()-0x30) + (x.Last()-0x30))
        .Sum();

    private int Part2() => input
        .Select(x => FixLine(x).ToCharArray().Where(c => c <= 0x39))
        .Select(x => 10 * (x.First()-0x30) + (x.Last()-0x30))
        .Sum();

    private string FixLine(string line)
    {
        line = line.Replace("oneight", "oneeight");
        line = line.Replace("twone", "twoone");
        line = line.Replace("threeight", "threeeight");
        line = line.Replace("fiveight", "fiveeight");
        line = line.Replace("sevenine", "sevennine");
        line = line.Replace("eightwo", "eighttwo");
        line = line.Replace("eighthree", "eightthree");
        line = line.Replace("nineight", "nineeight");

        numbers.ForEach(x => line = line.Replace(x,WordToNumber(x)));

        return line;
    }

    private string WordToNumber(string input) => 
        Array.IndexOf(numbers.ToArray(), input).ToString();
}
