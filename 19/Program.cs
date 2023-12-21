string[] lines = File.ReadAllLines("Input.txt");
var workflows = new List<string>();
var ratings = new List<Rating>();
bool isRating = false;
foreach (var line in lines)
{
	if (line == string.Empty)
	{
		isRating = true;
		continue;
	}
	if (isRating)
	{
		var lineSplit = line.Split(',');
		int x = Convert.ToInt32(lineSplit[0].Substring(3));
		int m = Convert.ToInt32(lineSplit[1].Substring(2));
		int a = Convert.ToInt32(lineSplit[2].Substring(2));
		int s = Convert.ToInt32(lineSplit[3].Substring(2, lineSplit[3].Length - 3));
		ratings.Add(new Rating(x, m, a, s));
	}
	else
	{
		workflows.Add(line);
	}
}

foreach (var rating in ratings)
{
	rating.IsAccepted = CalculateRating(rating);
}

var result = ratings.Where(r => r.IsAccepted == true).Sum(r => r.X + r.M + r.A + r.S);
Console.WriteLine(result);


// Part 2
long result2 = CalculateRating2();
Console.WriteLine(result2);


bool CalculateRating(Rating rating)
{
	var isAccepted = false;

	var workflowQ = new Queue<string>();
	var startWorkflow = workflows.First(w => w.StartsWith("in{"));
	workflowQ.Enqueue(startWorkflow.Substring(3, startWorkflow.Length - 4));

	while (workflowQ.Count > 0)
	{
		var workflow = workflowQ.Dequeue();
		if (workflow.Contains(':'))
		{
			var condition = workflow.Substring(0, workflow.IndexOf(':'));
			var options = workflow.Substring(workflow.IndexOf(':') + 1);

			var conditionOperator = "<";
			if (condition.Contains(">"))
			{
				conditionOperator = ">";
			}
			var item = condition.Substring(0, condition.IndexOf(conditionOperator));
			var value = Convert.ToInt32(condition.Substring(workflow.IndexOf(conditionOperator) + 1));
			bool isCondition = false;
			switch (item)
			{
				case "x":
					if (conditionOperator == "<")
					{
						isCondition = rating.X < value;
					}
					else
					{
						isCondition = rating.X > value;
					}
					break;
				case "m":
					if (conditionOperator == "<")
					{
						isCondition = rating.M < value;
					}
					else
					{
						isCondition = rating.M > value;
					}
					break;
				case "a":
					if (conditionOperator == "<")
					{
						isCondition = rating.A < value;
					}
					else
					{
						isCondition = rating.A > value;
					}
					break;
				case "s":
					if (conditionOperator == "<")
					{
						isCondition = rating.S < value;
					}
					else
					{
						isCondition = rating.S > value;
					}
					break;
			}

			var optionTrue = options.Substring(0, options.IndexOf(","));
			var optionFalse = options.Substring(options.IndexOf(",") + 1);
			var optionToEvaluate = optionFalse;
			if (isCondition)
			{
				optionToEvaluate = optionTrue;
			}

			if (optionToEvaluate == "A")
			{
				isAccepted = true;
				break;
			}
			else if (optionToEvaluate == "R")
			{
				isAccepted = false;
				break;
			}
			else
			{
				if (optionToEvaluate.Contains(":"))
				{
					workflowQ.Enqueue(optionToEvaluate);
				}
				else
				{
					var nextWorkflow = workflows.First(w => w.StartsWith(optionToEvaluate + "{"));
					workflowQ.Enqueue(nextWorkflow.Substring(optionToEvaluate.Length + 1, nextWorkflow.Length - (optionToEvaluate.Length + 2)));
				}
			}

		}
	}

	return isAccepted;
}

long CalculateRating2()
{
	long result = 0;
	var startDictionary = new Dictionary<string, (int, int)>
	{
		{ "m", (1, 4000) },
		{ "s", (1, 4000) },
		{ "a", (1, 4000) },
		{ "x", (1, 4000) }
	};
	var workflowQ = new Queue<(string, Dictionary<string, (int, int)>)>();
	var startWorkflow = workflows.First(w => w.StartsWith("in{"));
	workflowQ.Enqueue((startWorkflow.Substring(3, startWorkflow.Length - 4), startDictionary));

	while (workflowQ.Count > 0)
	{
		var workflowDequeue = workflowQ.Dequeue();
		var workflow = workflowDequeue.Item1;
		var inputQ = workflowDequeue.Item2;
		if (workflow.Contains(':'))
		{
			var condition = workflow.Substring(0, workflow.IndexOf(':'));
			var options = workflow.Substring(workflow.IndexOf(':') + 1);

			var conditionOperator = "<";
			if (condition.Contains(">"))
			{
				conditionOperator = ">";
			}
			var item = condition.Substring(0, condition.IndexOf(conditionOperator));
			var value = Convert.ToInt32(condition.Substring(workflow.IndexOf(conditionOperator) + 1));

			var newInput = new Dictionary<string, (int, int)>(inputQ);
			if (conditionOperator == "<")
			{
				newInput[item] = (inputQ[item].Item1, Math.Min(value - 1, inputQ[item].Item2));
				inputQ[item] = (Math.Max(value, inputQ[item].Item1), inputQ[item].Item2);
			}
			else
			{
				newInput[item] = (Math.Max(value + 1, inputQ[item].Item1), inputQ[item].Item2);
				inputQ[item] = (inputQ[item].Item1, Math.Min(value, inputQ[item].Item2));
			}

			var optionTrue = options.Substring(0, options.IndexOf(","));
			var optionFalse = options.Substring(options.IndexOf(",") + 1);

			if (optionTrue == "A")
			{
				long resultA = 1;
				foreach (var key in newInput)
				{
					int minValue = key.Value.Item1;
					int maxValue = key.Value.Item2;
					resultA *= Math.Max(0, maxValue - minValue + 1);
				}
				result += resultA;
			}
			else if (optionTrue != "R")
			{
				if (optionTrue.Contains(":"))
				{
					workflowQ.Enqueue((optionTrue, newInput));
				}
				else
				{
					var nextWorkflow = workflows.First(w => w.StartsWith(optionTrue + "{"));
					workflowQ.Enqueue((nextWorkflow.Substring(optionTrue.Length + 1, nextWorkflow.Length - (optionTrue.Length + 2)), newInput));
				}
			}

			if (optionFalse == "A")
			{
				long resultA = 1;
				foreach (var key in inputQ)
				{
					int minValue = key.Value.Item1;
					int maxValue = key.Value.Item2;
					resultA *= Math.Max(0, maxValue - minValue + 1);
				}
				result += resultA;
			}
			else if (optionFalse != "R")
			{
				if (optionFalse.Contains(":"))
				{
					workflowQ.Enqueue((optionFalse, inputQ));
				}
				else
				{
					var nextWorkflow = workflows.First(w => w.StartsWith(optionFalse + "{"));
					workflowQ.Enqueue((nextWorkflow.Substring(optionFalse.Length + 1, nextWorkflow.Length - (optionFalse.Length + 2)), inputQ));
				}
			}
		}
	}

	return result;
}

class Rating
{
	public int X { get; set; }
	public int M { get; set; }
	public int A { get; set; }
	public int S { get; set; }
	public bool IsAccepted { get; set; }

	public Rating(int x, int m, int a, int s)
	{
		X = x;
		M = m;
		A = a;
		S = s;
	}
}
