
string[] lines = File.ReadAllLines("Input.txt");

var directions = string.Empty;
var nodes = new List<Node>();
foreach (var line in lines)
{
	if ((line.StartsWith("L") || line.StartsWith("R")) &&
		!line.Contains("="))
	{
		directions = directions + line;
	}
	else if (line != "")
	{
		var lineSplit = line.Split(" = ");
		var lineSplitLR = lineSplit[1].Split(", ");
		nodes.Add(new Node(
			lineSplit[0],
			lineSplitLR[0].Substring(1, lineSplitLR[0].Length - 1),
			lineSplitLR[1].Substring(0, lineSplitLR[1].Length - 1)));
	}
}


int directionIndex = 0;
int steps = 0;

var nextNode = nodes.First(n => n.NodeName == "AAA");
while (nextNode.NodeName != "ZZZ")
{
	nextNode = GetNextNode(nextNode);
	steps++;
}

Console.WriteLine(steps);


//Part2
directionIndex = 0;
var steps2 = SolvePart2();

Console.WriteLine(steps2);


Node GetNextNode(Node currentNode)
{
	var currentDirection = directions[directionIndex];

	if (directionIndex == directions.Length - 1)
	{
		directionIndex = 0;
	}
	else
	{
		directionIndex++;
	}

	var nextNode = string.Empty;

	if (currentDirection.ToString() == "L")
	{
		nextNode = currentNode.NodeLeft;
	}
	else
	{
		nextNode = currentNode.NodeRight;
	}

	return nodes.First(n => n.NodeName == nextNode);
}


long SolvePart2()
{
	List<Node> currentNodes = nodes.Where(n => n.NodeName.EndsWith("A")).ToList();

	Dictionary<int, long> stepsNode = new Dictionary<int, long>();
	long stepsCount = 0;

	while (true)
	{
		List<Node> nextNodes = new List<Node>();
		var currentDirection = directions[directionIndex];
		if (directionIndex == directions.Length - 1)
		{
			directionIndex = 0;
		}
		else
		{
			directionIndex++;
		}

		for (int i = 0; i < currentNodes.Count; i++)
		{
			var nextNode = string.Empty;
			if (currentDirection.ToString() == "L")
			{
				nextNode = currentNodes[i].NodeLeft;
			}
			else
			{
				nextNode = currentNodes[i].NodeRight;
			}

			if (nextNode.EndsWith("Z"))
			{
				stepsNode[i] = stepsCount + 1;

				if (stepsNode.Count == currentNodes.Count)
				{
					return CalculateLeastCommonMultiple(stepsNode.Values.ToArray());
				}
			}
			nextNodes.Add(nodes.First(n => n.NodeName == nextNode));
		}

		currentNodes = nextNodes;
		stepsCount += 1;
	}
}

long CalculateLeastCommonMultiple(long[] numbers)
{
	long result = 1;
	foreach (long number in numbers)
	{
		result = (number * result) / CalculateGreatestCommonDivisor(number, result);
	}
	return result;
}

long CalculateGreatestCommonDivisor(long a, long b)
{
	while (b != 0)
	{
		long temp = b;
		b = a % b;
		a = temp;
	}
	return a;
}


class Node
{
	public string NodeName { get; set; }
	public string NodeLeft { get; set; }
	public string NodeRight { get; set; }

	public Node(string nodeName, string nodeLeft, string nodeRight)
	{
		NodeName = nodeName;
		NodeLeft = nodeLeft;
		NodeRight = nodeRight;
	}
}