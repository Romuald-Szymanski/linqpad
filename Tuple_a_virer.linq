<Query Kind="Program" />

void Main()
{
	Notify(new[] { "A", "B" })
		.GroupBy(item => item.Item2)
		.Select(item => new Tuple<string, int>(item.Key, item.Count()))
		.Dump();	
}

static IEnumerable<Tuple<string, string>> Notify(IEnumerable<string> samples)
{
	foreach (var sample in samples)
	{
		foreach (var wl in GetWl(sample))
		{
			yield return new Tuple<string, string>(sample, wl);
		}
	}
}

static IEnumerable<string> GetWl(string sample)
{
	if (sample == "A")
	{
		yield return "W1";
		yield return "W2";
	}

	if (sample == "B")
	{
		yield return "W2";
		yield return "W3";
	}
}

