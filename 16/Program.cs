
string[] lines = File.ReadAllLines("Input.txt");

var map = new List<List<char>>();
foreach (var line in lines)
{
	map.Add(line.ToCharArray().ToList());
}

int[] directionRows = { -1, 0, 1, 0 };
int[] directionColumns = { 0, 1, 0, -1 };

var result = GetEnergized(0, 0, 1);
Console.WriteLine(result);

// Part2
int result2 = 0;
for (int row = 0; row < map.Count; row++)
{
	result2 = Math.Max(result2, GetEnergized(row, 0, 1));
	result2 = Math.Max(result2, GetEnergized(row, map[0].Count - 1, 3));
}
for (int col = 0; col < map[0].Count; col++)
{
	result2 = Math.Max(result2, GetEnergized(0, col, 2));
	result2 = Math.Max(result2, GetEnergized(map.Count - 1, col, 0));
}
Console.WriteLine(result2);


int GetEnergized(int startRow, int startCol, int startDirection)
{
	var positions = new List<Tuple<int, int, int>> { Tuple.Create(startRow, startCol, startDirection) };
	var seenCells = new HashSet<Tuple<int, int>>();
	var seenCellsWithDirection = new HashSet<Tuple<int, int, int>>();

	while (positions.Any())
	{
		var nextPositions = new List<Tuple<int, int, int>>();

		foreach (var position in positions)
		{
			var row = position.Item1;
			var column = position.Item2;
			var direction = position.Item3;
			if (row >= 0 &&
				row < map.Count &&
				column >= 0 &&
				column < map[0].Count)
			{
				seenCells.Add(Tuple.Create(row, column));
				if (seenCellsWithDirection.Contains(Tuple.Create(row, column, direction)))
				{
					continue;
				}

				seenCellsWithDirection.Add(Tuple.Create(row, column, direction));
				var tile = map[row][column];

				if (tile == '.')
				{
					nextPositions.Add(Move(row, column, direction));
				}
				else if (tile == '/')
				{
					switch (direction)
					{
						case 0:
							nextPositions.Add(Move(row, column, 1));
							break;
						case 1:
							nextPositions.Add(Move(row, column, 0));
							break;
						case 2:
							nextPositions.Add(Move(row, column, 3));
							break;
						case 3:
							nextPositions.Add(Move(row, column, 2));
							break;
					}
				}
				else if (tile == '\\')
				{
					switch (direction)
					{
						case 0:
							nextPositions.Add(Move(row, column, 3));
							break;
						case 1:
							nextPositions.Add(Move(row, column, 2));
							break;
						case 2:
							;
							nextPositions.Add(Move(row, column, 1));
							break;
						case 3:
							nextPositions.Add(Move(row, column, 0));
							break;
					}
				}
				else if (tile == '|')
				{
					if (direction == 0 ||
						direction == 2)
					{
						nextPositions.Add(Move(row, column, direction));
					}
					else
					{
						nextPositions.Add(Move(row, column, 0));
						nextPositions.Add(Move(row, column, 2));
					}
				}
				else if (tile == '-')
				{
					if (direction == 1 ||
						direction == 3)
					{
						nextPositions.Add(Move(row, column, direction));
					}
					else
					{
						nextPositions.Add(Move(row, column, 1));
						nextPositions.Add(Move(row, column, 3));
					}
				}
			}
		}
		positions = nextPositions;
	}
	return seenCells.Count;
}

Tuple<int, int, int> Move(int row, int column, int direction)
{
	return Tuple.Create(row + directionRows[direction], column + directionColumns[direction], direction);
}