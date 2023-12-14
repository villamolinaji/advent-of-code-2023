
string[] lines = File.ReadAllLines("Input.txt");

var map = new List<List<char>>();
foreach (var line in lines)
{
	map.Add(line.ToCharArray().ToList());
}

Roll();

var result = GetResult();
Console.WriteLine(result);

// Part 2
map = new List<List<char>>();
foreach (var line in lines)
{
	map.Add(line.ToCharArray().ToList());
}

Dictionary<string, int> mapDictionary = new Dictionary<string, int>();
for (int i = 1; i <= 1000000000; i++)
{
	for (int j = 0; j < 4; j++)
	{
		Roll();
		Rotate();
	}

	string mapString = string.Join("", map.Select(row => new string(row.ToArray())));
	if (mapDictionary.ContainsKey(mapString))
	{
		int cycle = i - mapDictionary[mapString];
		int cyclesToSkip = (1000000000 - i) / cycle;
		i += cyclesToSkip * cycle;
	}

	mapDictionary[mapString] = i;
}

result = GetResult();
Console.WriteLine(result);


void Roll()
{
	for (int col = 0; col < map[0].Count; col++)
	{
		for (int row = 0; row < map.Count; row++)
		{
			if (map[row][col] == 'O')
			{
				int i = 0;
				while (row - (i + 1) >= 0 &&
					map[row - (i + 1)][col] == '.')
				{
					map[row - (i + 1)][col] = 'O';
					map[row - i][col] = '.';
					i++;
				}
			}
		}
	}
}

int GetResult()
{
	var result = 0;
	int cont = 1;
	for (int row = map.Count - 1; row >= 0; row--)
	{
		result += map[row].Count(c => c == 'O') * cont;
		cont++;
	}
	return result;
}

void Rotate()
{
	var newMap = new List<List<char>>(); ;
	for (int col = 0; col < map[0].Count; col++)
	{
		string line = string.Empty;
		for (int row = 0; row < map.Count; row++)
		{
			line += map[map.Count - 1 - row][col];
		}
		newMap.Add(line.ToCharArray().ToList());
	}
	map = newMap;
}