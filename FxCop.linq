<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Text.RegularExpressions.dll</Reference>
</Query>

/// findstr /s /i /c:"SuppressMessage" d:\dev\QC\*.cs > d:\suppressMessage-qc.txt
/// it includes 92 GlobalSuppression.cs files
/// cf. https://msdn.microsoft.com/en-us/library/ee1hzekz.aspx

static Dictionary<Rule, int> rules = new Dictionary<Rule, int>(new RuleComparer());

void Main()
{
	Rule[] rulesFoundManually = {
		// d:\dev\PatientResults\src\NI\Business\Reporting\Report\XtraReports\PatientOrder.Designer.cs
		new Rule("CA1303:DoNotPassLiteralsAsLocalizedParameters"),
		// d:\dev\PatientResults\src\NI\Business\Reporting\ReportAdapter\SummaryDataAdapter.cs
		new Rule("CA1031:DoNotCatchGeneralExceptionTypes"),
		new Rule("CA1031:DoNotCatchGeneralExceptionTypes"),
		// d:\dev\PatientResults\src\NI\Business\UI\StandardControlsLibrary\NIImageCollection.cs
		new Rule("CA1711:IdentifiersShouldNotHaveIncorrectSuffix"),
		// d:\dev\PatientResults\src\R3G\Business\UI\BusinessControlLibrary\RefactoredSettings\PopulationSettings\RangeEditors\RepositoryItem\PopulationEditRepositoryItem.cs
		new Rule("CA1810:InitializeReferenceTypeStaticFieldsInline")
	};

	foreach (var rule in rulesFoundManually)
	{
		AddToRules(rule);
	}
	
	string[] files = {
		@"d:\suppressMessage-PR.txt",
		@"d:\suppressMessage-qc.txt",
		@"d:\suppressMessage-Bordeaux.txt",
		@"d:\suppressMessage-BC.txt",
		@"d:\suppressMessage-R3GCommon.txt"
	};

	foreach (var file in files)
	{
		ProcessFile(file, rules);
	}

	rules.Sum(rule => rule.Value).Dump("Number of suppress messages");

	rules
		.Where(rule => GetManagedRecommandedRulesCode().Contains(rule.Key.Id))
		.Select(rule => new { Description = $"{rule.Key.Id} - {rule.Key.Description}", 
							   Count = rule.Value })
		.OrderByDescending(rule => rule.Count).ThenBy(rule => rule.Description)
//		.OrderBy(rule => rule.Description)
		.Dump("Suppress message list");
}

void ProcessFile(string filename, Dictionary<Rule, int> rules)
{
	string line;
	var file = new System.IO.StreamReader(filename);
	while ((line = file.ReadLine()) != null)
	{
		var rule = ProcessLine(line);
		if (rule != null)
		{
			AddToRules(rule);
		}
	}
}

static string pattern = @"SuppressMessage\w*\((?<content>[^\)|$]+)";
static Regex regex = new Regex(pattern, RegexOptions.Singleline);

Rule ProcessLine(string line)
{
	Rule rule = null;
	var match = regex.Match(line).Groups["content"].Value;
	var content = match.Replace("\"", string.Empty).Split(',');

	if (content.Length > 1)
	{
		var ruleInfo = content[1].Split(':');
		string id = ruleInfo[0];
		string desc = ruleInfo.Length > 1 ? ruleInfo[1] : string.Empty;
		rule = new Rule(id, desc);

		rule = CheckError(rule, line) ? null: rule;
	}
	
	return rule;
}

void AddToRules(Rule rule)
{
	if (rules.Keys.Contains(rule))
	{
		rules[rule]++;
	}
	else
	{
		rules.Add(rule, 1);
	}
}

const bool CHECK_DESCRIPTION_NULL = false;

bool CheckError(Rule rule, string line)
{
	bool error = false;
	
	if (string.IsNullOrEmpty(rule.Id)) 
	{
		line.Dump("id is null");
		error = true;
	}

	if (CHECK_DESCRIPTION_NULL && string.IsNullOrEmpty(rule.Description))
	{
		line.Dump("description is null");
	}
	
	return error;
}

class Rule
{
	public Rule(string manualEntrance)
	{
		var split = manualEntrance.Split(':');
		if (split.Length == 2)
		{
			Id = split[0].Trim();
			Description = split[1].Trim();
		}
	}
	
	public Rule(string id, string description)
	{
		Id = id.Trim();
		Description = description.Trim();
	}

	public string Id { get; set; }
	public string Description { get; set; }
}

class RuleComparer : IEqualityComparer<Rule>
{
	public bool Equals(Rule x, Rule y)
	{
		return x.Id.Equals(y.Id, StringComparison.InvariantCultureIgnoreCase);
	}

	public int GetHashCode(Rule rule)
	{
		return rule.Id.GetHashCode();
	}
}

IEnumerable<string> GetManagedRecommandedRulesCode()
{
	yield return "CA1001";
	yield return "CA1009";
	yield return "CA1016";
	yield return "CA1033";
	yield return "CA1049";
	yield return "CA1060";
	yield return "CA1061";
	yield return "CA1063";
	yield return "CA1065";
	yield return "CA1301";
	yield return "CA1400";
	yield return "CA1401";
	yield return "CA1403";
	yield return "CA1404";
	yield return "CA1405";
	yield return "CA1410";
	yield return "CA1415";
	yield return "CA1821";
	yield return "CA1900";
	yield return "CA1901";
	yield return "CA2002";
	yield return "CA2100";
	yield return "CA2101";
	yield return "CA2108";
	yield return "CA2111";
	yield return "CA2112";
	yield return "CA2114";
	yield return "CA2116";
	yield return "CA2117";
	yield return "CA2122";
	yield return "CA2123";
	yield return "CA2124";
	yield return "CA2126";
	yield return "CA2131";
	yield return "CA2132";
	yield return "CA2133";
	yield return "CA2134";
	yield return "CA2137";
	yield return "CA2138";
	yield return "CA2140";
	yield return "CA2141";
	yield return "CA2146";
	yield return "CA2147";
	yield return "CA2149";
	yield return "CA2200";
	yield return "CA2202";
	yield return "CA2207";
	yield return "CA2212";
	yield return "CA2213";
	yield return "CA2214";
	yield return "CA2216";
	yield return "CA2220";
	yield return "CA2229";
	yield return "CA2231";
	yield return "CA2232";
	yield return "CA2235";
	yield return "CA2236";
	yield return "CA2237";
	yield return "CA2238";
	yield return "CA2240";
	yield return "CA2241";
	yield return "CA2242";
}