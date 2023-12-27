
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

string[] lines = File.ReadAllLines("Input.txt");
var connections = new ConcurrentDictionary<string, List<string>>();
foreach (var line in lines)
{
	var l = line.Split();
	var from = l[0][..^1];

	foreach (var to in l.Skip(1))
	{
		connections.GetOrAdd(from, _ => new()).Add(to);
		connections.GetOrAdd(to, to => new()).Add(from);
	}
}

var result = 0;
var keys = connections.Keys.ToList();
foreach (var key in keys.Skip(1))
{
	var componentSize = GetComponentSize(connections, keys[0], key);
	if (componentSize > 0)
	{
		result = componentSize * (keys.Count - componentSize);
		break;
	}
}

Console.WriteLine(result);


int GetComponentSize(ConcurrentDictionary<string, List<string>> connections, string start, string end)
{
	var flows = new Dictionary<(string from, string to), int>();
	var numFlows = 0;
	while (true)
	{
		var componentSize = GetComponentSize2(connections, flows, start, end);
		if (componentSize == 0)
		{
			numFlows++;
		}
		else if (numFlows == 3)
		{
			return componentSize;
		}
		else
			break;
	}

	return 0;
}

int GetComponentSize2(ConcurrentDictionary<string, List<string>> connections, Dictionary<(string from, string to), int> flows, string start, string end)
{
	var from = new Dictionary<string, string>()
	{
		[start] = start,
	};

	var steps = 0;

	var Q = new Queue<string>();
	Q.Enqueue(start);

	while (Q.Count > 0 &&
		!from.ContainsKey(end))
	{
		var itemQ = Q.Dequeue();
		steps++;

		var current = connections[itemQ];
		foreach (var dest in connections.Keys)
		{
			var rate = (current.Contains(dest) ? 1 : 0) - flows.GetValueOrDefault((itemQ, dest));
			if (rate > 0 &&
				!from.ContainsKey(dest))
			{
				from[dest] = itemQ;
				Q.Enqueue(dest);
			}
		}
	}

	if (!from.ContainsKey(end))
	{
		return steps;
	}

	var cur = end;
	while (cur != start)
	{
		CollectionsMarshal.GetValueRefOrAddDefault(flows, (from[cur], cur), out _) += 1;
		CollectionsMarshal.GetValueRefOrAddDefault(flows, (cur, from[cur]), out _) -= 1;

		cur = from[cur];
	}

	return 0;
}
