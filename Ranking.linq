<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
</Query>

void Main()
{
	var flags = new List<Flag>
	{
		new Flag(FlagType.Driver, "Driver2"),
		new Flag(FlagType.Error, "Error"),
		new Flag(FlagType.Instrument, "Instrument"),
		new Flag(FlagType.Suspect, "Suspect"),
		new Flag(FlagType.Predefined, "Predefined"),
		new Flag(FlagType.Driver, "Driver"),
		new Flag(FlagType.Predefined, "Predefined2")
	};

	var order = new List<FlagType> {
		FlagType.Suspect,
		FlagType.Predefined,
        FlagType.Error,
		FlagType.Instrument,
		FlagType.Driver
	};
	
	var r = flags.GroupJoin(
		Rank(order), 
		flag => flag.Type, 
		ft => ft.Item1, 
		(flag, ft) => new { Flag = flag, ft = ft.SingleOrDefault() }
	)
	.OrderBy(item => item.ft.Item2)
	.Select(item => item.Flag.Name)
	.Take(2);

	string.Join(" ", r).Dump();
}

IEnumerable<Tuple<FlagType, int>> Rank(List<FlagType> source)
{
	int rank = 0;
	foreach (FlagType item in source)
	{
		yield return new Tuple<FlagType, int>(item, rank++);
	}
}

// Define other methods and classes here
public enum FlagType
{
	Suspect = 0,
	Error,
	Predefined,
	Driver,
	Instrument
}

public class Flag
{
	public Flag(FlagType type, string name)
	{
		Type = type;
		Name = name;
	}
	
	public FlagType Type { get; set; }
	public string Name { get; set; } 
}
