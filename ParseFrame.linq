<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
</Query>

void Main()
{
	const char firstLevelSeparator = '|';
	const char secondLevelSeparator = '!';

	// enter line here
	string line = @"O|1|AUTOSID9||!!!WBC\!!!WBC-NE#\!!!CBC\!!!PLT|R|20161230035833|20161230000000||||C||||Whole blood";
	
	Console.WriteLine(line + Environment.NewLine);
    int position = 0;
	foreach (var element in line.Split(firstLevelSeparator))
	{
		Console.WriteLine($"{++position}==>{element}");
		if (element.Contains(secondLevelSeparator))
        {
			int subPosition = 0;
			foreach (var subElement in element.Split(secondLevelSeparator))
			{
				Console.WriteLine($"\t\t{position}.{++subPosition}==>{subElement}");
			}
		}
	}
}