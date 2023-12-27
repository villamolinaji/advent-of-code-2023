using System.Numerics;
using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("Input.txt");

var hailstones = new List<Hailstone>();
foreach (var line in lines)
{
	var lineSplit = line.Split('@');
	var lineSplit1 = lineSplit[0].Split(", ");
	var lineSplit2 = lineSplit[1].Split(", ");

	hailstones.Add(new Hailstone(
		long.Parse(lineSplit1[0]),
		long.Parse(lineSplit1[1]),
		long.Parse(lineSplit1[2]),
		long.Parse(lineSplit2[0]),
		long.Parse(lineSplit2[1]),
		long.Parse(lineSplit2[2])));
}

//long minArea = 7;
//long maxArea = 27;
long minArea = 200000000000000;
long maxArea = 400000000000000;

int futureIntersect = 0;

for (int i = 0; i < hailstones.Count; i++)
{
	for (int j = i + 1; j < hailstones.Count; j++)
	{
		var (posX, posY, t1, t2) = GetIntersect(hailstones[i], hailstones[j]);
		if (minArea <= posX &&
			posX <= maxArea &&
			minArea <= posY &&
			posY <= maxArea &&
			t1 >= 0
			&& t2 >= 0)
		{
			futureIntersect++;
		}
	}
}

Console.WriteLine(futureIntersect);

// Part 2
var particles = ParseParticles3(File.ReadAllText("Input.txt"));
var result2 = Solve(v => v.x, particles) + Solve(v => v.y, particles) + Solve(v => v.z, particles);
Console.WriteLine(result2);


Tuple<long, long, long, long> GetIntersect(Hailstone h1, Hailstone h2)
{
	try
	{
		long t1 = (h2.VX * (h1.Y - h2.Y) + h2.X * h2.VY - h1.X * h2.VY) / (h2.VY * h1.VX - h2.VX * h1.VY);
		long t2 = (h1.VX * (h2.Y - h1.Y) + h1.X * h1.VY - h2.X * h1.VY) / (h1.VY * h2.VX - h1.VX * h2.VY);
		return Tuple.Create(h1.X + h1.VX * t1, h1.Y + h1.VY * t1, t1, t2);
	}
	catch (DivideByZeroException)
	{
		return Tuple.Create((long)-1, (long)-1, (long)-1, (long)-1);
	}
}

Particle3[] ParseParticles3(string input) => (
		from line in input.Split('\n')
		let v = Regex.Matches(line, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).ToArray()
		select new Particle3(new Vec3(v[0], v[1], v[2]), new Vec3(v[3], v[4], v[5]))
	).ToArray();

BigInteger Solve(Func<Vec3, BigInteger> dim, Particle3[] particles)
{
	for (var v0 = -10000; v0 < 10000; v0++)
	{
		var items = new List<(BigInteger dv, BigInteger x)>();
		foreach (var p in particles)
		{
			var dv = v0 - dim(p.vel);
			if (IsPrime(dv) && items.All(i => i.dv != dv))
			{
				items.Add((dv: dv, x: dim(p.pos)));
			}
		}
		if (items.Count > 1)
		{
			var p0 = ChineseRemainderTheorem(items.ToArray());
			var ok = true;
			foreach (var p in particles)
			{
				var dv = v0 - dim(p.vel);
				var dx = dim(p.pos) > p0 ? dim(p.pos) - p0 : p0 - dim(p.pos);
				if (dv == 0)
				{
					if (dx != 0)
					{
						ok = false;
					}
				}
				else
				{
					if (dx % dv != 0)
					{
						ok = false;
					}
				}
			}
			if (ok)
			{
				return p0;
			}
		}
	}
	throw new Exception();
}

bool IsPrime(BigInteger number)
{
	if (number <= 2)
	{
		return false;
	}
	if (number % 2 == 0)
	{
		return false;
	}

	for (int i = 3; i * i <= number; i += 2)
	{
		if (number % i == 0)
		{
			return false;
		}
	}

	return true;
}

BigInteger ChineseRemainderTheorem((BigInteger mod, BigInteger a)[] items)
{
	var prod = items.Aggregate(BigInteger.One, (acc, item) => acc * item.mod);
	var sum = items.Select((item, i) =>
	{
		var p = prod / item.mod;
		return item.a * ModInv(p, item.mod) * p;
	});

	var s = BigInteger.Zero;
	foreach (var item in sum)
	{
		s += item;
	}

	return s % prod;
}

BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);


record Particle3(Vec3 pos, Vec3 vel);

record Vec3(BigInteger x, BigInteger y, BigInteger z)
{
	public static Vec3 operator +(Vec3 v1, Vec3 v2)
	{
		return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
	}
	public static Vec3 operator -(Vec3 v1, Vec3 v2)
	{
		return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
	}
	public static Vec3 operator *(BigInteger d, Vec3 v1)
	{
		return new Vec3(d * v1.x, d * v1.y, d * v1.z);
	}
}


class Hailstone
{
	public long X { get; set; }
	public long Y { get; set; }
	public long Z { get; set; }
	public long VX { get; set; }
	public long VY { get; set; }
	public long VZ { get; set; }

	public Hailstone(long x, long y, long z, long vx, long vy, long vz)
	{
		X = x;
		Y = y;
		Z = z;
		VX = vx;
		VY = vy;
		VZ = vz;
	}
}
