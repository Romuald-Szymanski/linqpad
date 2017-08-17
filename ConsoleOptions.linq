<Query Kind="Program" />

void Main()
{
	string[] args = new[] { "option1:value1", "option2:value2", "option3:value3", "option4"};

	var options = args.Select(item => OptionValue.GetOptionValue(item))
	.ToDictionary (item => item.Option, item => item, StringComparer.InvariantCultureIgnoreCase);
	
	options["Option1"].Value.Dump();
	
}

public class OptionValue
{
	public string Option { get; set; }
	public string Value { get; set; }
	public bool HasNoValue { get; set; }

	private static char Separator = ':';

	public static OptionValue GetOptionValue(string argument)
	{
		OptionValue returnValue = null;
		string[] split = argument.Split(OptionValue.Separator);
		if (split != null && split.Any())
		{
			returnValue = new OptionValue
			{
				Option = split[0],
				Value = split.Count() > 1 ? split[1] : string.Empty,
				HasNoValue = split.Count() ==  1
			};
		}
		return returnValue;
	}
}

