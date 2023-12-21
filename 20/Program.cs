string[] lines = File.ReadAllLines("Input.txt");

var modules = new List<Module>();
foreach (var line in lines)
{
	char prefix = ' ';
	if (line.StartsWith('%'))
	{
		prefix = '%';
	}
	else if (line.StartsWith('&'))
	{
		prefix = '&';
	}

	var lineSplit = line.Split("->");
	string name = lineSplit[0].Trim().Replace("&", "").Replace("%", "");

	modules.Add(new Module(name, prefix, lineSplit[1].Split(",").Select(d => d.Trim()).ToList()));
}

var fromDictionary = new Dictionary<string, List<ModulePar>>();
foreach (var module in modules.Where(m => m.Prefix == '&'))
{
	fromDictionary[module.Name] = new List<ModulePar>();
	foreach (var moduleD in modules.Where(m => m.Destinations.Contains(module.Name)))
	{
		fromDictionary[module.Name].Add(new ModulePar(moduleD.Name, false));

	}
}

long lowCount = 0;
long highCount = 0;
for (int i = 0; i < 1000; i++)
{
	PushButton();
}
var result = lowCount * highCount;
Console.WriteLine(result);

// Part 2
modules = new List<Module>();
foreach (var line in lines)
{
	char prefix = ' ';
	if (line.StartsWith('%'))
	{
		prefix = '%';
	}
	else if (line.StartsWith('&'))
	{
		prefix = '&';
	}

	var lineSplit = line.Split("->");
	string name = lineSplit[0].Trim().Replace("&", "").Replace("%", "");

	modules.Add(new Module(name, prefix, lineSplit[1].Split(",").Select(d => d.Trim()).ToList()));
}

fromDictionary = new Dictionary<string, List<ModulePar>>();
foreach (var module in modules.Where(m => m.Prefix == '&'))
{
	fromDictionary[module.Name] = new List<ModulePar>();
	foreach (var moduleD in modules.Where(m => m.Destinations.Contains(module.Name)))
	{
		fromDictionary[module.Name].Add(new ModulePar(moduleD.Name, false));

	}
}

var result2 = PushButton2();
Console.WriteLine(result2);

void PushButton()
{
	var Q = new Queue<Tuple<string, bool, string>>();
	Q.Enqueue(Tuple.Create("broadcaster", false, "button"));

	while (Q.Count > 0)
	{
		var moduleQ = Q.Dequeue();
		var isCurrentHighQ = moduleQ.Item2;
		var from = moduleQ.Item3;

		if (isCurrentHighQ)
		{
			highCount++;
		}
		else
		{
			lowCount++;
		}

		var currentModule = modules.FirstOrDefault(m => m.Name == moduleQ.Item1);

		if (currentModule != null)
		{
			switch (currentModule.Prefix)
			{
				case '%':
					if (!isCurrentHighQ)
					{
						isCurrentHighQ = !currentModule.IsHigh;
						currentModule.IsHigh = !currentModule.IsHigh;
						foreach (var destination in currentModule.Destinations)
						{
							Q.Enqueue(Tuple.Create(destination, isCurrentHighQ, currentModule.Name));
						}
					}
					else
					{
						continue;
					}
					break;
				case '&':
					fromDictionary[currentModule.Name].First(m => m.Name == from).IsHigh = isCurrentHighQ;

					isCurrentHighQ = !fromDictionary[currentModule.Name].All(m => m.IsHigh);

					foreach (var destination in currentModule.Destinations)
					{
						Q.Enqueue(Tuple.Create(destination, isCurrentHighQ, currentModule.Name));
					}
					break;
				default:
					foreach (var destination in currentModule.Destinations)
					{
						Q.Enqueue(Tuple.Create(destination, isCurrentHighQ, currentModule.Name));
					}
					break;
			}
		}
	}
}

long PushButton2()
{
	var Q = new Queue<Tuple<string, bool, string>>();
	var rxPrevTimes = new List<long>();
	var times = new Dictionary<string, int>();
	var moduleCount = new Dictionary<string, int>();
	var rxPrev = new List<string> { "th", "sv", "gh", "ch" };

	for (int t = 1; t < 100000; t++)
	{
		Q.Enqueue(Tuple.Create("broadcaster", false, "button"));

		while (Q.Count > 0)
		{
			var moduleQ = Q.Dequeue();
			var isCurrentHighQ = moduleQ.Item2;
			var from = moduleQ.Item3;

			if (!isCurrentHighQ)
			{
				if (times.ContainsKey(moduleQ.Item1) && moduleCount[moduleQ.Item1] == 2 && rxPrev.Contains(moduleQ.Item1))
				{
					rxPrevTimes.Add(t - times[moduleQ.Item1]);
				}
				times[moduleQ.Item1] = t;
				if (moduleCount.ContainsKey(moduleQ.Item1))
				{
					moduleCount[moduleQ.Item1] += 1;
				}
				else
				{
					moduleCount[moduleQ.Item1] = 1;
				}
			}

			if (rxPrevTimes.Count == rxPrev.Count)
			{
				return CalculateLeastCommonMultiple(rxPrevTimes);
			}

			var currentModule = modules.FirstOrDefault(m => m.Name == moduleQ.Item1);

			if (currentModule != null)
			{
				switch (currentModule.Prefix)
				{
					case '%':
						if (!isCurrentHighQ)
						{
							isCurrentHighQ = !currentModule.IsHigh;
							currentModule.IsHigh = !currentModule.IsHigh;
							foreach (var destination in currentModule.Destinations)
							{
								Q.Enqueue(Tuple.Create(destination, isCurrentHighQ, currentModule.Name));
							}
						}
						else
						{
							continue;
						}
						break;
					case '&':
						fromDictionary[currentModule.Name].First(m => m.Name == from).IsHigh = isCurrentHighQ;

						isCurrentHighQ = !fromDictionary[currentModule.Name].All(m => m.IsHigh);

						foreach (var destination in currentModule.Destinations)
						{
							Q.Enqueue(Tuple.Create(destination, isCurrentHighQ, currentModule.Name));
						}
						break;
					default:
						foreach (var destination in currentModule.Destinations)
						{
							Q.Enqueue(Tuple.Create(destination, isCurrentHighQ, currentModule.Name));
						}
						break;
				}
			}
		}
	}

	return 0;
}

static long CalculateLeastCommonMultiple(List<long> numbers)
{
	long result = 1;
	foreach (long number in numbers)
	{
		result = (number * result) / CalculateGreatestCommonDivisor(number, result);
	}
	return result;
}

static long CalculateGreatestCommonDivisor(long a, long b)
{
	while (b != 0)
	{
		long temp = b;
		b = a % b;
		a = temp;
	}
	return a;
}

class Module
{
	public string Name { get; set; }
	public char Prefix { get; set; }
	public List<string> Destinations { get; set; }
	public bool IsHigh { get; set; }

	public Module(string name, char prefix, List<string> destinations)
	{
		Name = name;
		Prefix = prefix;
		Destinations = destinations;
		IsHigh = false;
	}
}

class ModulePar
{
	public string Name { get; set; }
	public bool IsHigh { get; set; }
	public ModulePar(string name, bool isHigh)
	{
		Name = name;
		IsHigh = isHigh;
	}
}
