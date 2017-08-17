<Query Kind="Program" />

void Main()
{
	Run run1 = new Run("Run1", "DXH1_Arras", new[] {
		new Analyte("MON", 6f),
		new Analyte("EOS", 1.4f),
		new Analyte("BAS", 6.2f),
		new Analyte("WBC", 3.2f),
		new Analyte("Hct", 5.8f)
	});

	Run run2 = new Run("Run2", "DXI1_Arras", new[] {
		new Analyte("MON", 8f),
		new Analyte("EOS", 7.4f),
		new Analyte("BAS", 3.2f),
		new Analyte("WBC", 5.2f),
//		new Analyte("Hct", 4.8f)
	});

	Run run3 = new Run("Run3", "DXH1_Arras", new[] {
		new Analyte("MON", 4f),
		new Analyte("EOS", 0.4f),
		new Analyte("BAS", 1.2f),
		new Analyte("WBC", 3.4f),
		new Analyte("Hct", 7.9f)
	});

	IEnumerable<Run> runs = new[] { run1, run2, run3 };

	var runList = runs.Select(run => new { name = run.Name, instrument = run.Instrument });
	runList.Dump();

	var allAnalytes = runs.SelectMany(run => run.Analytes.Select(item => new { run = run.Name, item.Code, item.Value, item.Status, item.DeltaCheck } ));
	allAnalytes.Dump();

	IEnumerable<string> analytes = runs.SelectMany(run => run.Analytes).Select(item => item.Code).Distinct();
	foreach (string currentAnalyte in analytes)
	{
		// runs LEFT JOIN analytes
		var values = runList.GroupJoin(allAnalytes.Where(item => item.Code == currentAnalyte), 
			r => r.name,
			a => a.run,
			(a, r) =>
			{
				var current = r.SingleOrDefault();
				return current != null ? current : new { run = a.name, Code = string.Empty, Value = 0f, Status = Status.None, DeltaCheck = DeltaCheck.None };
            });

		(new { analyte = currentAnalyte, values }).Dump();
	}
}

public class Run
{
	public Run()
	{
	}

	public Run(string name, string instrument, IEnumerable<Analyte> analytes)
	{
		Name = name;
		Instrument = instrument;
		Analytes = analytes;
	}

	public string Name { get; set; }
	public string Instrument { get; set; }
	public IEnumerable<Analyte> Analytes { get; set; }
}

public class Analyte
{
	public Analyte()
	{
	}
	
	public Analyte(string code, float value, Status status = Status.Uploaded, DeltaCheck deltaCheck = DeltaCheck.None)
	{
		Code = code;
		Value = value;
		Status = status;
		DeltaCheck = deltaCheck;
	}
	
	public string Code { get; set; }
	public float Value { get; set; }
	public Status Status { get; set; }
	public DeltaCheck DeltaCheck { get; set; }
}

public enum Status
{
	None = 0,
	Uploading,
	Uploaded
}

public enum DeltaCheck
{
	None = 0,
	Up,
	Down
}

// http://extensionmethod.net/csharp/ienumerable-t/pivot
public static class LinqExtenions
{
	public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, 
		Func<TSource, TFirstKey> firstKeySelector, 
		Func<TSource, TSecondKey> secondKeySelector, 
		Func<IEnumerable<TSource>, TValue> aggregate)
	{
		var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

		var l = source.ToLookup(firstKeySelector);
		foreach (var item in l)
		{
			var dict = new Dictionary<TSecondKey, TValue>();
			retVal.Add(item.Key, dict);
			var subdict = item.ToLookup(secondKeySelector);
			foreach (var subitem in subdict)
			{
				dict.Add(subitem.Key, aggregate(subitem));
			}
		}

		return retVal;
	}
}
