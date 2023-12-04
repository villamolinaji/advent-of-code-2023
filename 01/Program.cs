
string[] lines = File.ReadAllLines("Input.txt");

// Part 1
var numbers = new List<int>();
foreach (var line in lines)
{
	int first = firstNumber(line);
	int last = lastNumber(line);

	string lineNumber = first.ToString() + last.ToString();
	numbers.Add(Convert.ToInt32(lineNumber));
}

int sum = numbers.Sum();
Console.WriteLine(sum);

// Part 2
var numbers2 = new List<int>();
foreach (var line in lines)
{
	var first = firstNumber2(line);
	var last = lastNumber2(line);

	var firsts = firstStringNumber(line);
	var lasts = lastStringNumber(line);

	var firstStringPos =
		firsts.Any(f => f.Pos >= 0)
		? firsts.Where(f => f.Pos >= 0).OrderBy(f => f.Pos).Min(f => f.Pos)
		: line.Length;
	var lastStringPos =
		lasts.Any(f => f.Pos >= 0)
		? lasts.Where(f => f.Pos >= 0).OrderBy(f => f.Pos).Max(f => f.Pos)
		: -1;

	int firstNum = 0;
	int lastNum = 0;

	if (first.Pos < firstStringPos)
	{
		firstNum = first.Number;
	}
	else
	{
		firstNum = firsts.Where(f => f.Pos >= 0).OrderBy(f => f.Pos).First().Number;
	}

	if (last.Pos > lastStringPos)
	{
		lastNum = last.Number;
	}
	else
	{
		lastNum = lasts.Where(f => f.Pos >= 0).OrderBy(f => f.Pos).Last().Number;
	}

	string lineNumber = firstNum.ToString() + lastNum.ToString();
	numbers2.Add(Convert.ToInt32(lineNumber));
}

int sum2 = numbers2.Sum();
Console.WriteLine(sum2);


int firstNumber(string line)
{
	int result = 0;


	foreach (char c in line)
	{
		if (int.TryParse(c.ToString(), out result))
		{
			break;
		}
	}

	return result;
}

int lastNumber(string line)
{
	int result = 0;
	var lineReversed = line.ToCharArray();
	Array.Reverse(lineReversed);

	foreach (char c in lineReversed)
	{
		if (int.TryParse(c.ToString(), out result))
		{
			break;
		}
	}

	return result;
}

PosNumber firstNumber2(string line)
{
	int result = 0;
	int i = 0;

	foreach (char c in line)
	{
		if (int.TryParse(c.ToString(), out result))
		{
			break;
		}
		i++;
	}

	return new PosNumber(i, result);
}

PosNumber lastNumber2(string line)
{
	int result = 0;
	int i = line.Length - 1;
	var lineReversed = line.ToCharArray();
	Array.Reverse(lineReversed);

	foreach (char c in lineReversed)
	{
		if (int.TryParse(c.ToString(), out result))
		{
			break;
		}
		i--;
	}

	return new PosNumber(i, result);
}


List<PosNumber> firstStringNumber(string line)
{
	var firsts = new List<PosNumber>();
	firsts.Add(new PosNumber(line.IndexOf("one"), 1));
	firsts.Add(new PosNumber(line.IndexOf("two"), 2));
	firsts.Add(new PosNumber(line.IndexOf("three"), 3));
	firsts.Add(new PosNumber(line.IndexOf("four"), 4));
	firsts.Add(new PosNumber(line.IndexOf("five"), 5));
	firsts.Add(new PosNumber(line.IndexOf("six"), 6));
	firsts.Add(new PosNumber(line.IndexOf("seven"), 7));
	firsts.Add(new PosNumber(line.IndexOf("eight"), 8));
	firsts.Add(new PosNumber(line.IndexOf("nine"), 9));
	return firsts;
}

List<PosNumber> lastStringNumber(string line)
{
	var lasts = new List<PosNumber>();
	lasts.Add(new PosNumber(line.LastIndexOf("one"), 1));
	lasts.Add(new PosNumber(line.LastIndexOf("two"), 2));
	lasts.Add(new PosNumber(line.LastIndexOf("three"), 3));
	lasts.Add(new PosNumber(line.LastIndexOf("four"), 4));
	lasts.Add(new PosNumber(line.LastIndexOf("five"), 5));
	lasts.Add(new PosNumber(line.LastIndexOf("six"), 6));
	lasts.Add(new PosNumber(line.LastIndexOf("seven"), 7));
	lasts.Add(new PosNumber(line.LastIndexOf("eight"), 8));
	lasts.Add(new PosNumber(line.LastIndexOf("nine"), 9));
	return lasts;
}



class PosNumber
{
	public int Pos { get; set; }
	public int Number { get; set; }

	public PosNumber(int pos, int number)
	{
		Pos = pos;
		Number = number;
	}
}