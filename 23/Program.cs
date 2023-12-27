string[] lines = File.ReadAllLines("Input.txt");

var map = new List<List<char>>();
foreach (var line in lines)
{
	map.Add(line.ToCharArray().ToList());
}

var startRow = 0;
var startCol = map[0].IndexOf('.');

var directions = new List<Tuple<int, int>>
{
	Tuple.Create(-1, 0),
	Tuple.Create(0, 1),
	Tuple.Create(1, 0),
	Tuple.Create(0, -1)
};

int maxSteps = 0;
FindHike1();
Console.WriteLine(maxSteps);

// Part 2
maxSteps = 0;
FindHike2();
Console.WriteLine(maxSteps);

void FindHike1()
{
	var visited = new HashSet<Tuple<int, int>>();
	var visitedSteps = new Dictionary<Tuple<int, int>, int>();
	var Q = new Queue<Tuple<int, int, int, HashSet<Tuple<int, int>>>>();
	Q.Enqueue(Tuple.Create(startRow, startCol, 0, visited));

	while (Q.Count > 0)
	{
		var itemQ = Q.Dequeue();
		var row = itemQ.Item1;
		var col = itemQ.Item2;
		var steps = itemQ.Item3;
		var visitedQ = new HashSet<Tuple<int, int>>(itemQ.Item4);

		if (visitedQ.Contains(Tuple.Create(row, col)))
		{
			continue;
		}
		visitedQ.Add(Tuple.Create(row, col));
		if (visitedSteps.ContainsKey(Tuple.Create(row, col)))
		{
			if (visitedSteps[Tuple.Create(row, col)] > steps)
			{
				continue;
			}
		}
		visitedSteps[Tuple.Create(row, col)] = steps;

		if (row == map.Count - 1 && steps > maxSteps)
		{
			maxSteps = steps;
		}

		var validDirections = directions;

		if (map[row][col] == '>')
		{
			validDirections = new List<Tuple<int, int>>
			{
				Tuple.Create(0, 1)
			};
		}
		else if (map[row][col] == '<')
		{
			validDirections = new List<Tuple<int, int>>
			{
				Tuple.Create(0, -1)
			};
		}
		else if (map[row][col] == '^')
		{
			validDirections = new List<Tuple<int, int>>
			{
				Tuple.Create(-1, 0)
			};
		}
		else if (map[row][col] == 'v')
		{
			validDirections = new List<Tuple<int, int>>
			{
				Tuple.Create(1, 0)
			};
		}

		foreach (var direction in validDirections)
		{
			var nextRow = row + direction.Item1;
			var nextCol = col + direction.Item2;

			if (nextRow >= 0 &&
				nextRow < map.Count &&
				nextCol >= 0 &&
				nextCol < map[0].Count &&
				map[nextRow][nextCol] != '#')
			{
				Q.Enqueue(Tuple.Create(nextRow, nextCol, steps + 1, visitedQ));
			}
		}
	}
}

void FindHike2()
{
	HashSet<(int, int)> validPoints = GetValidPoints();

	Dictionary<(int, int), List<((int, int), int)>> adjacencyList = BuildAdjacencyList(validPoints);

	var visited = new bool[map.Count, map[0].Count];

	DFS(startRow, startCol, 0, adjacencyList, visited);
}

HashSet<(int, int)> GetValidPoints()
{
	var validPoints = new HashSet<(int, int)>();

	for (int r = 0; r < map.Count; r++)
	{
		for (int c = 0; c < map[0].Count; c++)
		{
			int neighborCount = CountNeighbors(r, c);

			if (neighborCount > 2 && map[r][c] != '#')
			{
				validPoints.Add((r, c));
			}
		}
	}

	for (int c = 0; c < map[0].Count; c++)
	{
		if (map[0][c] == '.')
		{
			validPoints.Add((0, c));
		}

		if (map[map.Count - 1][c] == '.')
		{
			validPoints.Add((map.Count - 1, c));
		}
	}

	return validPoints;
}

int CountNeighbors(int row, int col)
{
	int neighbors = 0;

	foreach (var direction in directions)
	{
		var nextRow = row + direction.Item1;
		var nextCol = col + direction.Item2;

		if (nextRow >= 0 &&
			nextRow < map.Count &&
			nextCol >= 0 &&
			nextCol < map[0].Count &&
			map[nextRow][nextCol] != '#')
		{
			neighbors++;
		}
	}

	return neighbors;
}

Dictionary<(int, int), List<((int, int), int)>> BuildAdjacencyList(HashSet<(int, int)> validPoints)
{
	var adjacencyList = new Dictionary<(int, int), List<((int, int), int)>>();

	foreach (var (rv, cv) in validPoints)
	{
		adjacencyList[(rv, cv)] = new List<((int, int), int)>();

		var Q = new Queue<(int, int, int)>();
		var visited = new HashSet<(int, int)>();

		Q.Enqueue((rv, cv, 0));

		while (Q.Count > 0)
		{
			var itemQ = Q.Dequeue();
			var row = itemQ.Item1;
			var col = itemQ.Item2;
			var steps = itemQ.Item3;

			if (visited.Contains((row, col)))
			{
				continue;
			}
			visited.Add((row, col));

			if (validPoints.Contains((row, col)) &&
				(row, col) != (rv, cv))
			{
				adjacencyList[(rv, cv)].Add(((row, col), steps));
				continue;
			}

			foreach (var direction in directions)
			{
				var nextRow = row + direction.Item1;
				var nextCol = col + direction.Item2;

				if (nextRow >= 0 &&
					nextRow < map.Count &&
					nextCol >= 0 &&
					nextCol < map[0].Count &&
					map[nextRow][nextCol] != '#')
				{
					Q.Enqueue((nextRow, nextCol, steps + 1));
				}
			}
		}
	}

	return adjacencyList;
}

void DFS(int row, int col, int distance, Dictionary<(int, int), List<((int, int), int)>> adjacencyList, bool[,] visited)
{
	if (visited[row, col])
	{
		return;
	}
	visited[row, col] = true;

	if (row == map.Count - 1)
	{
		maxSteps = Math.Max(maxSteps, distance);
	}

	foreach (var (neighbor, neighborDistance) in adjacencyList[(row, col)])
	{
		DFS(neighbor.Item1, neighbor.Item2, distance + neighborDistance, adjacencyList, visited);
	}

	visited[row, col] = false;
}
