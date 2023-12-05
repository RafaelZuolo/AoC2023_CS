namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly IList<string> input;
    private readonly IList<long> seeds;
    private readonly IList<BasicMap> seedToSoil;
    private readonly IList<BasicMap> soilToFertilizer;
    private readonly IList<BasicMap> fertilizerToWater;
    private readonly IList<BasicMap> waterToLight;
    private readonly IList<BasicMap> lightToTemperature;
    private readonly IList<BasicMap> temperatureToHumidity;
    private readonly IList<BasicMap> humidityToLocation;

    public Day05()
    {
        input = File.ReadAllText(InputFilePath).Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
        seeds = input[0].Split(" ", StringSplitOptions.TrimEntries).Skip(1).Select(long.Parse).ToList();
        seedToSoil = input[1].Split("\n").Skip(1).Select(ParseLineToBasicMap).ToList();
        soilToFertilizer = input[2].Split("\n").Skip(1).Select(ParseLineToBasicMap).ToList();
        fertilizerToWater = input[3].Split("\n").Skip(1).Select(ParseLineToBasicMap).ToList();
        waterToLight = input[4].Split("\n").Skip(1).Select(ParseLineToBasicMap).ToList();
        lightToTemperature = input[5].Split("\n").Skip(1).Select(ParseLineToBasicMap).ToList();
        temperatureToHumidity = input[6].Split("\n").Skip(1).Select(ParseLineToBasicMap).ToList();
        humidityToLocation = input[7].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(ParseLineToBasicMap).ToList();
    }


    private BasicMap ParseLineToBasicMap(string line)
    {        
        var values = line.Split(" ", StringSplitOptions.TrimEntries);
        var intervalSize = long.Parse(values[2]) - 1;
        var domainStart = long.Parse(values[1]);
        var domainEnd = domainStart + intervalSize;
        var imageStart = long.Parse(values[0]);
        var imageEnd = imageStart + intervalSize;

        return new(domainStart, domainEnd, imageStart, imageEnd);
    }


    public override ValueTask<string> Solve_1()
    {
        var location = seeds.Select(s => ApplyMap(seedToSoil, s))
            .Select(s => ApplyMap(soilToFertilizer, s))
            .Select(s => ApplyMap(fertilizerToWater, s))
            .Select(s => ApplyMap(waterToLight, s))
            .Select(s => ApplyMap(lightToTemperature, s))
            .Select(s => ApplyMap(temperatureToHumidity, s))
            .Select(s => ApplyMap(humidityToLocation, s));

        return new(location.Min().ToString());
    }

    private record BasicMap(long DomainStart, long DomainEnd, long ImageStart, long ImageEnd)
    {
        public long ApplyMap(long value)
        {
            if (DomainStart <= value && value <= DomainEnd)
            {
                return ImageStart + (value - DomainStart);
            }

            return value;
        }
    }

    private static long ApplyMap(IList<BasicMap> mapList, long value)
    {
        foreach (var map in mapList)
        {
            var result = map.ApplyMap(value);
            if (result != value)
            {
                return result;
            }
        }

        return value;
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>("foo");
    }

}
