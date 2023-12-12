string[] lines = File.ReadAllLines("Input.txt");

int row = 0;
var loopItems = new List<Loop>();

foreach (var line in lines)
{
	int col = 0;

	foreach (var pipe in line)
	{
		loopItems.Add(new Loop(col, row, pipe));
		col++;
	}

	row++;
}

foreach (var loop in loopItems)
{
	switch (loop.Pipe)
	{
		case '|':
			loop.NextLoops.Add(new Point(loop.X, loop.Y - 1));
			loop.NextLoops.Add(new Point(loop.X, loop.Y + 1));
			break;
		case '-':
			loop.NextLoops.Add(new Point(loop.X - 1, loop.Y));
			loop.NextLoops.Add(new Point(loop.X + 1, loop.Y));
			break;
		case 'L':
			loop.NextLoops.Add(new Point(loop.X, loop.Y - 1));
			loop.NextLoops.Add(new Point(loop.X + 1, loop.Y));
			break;
		case 'J':
			loop.NextLoops.Add(new Point(loop.X, loop.Y - 1));
			loop.NextLoops.Add(new Point(loop.X - 1, loop.Y));
			break;
		case '7':
			loop.NextLoops.Add(new Point(loop.X - 1, loop.Y));
			loop.NextLoops.Add(new Point(loop.X, loop.Y + 1));
			break;
		case 'F':
			loop.NextLoops.Add(new Point(loop.X + 1, loop.Y));
			loop.NextLoops.Add(new Point(loop.X, loop.Y + 1));
			break;
		default:
			break;
	}
}


var startLoop = loopItems.First(l => l.Pipe == 'S');

int distance = 0;
CalculateDistance(startLoop.X, startLoop.Y, distance);

var result = loopItems.Max(l => l.Distance);
Console.WriteLine(result);


//Part 2
startLoop.Pipe = GetStartPointPipe(startLoop.X, startLoop.Y);

var newMap = new List<List<char>>();
int maxY = loopItems.Max(l => l.Y) + 1;
int maxX = loopItems.Max(l => l.X) + 1;
int newMaxY = 3 * maxY;
int newMaxX = 3 * maxX;
FillNewMap();

var openList = new HashSet<Tuple<int, int>>();
CalculateOpenPositions();

int result2 = CountEnclosed();
Console.WriteLine(result2);


void CalculateDistance(int startX, int startY, int distance)
{
	var loopQueue = new Queue<LoopQueue>();

	var nextLoopItem = loopItems.FirstOrDefault(l => l.X == startX - 1 && l.Y == startY);
	if (nextLoopItem != null &&
		(nextLoopItem.Pipe == '-' || nextLoopItem.Pipe == 'L' || nextLoopItem.Pipe == 'F'))
	{
		loopQueue.Enqueue(new LoopQueue(startX - 1, startY, distance + 1));
	}

	nextLoopItem = loopItems.FirstOrDefault(l => l.X == startX && l.Y == startY - 1);
	if (nextLoopItem != null &&
		(nextLoopItem.Pipe == '|' || nextLoopItem.Pipe == '7' || nextLoopItem.Pipe == 'F'))
	{
		loopQueue.Enqueue(new LoopQueue(startX, startY - 1, distance + 1));
	}

	nextLoopItem = loopItems.FirstOrDefault(l => l.X == startX && l.Y == startY + 1);
	if (nextLoopItem != null &&
		(nextLoopItem.Pipe == '|' || nextLoopItem.Pipe == 'L' || nextLoopItem.Pipe == 'J'))
	{
		loopQueue.Enqueue(new LoopQueue(startX, startY + 1, distance + 1));
	}

	nextLoopItem = loopItems.FirstOrDefault(l => l.X == startX + 1 && l.Y == startY);
	if (nextLoopItem != null &&
		(nextLoopItem.Pipe == '-' || nextLoopItem.Pipe == 'J' || nextLoopItem.Pipe == '7'))
	{
		loopQueue.Enqueue(new LoopQueue(startX + 1, startY, distance + 1));
	}


	while (loopQueue.Count > 0)
	{
		var loopQueueItem = loopQueue.Dequeue();

		var loopItem = loopItems.FirstOrDefault(l => l.X == loopQueueItem.X && l.Y == loopQueueItem.Y);

		if (loopItem != null &&
			loopItem.Pipe != '.' &&
			loopItem.Distance == -1)
		{
			loopItem.Distance = loopQueueItem.Distance;
			foreach (var nextLoop in loopItem.NextLoops)
			{
				loopQueue.Enqueue(new LoopQueue(nextLoop.X, nextLoop.Y, loopQueueItem.Distance + 1));
			}
		}
	}
}

