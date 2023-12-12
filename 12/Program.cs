string[] lines = File.ReadAllLines("Input.txt");

string options = ".#";

var rows = new List<Row>();
foreach (var line in lines)
{
	var lineSplit = line.Split(' ');
	rows.Add(new Row(lineSplit[0], lineSplit[1].Split(',').Select(x => Convert.ToInt32(x)).ToList()));
}

Dictionary<(int, int, int), long> resolvedOptions = new Dictionary<(int, int, int), long>();
foreach (var row in rows)
{
	resolvedOptions.Clear();
	row.Arrangements = CalculateArrangements(row, 0, 0, 0);
}

var result = rows.Sum(r => r.Arrangements);
Console.WriteLine(result);


// Part 2
rows.Clear();
foreach (var line in lines)
{
	var lineSplit = line.Split(' ');
	var lineDuplicated = string.Join("?", lineSplit[0], lineSplit[0], lineSplit[0], lineSplit[0], lineSplit[0]);
	var groupsDuplicated = string.Join(",", lineSplit[1], lineSplit[1], lineSplit[1], lineSplit[1], lineSplit[1]);

	rows.Add(new Row(lineDuplicated, groupsDuplicated.Split(',').Select(x => Convert.ToInt32(x)).ToList()));
}

foreach (var row in rows)
{
	resolvedOptions.Clear();
	row.Arrangements = CalculateArrangements(row, 0, 0, 0);
}
var result2 = rows.Sum(r => r.Arrangements);
Console.WriteLine(result2);


long CalculateArrangements(Row row, int lineIndex, int groupIndex, int current)
{
	var key = (lineIndex, groupIndex, current);
	if (resolvedOptions.ContainsKey(key))
	{
		return resolvedOptions[key];
	}

	if (lineIndex == row.Line.Length)
	{
		if (groupIndex == row.Groups.Count &&
			current == 0)
		{
			return 1;
		}
		else if (groupIndex == row.Groups.Count - 1 &&
			row.Groups[groupIndex] == current)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}

	long arrangements = 0;

	foreach (var option in options)
	{
		if (row.Line[lineIndex] == option ||
			row.Line[lineIndex] == '?')
		{
			if (option == '.' &&
				current == 0)
			{
				arrangements += CalculateArrangements(row, lineIndex + 1, groupIndex, 0);
			}
			else if (option == '.' &&
				current > 0 &&
				groupIndex < row.Groups.Count &&
				row.Groups[groupIndex] == current)
			{
				arrangements += CalculateArrangements(row, lineIndex + 1, groupIndex + 1, 0);
			}
			else if (option == '#')
			{
				arrangements += CalculateArrangements(row, lineIndex + 1, groupIndex, current + 1);
			}
		}
	}

	resolvedOptions[key] = arrangements;

	return arrangements;
}


class Row
{
	public string Line { get; set; }
	public List<int> Groups { get; set; }
	public long Arrangements { get; set; }
	public Row(string line, List<int> groups)
	{
		Line = line;
		Groups = groups;
		Arrangements = 0;
	}
}
