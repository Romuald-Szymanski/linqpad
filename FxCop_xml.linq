<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.XML.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xml.Linq.dll</Reference>
</Query>

XElement document;
Dictionary<string, Element> results = new Dictionary<string, UserQuery.Element>(StringComparer.InvariantCultureIgnoreCase);

void Main()
{
	// PR done on #1.2.0+2546.c7d9fbd
	// Bordeaux done on #1.2.0+292.723e84b
	var files = new[] {
		@"C:\Users\rszymanski\Downloads\fxcop-pr.xml",
		@"C:\Users\rszymanski\Downloads\fxcop-bx.xml"
	};

	foreach (string filename in files)
	{
		ProcessFile(filename);
    }

	// Get total
	int total = results.Values.Sum(item => item.Count);
	total.Dump("Total");

	// Group by category
	results.GroupBy(
			item => item.Value.Category,
			item => new { Description = item.Value.Description, Level = item.Value.Level, Count = item.Value.Count, Percent = ComputePercent(item.Value.Count, total) })
		.Select(item => new
		{
			Category = item.Key,
			Rules = item.OrderByDescending(rule => rule.Count)
		})
		.Dump("Results");

	// full rule list
	
}

class Element
{
	public Element(string category, string checkId, string typeName, string level, int count)
	{
		Category = category;
		CheckId = checkId;
		TypeName = typeName;
		Count = count;
		Level = level;
	}

	public string Category { get; set; }
	public string CheckId { get; set; }
	public string TypeName { get; set; }
	public int Count { get; set; }
	public string Level { get; set; }

	public string Description
	{
		get
		{
			return $"{CheckId} - {TypeName}";
		}
	}
}

void ProcessFile(string filename)
{
	document = XElement.Load(filename);

	string xPath = "//Messages/Message";
	foreach (var element in Extract(xPath))
	{
		AddToResults(element);
	}
}

IEnumerable<Element> Extract(string xPath)
{
	foreach (var element in document.XPathSelectElements(xPath))
	{
		string category = element.Attribute("Category").Value;
		string checkId = element.Attribute("CheckId").Value;
		string typeName = element.Attribute("TypeName").Value;
		string level = element.Elements("Issue").FirstOrDefault()?.Attribute("Level").Value;
		int count = element.Elements("Issue").Count();
		yield return new Element(category, checkId, typeName, level, count);
	}
}

void AddToResults(Element element)
{
	if (!results.Keys.Contains(element.CheckId))
	{
		results.Add(element.CheckId, element);
	}
	else
	{
		results[element.CheckId].Count += element.Count;
	}
}

decimal ComputePercent(int value, int total, int precision = 4)
{
	return total == 0 ? 0 : Math.Round((decimal)value / total, precision);
}