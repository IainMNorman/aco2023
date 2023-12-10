using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Perfolizer.Horology;

namespace AdventOfCode;

public class Day_05 : BaseDay
{
    private readonly string[] input;

    public Day_05()
    {
        input = File.ReadAllLines(InputFilePath);

        var test =
            """
            seeds: 79 14 55 13

            seed-to-soil map:
            50 98 2
            52 50 48

            soil-to-fertilizer map:
            0 15 37
            37 52 2
            39 0 15

            fertilizer-to-water map:
            49 53 8
            0 11 42
            42 0 7
            57 7 4

            water-to-light map:
            88 18 7
            18 25 70

            light-to-temperature map:
            45 77 23
            81 45 19
            68 64 13

            temperature-to-humidity map:
            0 69 1
            1 0 69

            humidity-to-location map:
            60 56 37
            56 93 4
            
            """;

        //input = test.Split('\n');
    }

    [Benchmark]
    public override ValueTask<string> Solve_1() => new(Part1().ToString());

    private long Part1()
    {
        var seeds = input.First()[6..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Seed { Number = long.Parse(x) })
            .ToList();

        List<Map> maps = [];

        Map map = new();

        var mapIndex = 0;

        foreach (var line in input.Skip(2))
        {
            if (string.IsNullOrEmpty(line))
            {
                maps.Add(map);
            }
            else if (line.Contains("map"))
            {
                map = new Map
                {
                    Name = line,
                    Index = mapIndex++
                };
            }
            else
            {
                var ranges = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => long.Parse(x))
                    .ToArray();
                map.Mappings.Add(new Mapping
                {
                    DestinationStart = ranges[0],
                    Start = ranges[1],
                    Length = ranges[2]
                });
            }
        }

        foreach (var seed in seeds)
        {
            seed.Location = FindLocation(seed.Number, maps);
        }

        return seeds.Min(x => x.Location);
    }

    private long FindLocation(long source, List<Map> maps)
    {
        var destination = source;
        foreach (var map in maps.OrderBy(x => x.Index))
        {
            foreach (var range in map.Mappings)
            {
                if (source >= range.Start && source <= range.Start + range.Length)
                {
                    destination = range.DestinationStart + (source - range.Start);
                }
            }
            source = destination;
        }
        return destination;
    }

    [Benchmark]
    public override ValueTask<string> Solve_2() => new(Part2().ToString());

    private long Part2()
    {
        var seedPairs = input.First()[6..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => long.Parse(x))
            .ToArray();

        List<long> seedsNumbers = [];

        var sourceRanges = seedPairs.Chunk(2).Select(x => new NumberRange { Start = x.First(), End = x.First() + x.Last() - 1 }).ToList();

        List<Map> maps = [];

        Map map = new();

        var mapIndex = 0;

        foreach (var line in input.Skip(2))
        {
            if (string.IsNullOrEmpty(line))
            {
                map.Mappings = map.Mappings.OrderBy(x => x.Start).ToList();
                maps.Add(map);
            }
            else if (line.Contains("map"))
            {
                map = new Map
                {
                    Name = line,
                    Index = mapIndex++
                };
            }
            else
            {
                var ranges = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => long.Parse(x))
                    .ToArray();
                map.Mappings.Add(new Mapping
                {
                    DestinationStart = ranges[0],
                    Start = ranges[1],
                    Length = ranges[2]
                });
            }
        }

        return ProcessSourceRanges(maps, sourceRanges);
    }

    private long ProcessSourceRanges(List<Map> maps, List<NumberRange> sourceRanges)
    {
        var destinationRanges = new List<NumberRange>();
        var splitSourceRanges = new List<NumberRange>();
        
        foreach (var map in maps)
        {
            splitSourceRanges = new List<NumberRange>();
            destinationRanges = new List<NumberRange>();
            Console.WriteLine($"Map: {map.Name}");

            foreach (var sourceRange in sourceRanges.OrderBy(x => x.Start).ToList())
            {
                //Console.WriteLine($"\tSourceRange: {sourceRange.Start}-{sourceRange.End}");
                foreach (var mapping in map.Mappings.OrderBy(x=> x.Start).ToList())
                {
                    //Console.WriteLine($"\t\tMappingRange: {mapping.Start}-{mapping.End} Add: {mapping.Addition}");
                    // split on sourceRange by the mapping start and end points
                    // and store in splitSourceRanges      

                    // if mapping start is inside sourceRange  
                    if (mapping.Start >= sourceRange.Start  &&
                        mapping.Start <= sourceRange.End)
                        {
                            // if any source range left of mapping start
                            if (sourceRange.Start < mapping.Start) {
                                // create splitSource range from sourceRange.start
                                // to mapping.Start - 1
                                splitSourceRanges.Add(
                                    new NumberRange{ Start = sourceRange.Start, End = mapping.Start -1});
                                // change sourceRange to start from mapping.Start
                                sourceRange.Start = mapping.Start;
                            }
                        }
                    //if mapping end is inside source range
                    if (mapping.End >= sourceRange.Start &&
                        mapping.End <= sourceRange.End)
                        {
                            // if any source range right of end then split end off
                            if (sourceRange.End > mapping.End) {
                                splitSourceRanges.Add(
                                    new NumberRange{ Start = sourceRange.Start, End = mapping.End});
                                sourceRange.Start = mapping.End + 1;
                            }
                        }         
                }
                splitSourceRanges.Add(new NumberRange{ Start = sourceRange.Start, End = sourceRange.End});
            }

            // now all sourceRanges are split at mapping points into splitSourceRanges
            // so we can now map them all
            // let's order them first.
            // iterrate on splitSourceRanges
            foreach (var range in splitSourceRanges.OrderBy(x => x.Start)){
                // find out which mapping this range is in if any
                // should just be able to check the start point

                var matchingMappings = map.Mappings.Where(x => x.Start <= range.Start && x.End >= range.Start);

                if (matchingMappings.Any())
                {
                    //Console.WriteLine($"\t*** Found {range.Start}-{range.End} count = {matchingMappings.Count()}");
                    // then map
                    var matchedMapping = matchingMappings.First();
                    destinationRanges.Add(new NumberRange{
                        Start = range.Start + matchedMapping.Addition, 
                        End = range.End + matchedMapping.Addition});
                    //Console.WriteLine($"\tMapping {range.Start}-{range.End} to {range.Start + matchedMapping.Addition}-{range.End + matchedMapping.Addition}");
                }
                else {
                    //Console.WriteLine($"\tNot mapping {range.Start}-{range.End}");
                    destinationRanges.Add(range);
                }
            }

            sourceRanges = destinationRanges.OrderBy(x => x.Start).ToList();
        }
        var sortedDests = destinationRanges.Where(x => x.Start != 0).OrderBy(x => x.Start);
        return sortedDests.First().Start;
    }
}

public class Mapping()
{
    public long Start { get; set; }
    public long DestinationStart { get; set; }
    public long Length { get; set; }
    public long End => Start + Length - 1;
    public long Addition => DestinationStart - Start;
    
}

public class Map()
{
    public long Index { get; set; }
    public string Name { get; set; }
    public List<Mapping> Mappings { get; set; } = new List<Mapping>();
}

public class Seed()
{
    public long Number { get; set; }
    public long Location { get; set; }
}

public class NumberRange()
{
    public long Start { get; set; }
    public long End { get; set; }
}