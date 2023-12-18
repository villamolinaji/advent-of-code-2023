
string[] lines = File.ReadAllLines("Input.txt");

var map = new List<List<int>>();
foreach (var line in lines)
{
	map.Add(line.Select(c => Convert.ToInt32(c.ToString())).ToList());
}

var result = HeatLoss(false);
Console.WriteLine(result);

var result2 = HeatLoss(true);
Console.WriteLine(result2);


int HeatLoss(bool isPart2)
{
	int solution = Int32.MaxValue;
	int startRow = 0;
	int startCol = 0;
	int heatLoss = 0;

	var visited = new Dictionary<Tuple<int, int, int, char>, int>();
	var moveQueue = new PriorityQueue<(int, int, int, int, char)>((x, y) => x.Item1.CompareTo(y.Item1));

	moveQueue.Enqueue((SumHeatCount(heatLoss, startRow, startCol + 1), startRow, startCol + 1, 1, 'R'));
	moveQueue.Enqueue((SumHeatCount(heatLoss, startRow + 1, startCol), startRow + 1, startCol, 1, 'D'));

	while (moveQueue.Count() > 0)
	{
		var movement = moveQueue.Dequeue();
		int heatCount = movement.Item1;
		int row = movement.Item2;
		int col = movement.Item3;
		int consecutive = movement.Item4;
		char direction = movement.Item5;

		if (visited.ContainsKey(Tuple.Create(row, col, consecutive, direction)))
		{
			continue;
		}

		visited.Add(Tuple.Create(row, col, consecutive, direction), heatCount);

		if (row == map.Count - 1 &&
			col == map[0].Count - 1)
		{
			solution = Math.Min(solution, heatCount);
			break;
		}

		if ((consecutive < 3 && !isPart2) ||
			(consecutive < 10 && isPart2))
		{
			switch (direction)
			{
				case 'R':
					if (IsValidPosition(row, col + 1))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row, col + 1), row, col + 1, consecutive + 1, direction));
					}
					break;
				case 'D':
					if (IsValidPosition(row + 1, col))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row + 1, col), row + 1, col, consecutive + 1, direction));
					}
					break;
				case 'L':
					if (IsValidPosition(row, col - 1))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row, col - 1), row, col - 1, consecutive + 1, direction));
					}
					break;
				case 'U':
					if (IsValidPosition(row - 1, col))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row - 1, col), row - 1, col, consecutive + 1, direction));
					}
					break;
			}
		}

		if (!isPart2 ||
			consecutive >= 4)
		{
			switch (direction)
			{
				case 'R':
					if (IsValidPosition(row + 1, col))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row + 1, col), row + 1, col, 1, 'D'));
					}
					if (IsValidPosition(row - 1, col))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row - 1, col), row - 1, col, 1, 'U'));
					}
					break;
				case 'D':
					if (IsValidPosition(row, col - 1))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row, col - 1), row, col - 1, 1, 'L'));
					}
					if (IsValidPosition(row, col + 1))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row, col + 1), row, col + 1, 1, 'R'));
					}
					break;
				case 'L':
					if (IsValidPosition(row + 1, col))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row + 1, col), row + 1, col, 1, 'D'));
					}
					if (IsValidPosition(row - 1, col))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row - 1, col), row - 1, col, 1, 'U'));
					}
					break;
				case 'U':
					if (IsValidPosition(row, col - 1))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row, col - 1), row, col - 1, 1, 'L'));
					}
					if (IsValidPosition(row, col + 1))
					{
						moveQueue.Enqueue((SumHeatCount(heatCount, row, col + 1), row, col + 1, 1, 'R'));
					}
					break;
			}
		}
	}

	return solution;
}


bool IsValidPosition(int row, int col)
{
	return row >= 0 &&
			row < map.Count &&
			col >= 0 &&
			col < map[0].Count;
}

int SumHeatCount(int currentHeat, int row, int col)
{
	return currentHeat + map[row][col];
}


class PriorityQueue<T>
{
	private List<T> data;
	private readonly Comparison<T> comparison;

	public PriorityQueue(Comparison<T> comparison)
	{
		this.data = new List<T>();
		this.comparison = comparison;
	}

	public void Enqueue(T item)
	{
		data.Add(item);
		int currentIndex = data.Count - 1;

		while (currentIndex > 0)
		{
			int parentIndex = (currentIndex - 1) / 2;

			if (comparison.Invoke(data[currentIndex], data[parentIndex]) >= 0)
				break;

			T tmp = data[currentIndex];
			data[currentIndex] = data[parentIndex];
			data[parentIndex] = tmp;
			currentIndex = parentIndex;
		}
	}

	public T Dequeue()
	{
		int lastIndex = data.Count - 1;
		T frontItem = data[0];
		data[0] = data[lastIndex];
		data.RemoveAt(lastIndex);

		--lastIndex;
		int parentIndex = 0;

		while (true)
		{
			int childIndex = parentIndex * 2 + 1;

			if (childIndex > lastIndex)
				break;

			int rightChildIndex = childIndex + 1;

			if (rightChildIndex <= lastIndex && comparison.Invoke(data[rightChildIndex], data[childIndex]) < 0)
				childIndex = rightChildIndex;

			if (comparison.Invoke(data[parentIndex], data[childIndex]) <= 0)
				break;

			T tmp = data[parentIndex];
			data[parentIndex] = data[childIndex];
			data[childIndex] = tmp;
			parentIndex = childIndex;
		}

		return frontItem;
	}

	public int Count()
	{
		return data.Count;
	}
}
