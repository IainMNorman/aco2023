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

    private int Part1() =>

    // input is all the lines read using readlines earlier
    // Select allows you to loop over all the lines and define how you want to map
    // that line into something else.
    input.Select(line => new // new {} creates a dynamic object
    {
        // Here I am defnining a dynamic property on the thing I'm turning a line into, of Id
        // Id is being extracted from the line string using regex
        // \d means a digit, * means repeat the last match zero or more times, ie any more digits
        // until you get to a : character
        Id = int.Parse(Regex.Match(line, @"(\d*):").Groups[1].Value),
        // The second propery is Hands, each pull from the bag, there are multiple
        // of these per game.
        // [8..] filters the line string from index 8 to .. which is the end, getting rid of the
        // game xx: bit

        Hands = line[8..].Split(";", StringSplitOptions.RemoveEmptyEntries).Select(h => new
        {
            // then I'm spliting on ; to get each hand and using regex again to get the rgb values
            // if they exist. If SafeParseInt is passed and empty string for no match then that results
            // in zero cos of how tryparse works. () in regex creates a group to pull back out
            // \s matches any single whitespace char
            Red = SafeParseInt(Regex.Match(h, @"(\d*)\sred").Groups[1].Value),
            Green = SafeParseInt(Regex.Match(h, @"(\d*)\sgreen").Groups[1].Value),
            Blue = SafeParseInt(Regex.Match(h, @"(\d*)\sblue").Groups[1].Value),
        })
    })
        // this is the end of the select, so output here is the dynamic list of games that have an ID
        // and a collection of hands which each have a Red Green and Blue count.
        // basically all the code so far has just shaped the games into an object we can then query       
        .Where(g =>
        // Where will reduce the new dynamic list of games to only those that are valid
        // There are three conidtionals anded together, one for each of the colours and 
        // .All() will be true if every hand in Hands is true
            g.Hands.All(r => r.Red <= 12) &&
            g.Hands.All(r => r.Green <= 13) &&
            g.Hands.All(r => r.Blue <= 14)
        )
        // this is the end of the Where, so this will be the reduced collection and we can
        // just sum all the ids which is then being returned as it's the last thing in this
        // method body expression
        .Sum(x => x.Id);

    private int Part2() => input.Select(line => new
    {
        Id = int.Parse(Regex.Match(line, @"(\d*):").Groups[1].Value),
        Hands = line[8..].Split(";", StringSplitOptions.RemoveEmptyEntries).Select(h => new
        {
            Red = SafeParseInt(Regex.Match(h, @"(\d*)\sr").Groups[1].Value),
            Green = SafeParseInt(Regex.Match(h, @"(\d*)\sg").Groups[1].Value),
            Blue = SafeParseInt(Regex.Match(h, @"(\d*)\sb").Groups[1].Value),
        })
    }).Sum(x =>
        x.Hands.Max(m => m.Red) *
        x.Hands.Max(m => m.Green) *
        x.Hands.Max(m => m.Blue));

    private int SafeParseInt(string input)
    {
        _ = int.TryParse(input, out int result);
        return result;
    }
}