string[] lines = File.ReadAllLines("Input.txt");
var points = new List<int>();

Dictionary<int, int> instances = new Dictionary<int, int>();
int lineNumber = 0;

foreach (var line in lines)
{
	var winingNumbers = new List<int>();
	var myNumbers = new List<int>();

	var lineSplit = line.Split('|');

	var winStr = lineSplit[0].Substring(lineSplit[0].IndexOf(":") + 1);
	var myStr = lineSplit[1];

	var winStrNumbers = winStr.Split(" ");
	var myStrNumbers = myStr.Split(" ");

	foreach (var number in winStrNumbers)
	{
		int outNumber = 0;
		if (Int32.TryParse(number, out outNumber))
		{
			winingNumbers.Add(outNumber);
		}
	}
	foreach (var number in myStrNumbers)
	{
		int outMyNumber = 0;
		if (Int32.TryParse(number, out outMyNumber))
		{
			myNumbers.Add(outMyNumber);
		}
	}

	int countWin = myNumbers.Where(n => winingNumbers.Contains(n)).Count();

	if (countWin > 0)
	{
		points.Add((int)Math.Pow(2, countWin - 1));
	}

	//Part 2
	if (instances.ContainsKey(lineNumber))
	{
		instances[lineNumber] += 1;
	}
	else
	{
		instances[lineNumber] = 1;
	}

	for (int i = 0; i < countWin; i++)
	{
		if (instances.ContainsKey(lineNumber + 1 + i))
		{
			instances[lineNumber + 1 + i] += instances[lineNumber];
		}
		else
		{
			instances[lineNumber + 1 + i] = instances[lineNumber];
		}
	}

	lineNumber++;
}


var result = points.Sum();
//Console.WriteLine(result);

var result2 = instances.Values.Sum();
Console.WriteLine(result2);