char GetStartPointPipe(int startX, int startY)
{
	char pipe = '.';

	bool isLeft = (
		loopItems.Find(l => l.X == startX - 1 && l.Y == startY)?.Pipe == '-' ||
		loopItems.Find(l => l.X == startX - 1 && l.Y == startY)?.Pipe == 'L' ||
		loopItems.Find(l => l.X == startX - 1 && l.Y == startY)?.Pipe == 'F');
	bool isUp = (
		loopItems.Find(l => l.X == startX && l.Y == startY - 1)?.Pipe == '|' ||
		loopItems.Find(l => l.X == startX && l.Y == startY - 1)?.Pipe == '7' ||
		loopItems.Find(l => l.X == startX && l.Y == startY - 1)?.Pipe == 'F');
	bool isRight = (
		loopItems.Find(l => l.X == startX + 1 && l.Y == startY)?.Pipe == '-' ||
		loopItems.Find(l => l.X == startX + 1 && l.Y == startY)?.Pipe == '7' ||
		loopItems.Find(l => l.X == startX + 1 && l.Y == startY)?.Pipe == 'J');
	bool isDown = (
		loopItems.Find(l => l.X == startX && l.Y == startY + 1)?.Pipe == '|' ||
		loopItems.Find(l => l.X == startX && l.Y == startY + 1)?.Pipe == 'L' ||
		loopItems.Find(l => l.X == startX && l.Y == startY + 1)?.Pipe == 'J');

	if (isUp && isDown)
	{
		pipe = '|';
	}
	else if (isUp && isRight)
	{
		pipe = 'L';
	}
	else if (isUp && isLeft)
	{
		pipe = 'J';
	}
	else if (isDown && isRight)
	{
		pipe = 'F';
	}
	else if (isDown && isLeft)
	{
		pipe = '7';
	}
	else if (isLeft && isRight)
	{
		pipe = '-';
	}

	return pipe;
}

void FillNewMap()
{
	for (int y = 0; y < newMaxY; y++)
	{
		newMap.Add(new List<char>(new string('.', newMaxX).ToCharArray()));
	}

	for (int y = 0; y < maxY; y++)
	{
		for (int x = 0; x < maxX; x++)
		{
			var auxLoopItem = loopItems.First(l => l.X == x && l.Y == y);
			switch (auxLoopItem.Pipe)
			{
				case '|':
					newMap[3 * y + 0][3 * x + 1] = 'X';
					newMap[3 * y + 1][3 * x + 1] = 'X';
					newMap[3 * y + 2][3 * x + 1] = 'X';
					break;
				case '-':
					newMap[3 * y + 1][3 * x + 0] = 'X';
					newMap[3 * y + 1][3 * x + 1] = 'X';
					newMap[3 * y + 1][3 * x + 2] = 'X';
					break;
				case 'L':
					newMap[3 * y + 0][3 * x + 1] = 'X';
					newMap[3 * y + 1][3 * x + 1] = 'X';
					newMap[3 * y + 1][3 * x + 2] = 'X';
					break;
				case 'J':
					newMap[3 * y + 1][3 * x + 0] = 'X';
					newMap[3 * y + 1][3 * x + 1] = 'X';
					newMap[3 * y + 0][3 * x + 1] = 'X';
					break;
				case '7':
					newMap[3 * y + 1][3 * x + 0] = 'X';
					newMap[3 * y + 1][3 * x + 1] = 'X';
					newMap[3 * y + 2][3 * x + 1] = 'X';
					break;
				case 'F':
					newMap[3 * y + 2][3 * x + 1] = 'X';
					newMap[3 * y + 1][3 * x + 1] = 'X';
					newMap[3 * y + 1][3 * x + 2] = 'X';
					break;
				default:
					break;
			}
		}
	}
}

void CalculateOpenPositions()
{
	var pointQ = new Queue<Tuple<int, int>>();

	for (int y = 0; y < newMaxY; y++)
	{
		pointQ.Enqueue(Tuple.Create(y, 0));
		pointQ.Enqueue(Tuple.Create(y, newMaxX - 1));
	}
	for (int x = 0; x < newMaxX; x++)
	{
		pointQ.Enqueue(Tuple.Create(0, x));
		pointQ.Enqueue(Tuple.Create(newMaxY - 1, x));
	}

	while (pointQ.Count > 0)
	{
		var currentPoint = pointQ.Dequeue();

		int currentY = currentPoint.Item1;
		int currentX = currentPoint.Item2;

		if (currentY < 0 ||
			currentX < 0 ||
			currentY >= newMaxY ||
			currentX >= newMaxX)
		{
			continue;
		}

		if (openList.Contains(Tuple.Create(currentY, currentX)))
		{
			continue;
		}

		openList.Add(Tuple.Create(currentY, currentX));

		if (newMap[currentY][currentX] == 'X')
		{
			continue;
		}

		pointQ.Enqueue(Tuple.Create(currentY, currentX - 1));
		pointQ.Enqueue(Tuple.Create(currentY - 1, currentX));
		pointQ.Enqueue(Tuple.Create(currentY, currentX + 1));
		pointQ.Enqueue(Tuple.Create(currentY + 1, currentX));
	}
}

int CountEnclosed()
{
	int count = 0;
	for (int y = 0; y < maxY; y++)
	{
		for (int x = 0; x < maxX; x++)
		{
			bool isOpen = false;

			for (int yy = 0; yy < 3; yy++)
			{
				for (int xx = 0; xx < 3; xx++)
				{
					if (openList.Contains(Tuple.Create(3 * y + yy, 3 * x + xx)))
					{
						isOpen = true;
						break;
					}
				}
				if (isOpen)
				{
					break;
				}
			}

			if (!isOpen)
			{
				count++;
			}
		}
	}

	return count;
}


class Loop
{
	public int X { get; set; }
	public int Y { get; set; }
	public char Pipe { get; set; }
	public int Distance { get; set; }
	public List<Point> NextLoops { get; set; }

	public Loop(int x, int y, char pipe)
	{
		X = x;
		Y = y;
		Pipe = pipe;
		Distance = -1;
		NextLoops = new List<Point>();
	}
}

class Point
{
	public int X { get; set; }
	public int Y { get; set; }

	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
}

class LoopQueue
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Distance { get; set; }

	public LoopQueue(int x, int y, int distance)
	{
		X = x;
		Y = y;
		Distance = distance;
	}
}
