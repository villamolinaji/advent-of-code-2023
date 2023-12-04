string[] lines = File.ReadAllLines("Input.txt");

int red = 12;
int green = 13;
int blue = 14;

int gameId = 1;
var validGames = new List<int>();

foreach (var line in lines)
{
	var sets = line.Split(';');
	bool validGame = true;

	foreach (var set in sets)
	{
		var setRed = ReadColor(set, "red");
		var setGreen = ReadColor(set, "green");
		var setBlue = ReadColor(set, "blue");

		if (setRed > red || setGreen > green || setBlue > blue)
		{
			validGame = false;
		}
	}

	if (validGame)
	{
		validGames.Add(gameId);
	}

	gameId++;
}

var result = validGames.Sum();
Console.WriteLine(result);


//Part 2
var powers = new List<int>();
foreach (var line in lines)
{
	var sets = line.Split(';');
	var redList = new List<int>();
	var greenList = new List<int>();
	var blueList = new List<int>();

	foreach (var set in sets)
	{
		var setRed = ReadColor(set, "red");
		var setGreen = ReadColor(set, "green");
		var setBlue = ReadColor(set, "blue");

		redList.Add(setRed);
		greenList.Add(setGreen);
		blueList.Add(setBlue);
	}

	var maxRed = redList.Max();
	var maxGreen = greenList.Max();
	var maxBlue = blueList.Max();

	if (maxRed == 0) maxRed = 1;
	if (maxGreen == 0) maxGreen = 1;
	if (maxBlue == 0) maxBlue = 1;

	powers.Add(maxRed * maxGreen * maxBlue);

	gameId++;
}

var result2 = powers.Sum();
Console.WriteLine(result2);



int ReadColor(string set, string color)
{
	int result = 0;

	if (set.Contains(color))
	{
		var colorPos = set.IndexOf(color);
		string colorNumber = "";
		bool firstBlank = false;
		for (int i = colorPos; i >= 0; i--)
		{
			int number;
			if (int.TryParse(set[i].ToString(), out number))
			{
				colorNumber = colorNumber + set[i].ToString();
			}
			else if (set[i].ToString() == " ")
			{
				if (firstBlank)
				{
					break;
				}
				firstBlank = true;
			}
		}
		var colorNumberReversed = colorNumber.ToCharArray();
		Array.Reverse(colorNumberReversed);

		int.TryParse(colorNumberReversed, out result);
	}

	return result;
}
