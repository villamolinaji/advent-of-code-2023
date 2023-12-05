string[] lines = File.ReadAllLines("Input.txt");

var seeds = new List<long>();
var mapConverts = new List<MapConvert>();
var maps = new List<Map>();

foreach (var line in lines)
{
	if (line.StartsWith("seeds:"))
	{
		var lineSplit = line.Split(": ");
		var seedsAux = lineSplit[1].Split(" ");

		foreach (var seed in seedsAux)
		{
			seeds.Add(Convert.ToInt64(seed));
		}
	}
	else if (line.Contains(":"))
	{
	}
	else if (line == "")
	{
		if (mapConverts.Count > 0)
		{
			var map = new Map(mapConverts);
			maps.Add(map);
			mapConverts = new List<MapConvert>();
		}
	}
	else
	{
		var lineSplit = (line.Split(" "));

		var destination = Convert.ToInt64(lineSplit[0]);
		var source = Convert.ToInt64(lineSplit[1]);
		var length = Convert.ToInt64(lineSplit[2]);

		mapConverts.Add(new MapConvert(destination, source, length));
	}
}
if (mapConverts.Count > 0)
{
	var map = new Map(mapConverts);
	maps.Add(map);
}

var seedLocationList = new List<SeedLocation>();
foreach (var seed in seeds)
{
	long location = CalculateLocation(seed);
	seedLocationList.Add(new SeedLocation(seed, location));
}

var result = seedLocationList.Min(l => l.Location);
Console.WriteLine(result);

// Part 2
var locations = new List<long>();
List<(long, long)> seedPairs = CreateSeedPairs(seeds);

foreach ((long startSeed, long endSeed) in seedPairs)
{
	long location = CalculateLocationRange(startSeed, endSeed);
	locations.Add(location);
}
var result2 = locations.Min();
Console.WriteLine(result2);



long CalculateLocation(long seed)
{
	long location = seed;

	for (int i = 0; i < maps.Count; i++)
	{
		location = GetNextLocation(location, maps[i]);
	}

	return location;
}

long GetNextLocation(long currentLocation, Map map)
{
	foreach (var mapConvert in map.MapConvertList)
	{
		if (currentLocation >= mapConvert.Source
			&& currentLocation < (mapConvert.Source + mapConvert.Length))
		{
			return currentLocation + mapConvert.Destination - mapConvert.Source;
		}
	}
	return currentLocation;
}

List<(long, long)> CreateSeedPairs(List<long> seeds)
{
	var result = new List<(long, long)>();

	for (int i = 0; i < seeds.Count; i = i + 2)
	{
		result.Add((seeds[i], seeds[i + 1]));
	}

	return result;
}

long CalculateLocationRange(long startRange, long endRange)
{
	List<(long, long)> range = new List<(long, long)> { (startRange, startRange + endRange) };
	for (int i = 0; i < maps.Count; i++)
	{
		range = GetNextLocationRange(range, maps[i]);
	}

	return range.Min(r => r.Item1);
}

List<(long, long)> GetNextLocationRange(List<(long, long)> seedsRange, Map map)
{
	List<(long, long)> addedSeedsRange = new List<(long, long)>();

	foreach (var mapConvert in map.MapConvertList)
	{
		long sourceEnd = mapConvert.Source + mapConvert.Length;
		List<(long, long)> newSeedsRange = new List<(long, long)>();

		while (seedsRange.Count > 0)
		{
			(long rangeStart, long rangeEnd) = seedsRange[seedsRange.Count - 1];
			seedsRange.RemoveAt(seedsRange.Count - 1);

			(long beforeStart, long beforeEnd) = (rangeStart, Math.Min(rangeEnd, mapConvert.Source));
			(long middleStart, long middleEnd) = (Math.Max(rangeStart, mapConvert.Source), Math.Min(sourceEnd, rangeEnd));
			(long afterStart, long afterEnd) = (Math.Max(sourceEnd, rangeStart), rangeEnd);

			if (beforeEnd > beforeStart)
			{
				newSeedsRange.Add((beforeStart, beforeEnd));
			}
			if (middleEnd > middleStart)
			{
				addedSeedsRange.Add((middleStart - mapConvert.Source + mapConvert.Destination, middleEnd - mapConvert.Source + mapConvert.Destination));
			}
			if (afterEnd > afterStart)
			{
				newSeedsRange.Add((afterStart, afterEnd));
			}
		}
		seedsRange = newSeedsRange;
	}
	return addedSeedsRange.Concat(seedsRange).ToList();
}

class Map
{
	public List<MapConvert> MapConvertList { get; set; }

	public Map(List<MapConvert> mapConvertList)
	{
		MapConvertList = new List<MapConvert>();
		foreach (var mapConvert in mapConvertList)
		{
			MapConvertList.Add(mapConvert);
		}
	}
}

class MapConvert
{
	public long Destination { get; set; }
	public long Source { get; set; }
	public long Length { get; set; }

	public MapConvert(long destination, long source, long length)
	{
		Destination = destination;
		Source = source;
		Length = length;
	}
}

class SeedLocation
{
	public long Seed { get; set; }
	public long Location { get; set; }
	public SeedLocation(long seed, long location)
	{
		Seed = seed;
		Location = location;
	}
}