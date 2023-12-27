string[] lines = File.ReadAllLines("Input.txt");

var bricks = new List<Brick>();
foreach (var line in lines)
{
	var lineSplit = line.Split('~');
	var lineSplit1 = lineSplit[0].Split(',');
	var lineSplit2 = lineSplit[1].Split(',');
	bricks.Add(new Brick(
		Convert.ToInt32(lineSplit1[0]),
		Convert.ToInt32(lineSplit1[1]),
		Convert.ToInt32(lineSplit1[2]),
		Convert.ToInt32(lineSplit2[0]),
		Convert.ToInt32(lineSplit2[1]),
		Convert.ToInt32(lineSplit2[2])));
}

var map = new List<List<(int, int, int)>>();
foreach (var brick in bricks)
{
	var mapLine = new List<(int, int, int)>();
	if (brick.X1 == brick.X2 && brick.Y1 == brick.Y2)
	{
		for (int z = brick.Z1; z <= brick.Z2; z++)
		{
			mapLine.Add((brick.X1, brick.Y1, z));
		}
	}
	else if (brick.X1 == brick.X2 && brick.Z1 == brick.Z2)
	{
		for (int y = brick.Y1; y <= brick.Y2; y++)
		{
			mapLine.Add((brick.X1, y, brick.Z1));
		}
	}
	else if (brick.Y1 == brick.Y2 && brick.Z1 == brick.Z2)
	{
		for (int x = brick.X1; x <= brick.X2; x++)
		{
			mapLine.Add((x, brick.Y1, brick.Z1));
		}
	}

	map.Add(mapLine);
}

var visited = new HashSet<(int, int, int)>();
foreach (var mapLine in map)
{
	foreach (var pos in mapLine)
	{
		visited.Add(pos);
	}
}

while (true)
{
	bool foundBrick = false;

	for (int i = 0; i < map.Count; i++)
	{
		bool checkZ = true;

		foreach (var pos in map[i])
		{
			int x = pos.Item1;
			int y = pos.Item2;
			int z = pos.Item3;

			if (z == 1)
			{
				checkZ = false;
			}

			if (visited.Contains((x, y, z - 1)) &&
				!map[i].Contains((x, y, z - 1)))
			{
				checkZ = false;
			}
		}

		if (checkZ)
		{
			foundBrick = true;

			foreach (var pos in map[i])
			{
				int x = pos.Item1;
				int y = pos.Item2;
				int z = pos.Item3;

				visited.Remove((x, y, z));
				visited.Add((x, y, z - 1));
			}

			map[i] = map[i].Select(pos => (pos.Item1, pos.Item2, pos.Item3 - 1)).ToList();
		}
	}

	if (!foundBrick)
	{
		break;
	}
}

var visitedCopy = new HashSet<(int, int, int)>(visited);
var mapCopy = map.Select(list => list.ToList()).ToList();

int result1 = 0;
int result2 = 0;

for (int i = 0; i < map.Count; i++)
{
	visited = new HashSet<(int, int, int)>(visitedCopy);
	map = mapCopy.Select(list => list.ToList()).ToList();

	foreach (var pos in map[i])
	{
		visited.Remove(pos);
	}

	var fallBricks = new HashSet<int>();

	while (true)
	{
		bool foundBrick = false;

		for (int j = 0; j < map.Count; j++)
		{
			if (j == i)
			{
				continue;
			}

			bool checkZ = true;

			foreach (var pos in map[j])
			{
				int x = pos.Item1;
				int y = pos.Item2;
				int z = pos.Item3;

				if (z == 1)
				{
					checkZ = false;
				}

				if (visited.Contains((x, y, z - 1)) &&
					!map[j].Contains((x, y, z - 1)))
				{
					checkZ = false;
				}
			}

			if (checkZ)
			{
				fallBricks.Add(j);

				foreach (var pos in map[j])
				{
					visited.Remove(pos);
					visited.Add((pos.Item1, pos.Item2, pos.Item3 - 1));
				}

				map[j] = map[j].Select(pos => (pos.Item1, pos.Item2, pos.Item3 - 1)).ToList();

				foundBrick = true;
			}
		}

		if (!foundBrick)
		{
			break;
		}
	}

	if (fallBricks.Count == 0)
	{
		result1++;
	}

	result2 += fallBricks.Count;
}

Console.WriteLine(result1);
Console.WriteLine(result2);


class Brick
{
	public int X1 { get; set; }
	public int Y1 { get; set; }
	public int Z1 { get; set; }
	public int X2 { get; set; }
	public int Y2 { get; set; }
	public int Z2 { get; set; }

	public Brick(int x1, int y1, int z1, int x2, int y2, int z2)
	{
		X1 = x1;
		Y1 = y1;
		Z1 = z1;
		X2 = x2;
		Y2 = y2;
		Z2 = z2;
	}
}
