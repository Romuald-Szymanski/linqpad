<Query Kind="Program" />

void Main()
{
	var orders = new[] 
	{
		new Order { Tubes = new[] { new Tube { SampleId = "Test1" } } },
		new Order { Tubes = new[] { new Tube { SampleId = "Test2" } } },
		new Order { Tubes = new[] { new Tube { SampleId = "Test3" } } },
		new Order { Tubes = new[] { new Tube { SampleId = "Test2" } } },
		new Order { Tubes = new[] { new Tube { SampleId = "Test4" } } },
		new Order { Tubes = new[] { new Tube { SampleId = "Test1" } } },
		new Order { Tubes = new[] { new Tube { SampleId = "Test1" } } },

	};

	orders.SelectMany(item => item.Tubes)
		.GroupBy(item => item.SampleId)
		.Select(item => new { SampleId = item.Key, Count = item.Count () })
		.Where(item => item.Count > 1)
		.Dump();
}

public class Tube
{
	public string SampleId { get; set; }
}

public class Order
{
	public IEnumerable<Tube> Tubes { get; set; }
}