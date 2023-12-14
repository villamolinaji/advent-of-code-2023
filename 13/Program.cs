string[] lines = File.ReadAllLines("Input.txt");

var patterns = new List<Pattern>();
var pattern = new Pattern();
var map = new List<List<char>>();
foreach (var line in lines)
{
	if (line == "")
	{
		pattern.Map = map;
		patterns.Add(pattern);
		pattern = new Pattern();
		map = new List<List<char>>();
	}
	else
	{
		map.Add(line.ToCharArray().ToList());
	}
}
pattern.Map = map;
patterns.Add(pattern);

foreach (var p in patterns)
{
	FindReflection(p, false);
}

var result = patterns.Sum(p => p.Value);
Console.WriteLine(result);

// Part 2
foreach (var p in patterns)
{
	FindReflection(p, true);
}

var result2 = patterns.Sum(p => p.Value);
Console.WriteLine(result2);


void FindReflection(Pattern pattern, bool isPart2)
{
	int value = 0;

	for (int col = 0; col < pattern.Map[0].Count - 1; col++)
	{
		int badCount = 0;
		for (int i = 0; i < pattern.Map[0].Count; i++)
		{
			int left = col - i;
			int right = col + i + 1;
			if (left >= 0 &&
				right < pattern.Map[0].Count)
			{
				for (int row = 0; row < pattern.Map.Count; row++)
				{
					if (pattern.Map[row][left] != pattern.Map[row][right])
					{
						badCount++;
					}
				}
			}
		}
		if (badCount == (isPart2 ? 1 : 0))
		{
			value += col + 1;
		}
	}

	for (int row = 0; row < pattern.Map.Count - 1; row++)
	{
		int badCount = 0;
		for (int i = 0; i < pattern.Map.Count; i++)
		{
			int up = row - i;
			int down = row + i + 1;
			if (up >= 0 &&
				down < pattern.Map.Count)
			{
				for (int col = 0; col < pattern.Map[0].Count; col++)
				{
					if (pattern.Map[up][col] != pattern.Map[down][col])
					{
						badCount++;
					}
				}
			}
		}
		if (badCount == (isPart2 ? 1 : 0))
		{
			value += 100 * (row + 1);
		}
	}

	pattern.Value = value;
}


class Pattern
{
	public List<List<char>> Map { get; set; }
	public int Value { get; set; }

	public Pattern()
	{
		Map = new List<List<char>>();
		Value = 0;
	}
}