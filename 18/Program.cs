string[] lines = File.ReadAllLines("Input.txt");
var plans = new List<Plan>();
foreach (var line in lines)
{
	var lineSplit = line.Split(' ');
	plans.Add(new Plan(Convert.ToChar(lineSplit[0]), Convert.ToInt32(lineSplit[1]), lineSplit[2]));
}

var border = new List<Tuple<int, int>>();
int row = 0;
int col = 0;
border.Add(Tuple.Create(row, col));
foreach (var plan in plans)
{
	for (int i = 0; i < plan.Meter; i++)
	{
		switch (plan.Direction)
		{
			case 'R':
				col++;
				break;
			case 'D':
				row++;
				break;
			case 'L':
				col--;
				break;
			case 'U':
				row--;
				break;
		}
		border.Add(Tuple.Create(row, col));
	}
}

var result = CalculateCubicMeters();
Console.WriteLine(result);

// Part2
var points = new List<(long, long)> { (0, 0) };
long numPoints = 0;
foreach (var plan in plans)
{
	int direction = int.Parse(plan.Color.Substring(plan.Color.Length - 2, 1));
	switch (direction)
	{
		case 0:
			plan.ColorDirection = 'R';
			break;
		case 1:
			plan.ColorDirection = 'D';
			break;
		case 2:
			plan.ColorDirection = 'L';
			break;
		case 3:
			plan.ColorDirection = 'U';
			break;
	}
	plan.ColorMeter = long.Parse(plan.Color.Substring(2, plan.Color.Length - 4), System.Globalization.NumberStyles.HexNumber);
}

foreach (var plan in plans)
{
	numPoints += plan.ColorMeter;
	var (r, c) = points[^1];

	switch (plan.ColorDirection)
	{
		case 'R':
			points.Add((r, c + plan.ColorMeter));
			break;
		case 'D':
			points.Add((r + plan.ColorMeter, c));
			break;
		case 'L':
			points.Add((r, c - plan.ColorMeter));
			break;
		case 'U':
			points.Add((r - plan.ColorMeter, c));
			break;
	}
}

var result2 = CalculateCubicMeters2();
Console.WriteLine(result2);


double CalculateCubicMeters()
{
	double area = Area(border.Select(b => (long)b.Item1).ToList(), border.Select(b => (long)b.Item2).ToList());
	int points = border.Distinct().Count();

	double I = area + 1 - points / 2;
	return I + points;
}

static double Area(List<long> x, List<long> y)
{
	return 0.5 * Math.Abs(Enumerable.Range(0, x.Count).Sum(i => x[i] * y[(i + 1) % x.Count]) - Enumerable.Range(0, y.Count).Sum(i => y[i] * x[(i + 1) % y.Count]));
}

double CalculateCubicMeters2()
{
	double area = Area(points.Select(p => p.Item1).ToList(), points.Select(p => p.Item2).ToList());
	double I = area + 1 - numPoints / 2;

	return I + numPoints;
}


class Plan
{
	public char Direction { get; set; }
	public int Meter { get; set; }
	public string Color { get; set; }
	public char ColorDirection { get; set; }
	public long ColorMeter { get; set; }


	public Plan(char direction, int meter, string color)
	{
		Direction = direction;
		Meter = meter;
		Color = color;
	}
}