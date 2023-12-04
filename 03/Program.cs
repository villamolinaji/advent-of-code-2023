string[] lines = File.ReadAllLines("Input.txt");
var numbers = new List<int>();
string prevChar = string.Empty;
string currentNumber = string.Empty;
bool isValid = false;

int rowNumber = 0;
var gearNumbers = new Dictionary<(int, int), List<int>>();

foreach (var line in lines)
{
	var gears = new HashSet<(int, int)>();

	for (int i = 0; i <= line.Length; i++)
	{
		int auxNumber;
		if (i < line.Length && Int32.TryParse(line[i].ToString(), out auxNumber))
		{
			currentNumber = currentNumber + line[i].ToString();
			if (!isValid)
			{
				isValid = IsValid(rowNumber, i, lines);
				CheckGear(rowNumber, i, lines, gears);
			}
		}
		else
		{
			if (isValid)
			{
				numbers.Add(Convert.ToInt32(currentNumber));
			}

			foreach (var gear in gears)
			{
				if (!gearNumbers.ContainsKey(gear))
				{
					gearNumbers[gear] = new List<int>();
				}
				gearNumbers[gear].Add((Convert.ToInt32(currentNumber)));
			}

			isValid = false;
			currentNumber = string.Empty;
			gears.Clear();
		}
	}
	rowNumber++;
}

var result = numbers.Sum();
Console.WriteLine(result);

var result2 = gearNumbers
	.Where(pair => pair.Value.Count == 2)
	.Sum(pair => pair.Value[0] * pair.Value[1]);
Console.WriteLine(result2);


bool IsValid(int rowNumber, int colNumber, string[] lines)
{
	bool isValid = false;

	if (rowNumber > 0)
	{
		if (colNumber > 0)
		{
			if (IsSymbol(rowNumber - 1, colNumber - 1, lines))
			{
				return true;
			}
		}
		if (IsSymbol(rowNumber - 1, colNumber, lines))
		{
			return true;
		}
		if (colNumber < lines[rowNumber - 1].Length - 1)
		{
			if (IsSymbol(rowNumber - 1, colNumber + 1, lines))
			{
				return true;
			}
		}
	}

	if (colNumber > 0)
	{
		if (IsSymbol(rowNumber, colNumber - 1, lines))
		{
			return true;
		}
	}
	if (colNumber < lines[rowNumber].Length - 1)
	{
		if (IsSymbol(rowNumber, colNumber + 1, lines))
		{
			return true;
		}
	}

	if (rowNumber < lines.Length - 1)
	{
		if (colNumber > 0)
		{
			if (IsSymbol(rowNumber + 1, colNumber - 1, lines))
			{
				return true;
			}
		}
		if (IsSymbol(rowNumber + 1, colNumber, lines))
		{
			return true;
		}
		if (colNumber < lines[rowNumber + 1].Length - 1)
		{
			if (IsSymbol(rowNumber + 1, colNumber + 1, lines))
			{
				return true;
			}
		}
	}


	return isValid;
}

bool CheckGear(int rowNumber, int colNumber, string[] lines, HashSet<(int, int)> gears)
{
	bool isValid = false;

	if (rowNumber > 0)
	{
		if (colNumber > 0)
		{
			if (IsGear(rowNumber - 1, colNumber - 1, lines))
			{
				gears.Add((rowNumber - 1, colNumber - 1));
			}
		}
		if (IsGear(rowNumber - 1, colNumber, lines))
		{
			gears.Add((rowNumber - 1, colNumber));
		}
		if (colNumber < lines[rowNumber - 1].Length - 1)
		{
			if (IsGear(rowNumber - 1, colNumber + 1, lines))
			{
				gears.Add((rowNumber - 1, colNumber + 1));
			}
		}
	}

	if (colNumber > 0)
	{
		if (IsGear(rowNumber, colNumber - 1, lines))
		{
			gears.Add((rowNumber, colNumber - 1));
		}
	}
	if (colNumber < lines[rowNumber].Length - 1)
	{
		if (IsGear(rowNumber, colNumber + 1, lines))
		{
			gears.Add((rowNumber, colNumber + 1));
		}
	}

	if (rowNumber < lines.Length - 1)
	{
		if (colNumber > 0)
		{
			if (IsGear(rowNumber + 1, colNumber - 1, lines))
			{
				gears.Add((rowNumber + 1, colNumber - 1));
			}
		}
		if (IsGear(rowNumber + 1, colNumber, lines))
		{
			gears.Add((rowNumber + 1, colNumber));
		}
		if (colNumber < lines[rowNumber + 1].Length - 1)
		{
			if (IsGear(rowNumber + 1, colNumber + 1, lines))
			{
				gears.Add((rowNumber + 1, colNumber + 1));
			}
		}
	}


	return isValid;
}

bool IsSymbol(int x, int y, string[] lines)
{
	string aux = lines[x][y].ToString();
	int auxNumber;
	if (aux != "." && !Int32.TryParse(aux, out auxNumber))
	{
		return true;
	}
	return false;
}

bool IsGear(int x, int y, string[] lines)
{
	string aux = lines[x][y].ToString();
	return aux == "*";
}