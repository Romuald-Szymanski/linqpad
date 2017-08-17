<Query Kind="Program" />

void Main()
{
	var tuples1 = new Tuple<Control, string>[]
	{
		new Tuple<Control, string>(new Control(1, "Control#1"), "message #1"),
		new Tuple<Control, string>(new Control(1, "Control#1"), "message #2"),
		new Tuple<Control, string>(new Control(2, "Control#2"), "message #1"),
		new Tuple<Control, string>(new Control(2, "Control#2"), "message #2"),
		new Tuple<Control, string>(new Control(3, "Control#3"), "message #1"),
		new Tuple<Control, string>(new Control(3, "Control#3"), "message #2"),
		new Tuple<Control, string>(new Control(3, "Control#3"), "message #3")
	};
	
	var result1 = tuples1.GroupBy (item => item.Item1.Id)
		.Select(g => new { control = g.First().Item1, messages = string.Join(Environment.NewLine, g.Select (item => item.Item2)) })
		.Dump();
		
	var control1 = new Control(1, "Control#1");
	var control2 = new Control(2, "Control#2");
	var control3 = new Control(3, "Control#3");
	
	var tuples2 = new Tuple<Control, string>[]
	{
		new Tuple<Control, string>(control1, "message #1"),
		new Tuple<Control, string>(control1, "message #2"),
		new Tuple<Control, string>(control2, "message #1"),
		new Tuple<Control, string>(control2, "message #2"),
		new Tuple<Control, string>(control3, "message #1"),
		new Tuple<Control, string>(control3, "message #2"),
		new Tuple<Control, string>(control3, "message #3")
	};
	
	var result2 = tuples2.GroupBy (item => item.Item1)
		.Select(g => new { control = g.Key, messages = string.Join(Environment.NewLine, g.Select (item => item.Item2)) })
		.Dump();
}

class Control
{
	public Control()
	{
	}
	
	public Control(int id, string nom)
	{
		this.Id = id;
		this.Nom = nom;
	}

	public int Id {get; set;}
	public string Nom {get; set;}
}

