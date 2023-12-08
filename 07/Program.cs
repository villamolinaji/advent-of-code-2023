string[] lines = File.ReadAllLines("Input.txt");

var hands = new List<Hand>();
foreach (var line in lines)
{
	var lineSplit = line.Split(' ');
	hands.Add(new Hand(lineSplit[0], Convert.ToInt32(lineSplit[1])));
}

foreach (var hand in hands)
{
	hand.HandValue = GetHandValue(hand.Cards);
	var cardOrders = new List<int>();
	foreach (var card in hand.Cards)
	{
		cardOrders.Add(GetCardValue(card.ToString()));
	}
	hand.CardOrders = cardOrders;
}

var handsOrder = hands
	.OrderBy(h => h.HandValue)
	.ThenBy(h => h.CardOrders[0])
	.ThenBy(h => h.CardOrders[1])
	.ThenBy(h => h.CardOrders[2])
	.ThenBy(h => h.CardOrders[3])
	.ThenBy(h => h.CardOrders[4])
	.ToList();

int order = 1;
foreach (var hand in handsOrder)
{
	hand.Rank = order;
	order++;
}

var result = handsOrder.Sum(h => h.Rank * h.Bid);
Console.WriteLine(result);


//Part2
hands = new List<Hand>();
foreach (var line in lines)
{
	var lineSplit = line.Split(' ');
	hands.Add(new Hand(lineSplit[0], Convert.ToInt32(lineSplit[1])));
}

foreach (var hand in hands)
{
	hand.HandValue = GetHandValue(hand.Cards, true);
	var cardOrders = new List<int>();
	foreach (var card in hand.Cards)
	{
		cardOrders.Add(GetCardValue(card.ToString(), true));
	}
	hand.CardOrders = cardOrders;
}

handsOrder = hands
	.OrderBy(h => h.HandValue)
	.ThenBy(h => h.CardOrders[0])
	.ThenBy(h => h.CardOrders[1])
	.ThenBy(h => h.CardOrders[2])
	.ThenBy(h => h.CardOrders[3])
	.ThenBy(h => h.CardOrders[4])
	.ToList();

order = 1;
foreach (var hand in handsOrder)
{
	hand.Rank = order;
	order++;
}

result = handsOrder.Sum(h => h.Rank * h.Bid);
Console.WriteLine(result);



int GetCardValue(string card, bool isPart2 = false)
{
	switch (card)
	{
		case "2":
			return 2;
		case "3":
			return 3;
		case "4":
			return 4;
		case "5":
			return 5;
		case "6":
			return 6;
		case "7":
			return 7;
		case "8":
			return 8;
		case "9":
			return 9;
		case "T":
			return 10;
		case "J":
			if (isPart2)
			{
				return 1;
			}
			return 11;
		case "Q":
			return 12;
		case "K":
			return 13;
		case "A":
			return 14;
		default:
			return 0;
	}
}

int GetHandValue(string cards, bool isPart2 = false)
{
	var cardsCount = cards.GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count());
	var joker = 'J';

	if (isPart2 && cardsCount.ContainsKey(joker))
	{
		char target = cardsCount.Keys.First();
		foreach (var card in cardsCount.Keys)
		{
			if (card != joker)
			{
				if (cardsCount[card] > cardsCount[target] || target == joker)
				{
					target = card;
				}
			}
		}

		if (target != joker && cardsCount.ContainsKey(joker))
		{
			cardsCount[target] += cardsCount[joker];
			cardsCount.Remove(joker);
		}

		if (cardsCount.ContainsKey(joker) && cardsCount.Count > 1)
		{
			cardsCount.Remove(joker);
		}
	}

	if (cardsCount.Max(c => c.Value) == 1)
	{
		return 1;
	}
	if (cardsCount.Max(c => c.Value) == 2)
	{
		if (cardsCount.Where(c => c.Value == 2).Count() == 1)
		{
			return 2;
		}
		return 3;
	}
	if (cardsCount.Max(c => c.Value) == 3)
	{
		if (cardsCount.Where(c => c.Value == 2).Count() == 0)
		{
			return 4;
		}
		return 5;
	}
	if (cardsCount.Max(c => c.Value) == 4)
	{
		return 6;
	}
	if (cardsCount.Max(c => c.Value) == 5)
	{
		return 7;
	}
	return 0;
}

class Hand
{
	public string Cards { get; set; }
	public int Bid { get; set; }
	public int Rank { get; set; }
	public int HandValue { get; set; }
	public List<int> CardOrders { get; set; }

	public Hand(string cards, int bid)
	{
		Cards = cards;
		Bid = bid;
		CardOrders = new List<int>();
	}
}