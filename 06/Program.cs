var races = new List<Race>();

/*
races.Add(new Race(7, 9));
races.Add(new Race(15, 40));
races.Add(new Race(30, 200));
*/

races.Add(new Race(40, 277));
races.Add(new Race(82, 1338));
races.Add(new Race(91, 1349));
races.Add(new Race(66, 1063));


var beats = new List<long>();

foreach (var race in races)
{
	beats.Add(CalculateWins(race.Lasts, race.Record));
}

//var result = beats[0] * beats[1] * beats[2];
var result = beats[0] * beats[1] * beats[2] * beats[3];
Console.WriteLine(result);


//Part 2
//var race2 = new Race(71530, 940200);
var race2 = new Race(40829166, 277133813491063);

var result2 = CalculateWins(race2.Lasts, race2.Record);
Console.WriteLine(result2);


long CalculateWins(long lasts, long record)
{
	long result = 0;
	for (long i = 0; i <= lasts; i++)
	{
		long distance = i * (lasts - i);
		if (distance > record)
		{
			result += 1;
		}
	}
	return result;
}


class Race
{
	public long Lasts { get; set; }
	public long Record { get; set; }

	public Race(long lasts, long record)
	{
		Lasts = lasts;
		Record = record;
	}
}