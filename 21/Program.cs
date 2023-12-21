string[] lines = File.ReadAllLines("Input.txt");

var map = new List<List<char>>();
foreach (var line in lines)
{
	map.Add(line.ToCharArray().ToList());
}

int maxSteps = 64;
int maxRow = map.Count;
int maxCol = map[0].Count;
int startRow = -1;
int startCol = -1;
for (int row = 0; row < maxRow; row++)
{
	for (int col = 0; col < maxCol; col++)
	{
		if (map[row][col] == 'S')
		{
			startRow = row;
			startCol = col;
			break;
		}
	}

	if (startRow >= 0)
	{
		break;
	}
}

var visited = new HashSet<Tuple<int, int, int>>();
var Q = new Queue<Tuple<int, int, int>>();
Q.Enqueue(Tuple.Create(startRow, startCol, 0));

while (Q.Count > 0)
{
	var itemQ = Q.Dequeue();
	var currentRow = itemQ.Item1;
	var currentCol = itemQ.Item2;
	var currentSteps = itemQ.Item3;

	if (visited.Contains(Tuple.Create(currentRow, currentCol, currentSteps)))
	{
		continue;
	}
	else
	{
		visited.Add(Tuple.Create(currentRow, currentCol, currentSteps));
	}

	if (currentSteps == maxSteps)
	{
		map[currentRow][currentCol] = 'O';
	}
	else
	{
		if ((currentCol - 1) >= 0 && (currentCol - 1) < maxCol && map[currentRow][currentCol - 1] == '.')
		{
			Q.Enqueue(Tuple.Create(currentRow, currentCol - 1, currentSteps + 1));
		}
		if ((currentCol + 1) >= 0 && (currentCol + 1) < maxCol && map[currentRow][currentCol + 1] == '.')
		{
			Q.Enqueue(Tuple.Create(currentRow, currentCol + 1, currentSteps + 1));
		}
		if ((currentRow - 1) >= 0 && (currentRow - 1) < maxCol && map[currentRow - 1][currentCol] == '.')
		{
			Q.Enqueue(Tuple.Create(currentRow - 1, currentCol, currentSteps + 1));
		}
		if ((currentRow + 1) >= 0 && (currentRow + 1) < maxCol && map[currentRow + 1][currentCol] == '.')
		{
			Q.Enqueue(Tuple.Create(currentRow + 1, currentCol, currentSteps + 1));
		}
	}
}


var result = 0;
for (int row = 0; row < map.Count; row++)
{
	for (int col = 0; col < map[0].Count; col++)
	{
		if (map[row][col] == 'O' || map[row][col] == 'S')
		{
			result++;
		}
	}
}
Console.WriteLine(result);

// Part 2
maxSteps = 26501365;
var map2 = new List<List<char>>();
foreach (var line in lines)
{
	map2.Add(line.ToCharArray().ToList());
}

int maxRow2 = map2.Count;
int maxCol2 = map2[0].Count;
for (int row = 0; row < maxRow2; row++)
{
	for (int col = 0; col < maxCol2; col++)
	{
		if (map2[row][col] == 'S')
		{
			startRow = row;
			startCol = col;
			break;
		}
	}

	if (startRow >= 0)
	{
		break;
	}
}

var waysCache = new Dictionary<(int, int), long>();
var distances = GetDistances(startRow, startCol);

var result2 = CountPlots(distances);
Console.WriteLine(result2);


Dictionary<(int, int, int, int), int> GetDistances(int startRow, int startCol)
{
	Dictionary<(int, int, int, int), int> distances = new Dictionary<(int, int, int, int), int>();
	Queue<(int, int, int, int, int)> Q = new Queue<(int, int, int, int, int)>();
	Q.Enqueue((0, 0, startRow, startCol, 0));

	while (Q.Count > 0)
	{
		var itemQ = Q.Dequeue();
		var currentTransitionRow = itemQ.Item1;
		var currentTransitionCol = itemQ.Item2;
		var currentRow = itemQ.Item3;
		var currentCol = itemQ.Item4;
		var currentSteps = itemQ.Item5;

		if (currentRow < 0)
		{
			currentTransitionRow -= 1;
			currentRow += maxRow2;
		}
		if (currentRow >= maxRow2)
		{
			currentTransitionRow += 1;
			currentRow -= maxRow2;
		}
		if (currentCol < 0)
		{
			currentTransitionCol -= 1;
			currentCol += maxCol2;
		}
		if (currentCol >= maxCol2)
		{
			currentTransitionCol += 1;
			currentCol -= maxCol2;
		}

		if (!(currentRow >= 0 &&
			currentRow < maxRow2 &&
			currentCol >= 0 &&
			currentCol < maxCol2 &&
			map2[currentRow][currentCol] != '#'))
		{
			continue;
		}

		if (distances.ContainsKey((currentTransitionRow, currentTransitionCol, currentRow, currentCol)))
		{
			continue;
		}

		if (Math.Abs(currentTransitionRow) > 4 ||
			Math.Abs(currentTransitionCol) > 4)
		{
			continue;
		}

		distances[(currentTransitionRow, currentTransitionCol, currentRow, currentCol)] = currentSteps;

		Q.Enqueue((currentTransitionRow, currentTransitionCol, currentRow, currentCol - 1, currentSteps + 1));
		Q.Enqueue((currentTransitionRow, currentTransitionCol, currentRow, currentCol + 1, currentSteps + 1));
		Q.Enqueue((currentTransitionRow, currentTransitionCol, currentRow - 1, currentCol, currentSteps + 1));
		Q.Enqueue((currentTransitionRow, currentTransitionCol, currentRow + 1, currentCol, currentSteps + 1));
	}

	return distances;
}

long CountPlots(Dictionary<(int, int, int, int), int> distances)
{
	long ways = 0;
	var directions = new List<int> { -3, -2, -1, 0, 1, 2, 3 };

	for (int row = 0; row < maxRow2; row++)
	{
		for (int col = 0; col < maxCol2; col++)
		{
			if (distances.ContainsKey((0, 0, row, col)))
			{
				foreach (var transitionRow in directions)
				{
					foreach (var transitionCol in directions)
					{
						var steps = distances[(transitionRow, transitionCol, row, col)];

						if (steps % 2 == maxSteps % 2 &&
							steps <= maxSteps)
						{
							ways += 1;
						}

						if ((transitionRow == directions.Min() || transitionRow == directions.Max()) &&
							(transitionCol == directions.Min() || transitionCol == directions.Max()))
						{
							ways += CalculateWays(steps, 2);
						}
						else if ((transitionRow == directions.Min() || transitionRow == directions.Max()) ||
							(transitionCol == directions.Min() || transitionCol == directions.Max()))
						{
							ways += CalculateWays(steps, 1);
						}
					}
				}
			}
		}
	}

	return ways;
}

long CalculateWays(int steps, int variant)
{
	int stepsToConsider = (maxSteps - steps) / maxRow2;

	if (waysCache.ContainsKey((steps, variant)))
	{
		return waysCache[(steps, variant)];
	}

	long waysCount = 0;
	for (int i = 1; i <= stepsToConsider; i++)
	{
		if (steps + (maxRow2 * i) <= maxSteps &&
			(steps + (maxRow2 * i)) % 2 == (maxSteps % 2))
		{
			waysCount += (variant == 2)
				? (i + 1)
				: 1;
		}
	}

	waysCache[(steps, variant)] = waysCount;

	return waysCount;
}
