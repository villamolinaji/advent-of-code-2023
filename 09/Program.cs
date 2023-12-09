string[] lines = File.ReadAllLines("Input.txt");

var histories = new List<History>();

foreach (var line in lines)
{
	var sequence = line.Split(' ').Select(s => Convert.ToInt32(s)).ToList();
	var history = new History(sequence);
	histories.Add(history);
}

foreach (var history in histories)
{
	history.Value = GetSequenceValue(history.Sequence, false);
}

var result = histories.Sum(h => h.Value);
Console.WriteLine(result);

// Part2
foreach (var history in histories)
{
	history.Value = GetSequenceValue(history.Sequence, true);
}

var result2 = histories.Sum(h => h.Value);
Console.WriteLine(result2);


static int GetSequenceValue(List<int> sequence, bool isPart2)
{
	var newSequence = new List<int>();
	for (int i = 0; i < sequence.Count - 1; i++)
	{
		newSequence.Add(sequence[i + 1] - sequence[i]);
	}

	if (newSequence.All(s => s == 0))
	{
		return sequence[sequence.Count - 1];
	}
	else
	{
		return sequence[isPart2 ? 0 : sequence.Count - 1] + (isPart2 ? -1 : 1) * GetSequenceValue(newSequence, isPart2);
	}
}


class History
{
	public List<int> Sequence { get; set; }

	public int Value { get; set; }

	public History(List<int> sequence)
	{
		Sequence = sequence;
		Value = 0;
	}
}
