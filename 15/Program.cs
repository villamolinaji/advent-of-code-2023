
string[] lines = File.ReadAllLines("Input.txt");

var sequences = new List<Sequence>();

foreach (var line in lines)
{
	var lineSplit = line.Split(',');
	foreach (var sequence in lineSplit)
	{
		sequences.Add(new Sequence(sequence));
	}
}

foreach (var sequence in sequences)
{
	sequence.SequenceValue = CalculateHASH(sequence.SequenceString);
}

var result = sequences.Sum(s => s.SequenceValue);
Console.WriteLine(result);

// Part 2
var boxes = new List<Box>();
for (int i = 0; i < 256; i++)
{
	boxes.Add(new Box(i));
}

foreach (var sequence in sequences)
{
	if (sequence.SequenceString.Contains('='))
	{
		var stringSplit = sequence.SequenceString.Split('=');
		int hash = CalculateHASH(stringSplit[0]);
		var box = boxes.FirstOrDefault(b => b.Index == hash);
		if (box != null)
		{
			if (box.Sequences.Any(s => s.Item1 == stringSplit[0]))
			{
				box.Sequences = box.Sequences.Select(s => (s.Item1, s.Item1 == stringSplit[0] ? Convert.ToInt32(stringSplit[1]) : s.Item2)).ToList();
			}
			else
			{
				box.Sequences.Add((stringSplit[0], Convert.ToInt32(stringSplit[1])));
			}
		}
	}
	else if (sequence.SequenceString.Contains('-'))
	{
		var stringSplit = sequence.SequenceString.Split('-');
		var box = boxes.FirstOrDefault(b => b.Sequences.Any(s => s.Item1 == stringSplit[0]));
		if (box != null)
		{
			box.Sequences = box.Sequences.Where(s => s.Item1 != stringSplit[0]).ToList();
		}
	}
}


var result2 = CalculatePower();
Console.WriteLine(result2);


int CalculateHASH(string sequenceString)
{
	int value = 0;

	foreach (var c in sequenceString)
	{
		int ascii = Convert.ToInt32(c);
		value += ascii;
		value *= 17;
		value %= 256;
	}

	return value;
}

int CalculatePower()
{
	int value = 0;

	foreach (var box in boxes)
	{
		var slotCount = 1;
		foreach (var slot in box.Sequences)
		{
			int focal = slot.Item2;
			value += ((box.Index + 1) * slotCount * focal);
			slotCount++;
		}
	}

	return value;
}

class Sequence
{
	public string SequenceString { get; set; }
	public int SequenceValue { get; set; }
	public int Power { get; set; }

	public Sequence(string sequenceString)
	{
		SequenceString = sequenceString;
		SequenceValue = 0;
		Power = 0;
	}
}

class Box
{
	public List<(string, int)> Sequences { get; set; }
	public int Index { get; set; }

	public Box(int index)
	{
		Sequences = new List<(string, int)>();
		Index = index;
	}
}