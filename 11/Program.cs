
string[] lines = File.ReadAllLines("Input.txt");

var mapAux = new List<List<char>>();
var map = new List<List<char>>();
var galaxies = new HashSet<Tuple<int, int>>();
foreach (var line in lines)
{
	mapAux.Add(line.ToCharArray().ToList());
	if (!line.Contains('#'))
	{
		mapAux.Add(line.ToCharArray().ToList());
	}
}
var emptyCols = new List<int>();
for (int col = 0; col < mapAux[0].Count; col++)
{
	if (!lines.Any(l => l[col] == '#'))
	{
		emptyCols.Add(col);
	}
}
foreach (var line in mapAux)
{
	var mapCols = new List<char>();
	for (int i = 0; i < line.Count; i++)
	{
		if (emptyCols.Contains(i))
		{
			mapCols.Add(line[i]);
		}
		mapCols.Add(line[i]);
	}
	map.Add(mapCols);
}
for (int row = 0; row < map.Count; row++)
{
	for (int col = 0; col < map[row].Count; col++)
	{
		if (map[row][col] == '#')
		{
			galaxies.Add(Tuple.Create(row, col));
		}
	}
}

var distances = new List<int>();
for (int i = 0; i < galaxies.Count; i++)
{
	for (int j = i + 1; j < galaxies.Count; j++)
	{
		var startGalaxy = galaxies.ElementAt(i);
		var endGalaxy = galaxies.ElementAt(j);
		distances.Add(CalculateDistance(startGalaxy.Item1, startGalaxy.Item2, endGalaxy.Item1, endGalaxy.Item2));
	}
}

var result = distances.Sum();
Console.WriteLine(result);


//Part 2
var map2 = new List<List<char>>();
emptyCols = new List<int>();
var emptyRows = new List<int>();
for (int row = 0; row < lines.Length; row++)
{
	map2.Add(lines[row].ToCharArray().ToList());
	if (!lines[row].Contains('#'))
	{
		emptyRows.Add(row);
	}
}
emptyCols = new List<int>();
for (int col = 0; col < map2[0].Count; col++)
{
	if (!lines.Any(l => l[col] == '#'))
	{
		emptyCols.Add(col);
	}
}

var galaxies2 = new HashSet<Tuple<int, int>>();
for (int row = 0; row < map2.Count; row++)
{
	for (int col = 0; col < map2[row].Count; col++)
	{
		if (map2[row][col] == '#')
		{
			galaxies2.Add(Tuple.Create(row, col));
		}
	}
}

var distances2 = new List<long>();
for (int i = 0; i < galaxies2.Count; i++)
{
	for (int j = i + 1; j < galaxies2.Count; j++)
	{
		var startGalaxy = galaxies2.ElementAt(i);
		var endGalaxy = galaxies2.ElementAt(j);
		distances2.Add(CalculateDistance2(startGalaxy.Item1, startGalaxy.Item2, endGalaxy.Item1, endGalaxy.Item2));
	}
}

var result2 = distances2.Sum();
Console.WriteLine(result2);


int CalculateDistance(int rowStart, int colStart, int rowEnd, int colEnd)
{
	var distance = Math.Abs(rowEnd - rowStart) + Math.Abs(colEnd - colStart);
	return distance;
}

long CalculateDistance2(int rowStart, int colStart, int rowEnd, int colEnd)
{
	int times = 1000000 - 1;
	long distance = Math.Abs(rowEnd - rowStart) + Math.Abs(colEnd - colStart);

	foreach (int emptyRow in emptyRows)
	{
		if (Math.Min(rowStart, rowEnd) <= emptyRow && emptyRow <= Math.Max(rowStart, rowEnd))
		{
			distance += times;
		}
	}

	foreach (int emptyCol in emptyCols)
	{
		if (Math.Min(colStart, colEnd) <= emptyCol && emptyCol <= Math.Max(colStart, colEnd))
		{
			distance += times;
		}
	}

	return distance;
}