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
        var reinterpretedSeeds = ParseSeeds(seeds);

        var composedMappings = ComposeMap(seedToSoil, reinterpretedSeeds);
        
        return new ValueTask<string>("foo");
    }

    private static IList<BasicMap> ApplyMapToRange(IList<BasicMap> mapList, BasicMap firstMap)
    {
        var rangeMapping = new List<BasicMap>();
        foreach (var secondMap in mapList)
        {
            if (secondMap.DomainEnd < firstMap.ImageStart || firstMap.ImageEnd < secondMap.DomainStart)
            {
                rangeMapping.Add(secondMap);
            }
            
            else if (firstMap.ImageStart < secondMap.DomainStart && secondMap.DomainEnd < firstMap.ImageEnd)
            {
                var firstPartLength = secondMap.DomainStart - firstMap.ImageStart;                
                rangeMapping.Add(new(
                    firstMap.DomainStart,
                    firstMap.DomainStart + (firstPartLength - 1),
                    firstMap.ImageStart,
                    firstMap.ImageStart + (firstPartLength - 1)));

                rangeMapping.Add(new(
                    firstMap.DomainStart + firstPartLength,
                    firstMap.DomainStart + (secondMap.DomainEnd - secondMap.DomainStart),
                    secondMap.ImageStart,
                    secondMap.ImageEnd));

                var thirdPartLength = firstMap.ImageEnd - secondMap.DomainEnd;
                rangeMapping.Add(new(
                    firstMap.DomainEnd - thirdPartLength + 1,
                    firstMap.DomainEnd,
                    firstMap.ImageEnd - thirdPartLength + 1,
                    firstMap.ImageEnd));
            }

            else if (secondMap.DomainStart < firstMap.ImageStart && firstMap.ImageEnd < secondMap.DomainEnd)
            {
                var firstPartLength = firstMap.ImageStart - secondMap.DomainStart;
                rangeMapping.Add(new(
                    secondMap.DomainStart,
                    secondMap.DomainStart + (firstPartLength - 1),
                    secondMap.ImageStart,
                    secondMap.ImageStart + (firstPartLength - 1)));

                var secondPartLenght = firstMap.ImageEnd - firstMap.ImageStart + 1; // length of firstMap
                rangeMapping.Add(new(
                    firstMap.DomainStart,
                    firstMap.DomainEnd,
                    secondMap.ImageStart + firstPartLength,
                    secondMap.ImageStart + firstPartLength + secondPartLenght));

                var thirdPartLength = secondMap.DomainEnd - firstMap.ImageEnd;
                rangeMapping.Add(new(
                    secondMap.DomainEnd - thirdPartLength + 1,
                    secondMap.DomainEnd,
                    secondMap.ImageEnd - thirdPartLength + 1,
                    secondMap.ImageEnd));
            }

            else if (secondMap.DomainStart <= firstMap.ImageStart && secondMap.DomainEnd < firstMap.ImageEnd)
            {
                // TODO
                var firstPartLength = firstMap.ImageStart - secondMap.DomainStart;
                rangeMapping.Add(new(
                    secondMap.DomainStart,
                    secondMap.DomainStart + (firstPartLength - 1),
                    secondMap.ImageStart,
                    secondMap.ImageStart + (firstPartLength - 1)));

                var secondPartLenght = firstMap.ImageEnd - firstMap.ImageStart + 1; // length of firstMap
                rangeMapping.Add(new(
                    firstMap.DomainStart,
                    firstMap.DomainEnd,
                    secondMap.ImageStart + firstPartLength,
                    secondMap.ImageStart + firstPartLength + secondPartLenght));

                var thirdPartLength = secondMap.DomainEnd - firstMap.ImageEnd;
                rangeMapping.Add(new(
                    secondMap.DomainEnd - thirdPartLength + 1,
                    secondMap.DomainEnd,
                    secondMap.ImageEnd - thirdPartLength + 1,
                    secondMap.ImageEnd));
            }

            else if ( firstMap.ImageStart <= secondMap.DomainStart && firstMap.ImageEnd < secondMap.DomainEnd)
            {
                // TODO
                var firstPartLength = firstMap.ImageStart - secondMap.DomainStart;
                rangeMapping.Add(new(
                    secondMap.DomainStart,
                    secondMap.DomainStart + (firstPartLength - 1),
                    secondMap.ImageStart,
                    secondMap.ImageStart + (firstPartLength - 1)));

                var secondPartLenght = firstMap.ImageEnd - firstMap.ImageStart + 1; // length of firstMap
                rangeMapping.Add(new(
                    firstMap.DomainStart,
                    firstMap.DomainEnd,
                    secondMap.ImageStart + firstPartLength,
                    secondMap.ImageStart + firstPartLength + secondPartLenght));

                var thirdPartLength = secondMap.DomainEnd - firstMap.ImageEnd;
                rangeMapping.Add(new(
                    secondMap.DomainEnd - thirdPartLength + 1,
                    secondMap.DomainEnd,
                    secondMap.ImageEnd - thirdPartLength + 1,
                    secondMap.ImageEnd));
            }

            var result = secondMap.ApplyMap(firstMap.ImageStart);
            if (result != firstMap.ImageStart)
            {
                return result;
            }
        }

        return firstMap;
    }

    private IList<BasicMap> ParseSeeds(IList<long> seeds)
    {
        var seedMapping = new List<BasicMap>();
        for (var i = 0; i < seeds.Count; i += 2)
        {
            var firstSeed = seeds[i];
            var numberOfSeeds = seeds[i + 1] - 1;

            seedMapping.Add(new(firstSeed, numberOfSeeds, firstSeed, numberOfSeeds));
        }

        return seedMapping;
    }
}
