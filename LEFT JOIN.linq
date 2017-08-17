<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
</Query>

void Main()
{
	Language[] languages = new Language[]
		{
		new Language {Id = 1, Name = "English"},
		new Language {Id = 2, Name = "Russian"}
		};

	Person[] persons = new Person[]
	{
		new Person { LanguageId = 1, FirstName = "Tom" },
		new Person { LanguageId = 1, FirstName = "Sandy" },
		new Person { LanguageId = 2, FirstName = "Vladimir" },
		new Person { LanguageId = 2, FirstName = "Mikhail" },
	};

	var result = languages.GroupJoin(persons, lang => lang.Id, pers => pers.LanguageId,
		(lang, ps) => new { Key = lang.Name, Persons = ps });

//	result.Dump();

	string[] modules = new[] {
	"Analytical Module",
	"Bulk Feeder",
	"Compressor",
	"Database/Software",
	"Pick and Place",
	"Reagent Pipettors",
	"Reagent Storage",
	"Sample Pipettor",
	"Sample Presentation Unit",
	"Supplies/Other",
	"Temperature"
	};

	SystemStatus[] dataset = new[]{
		new SystemStatus { Module_Name = "Bulk Feeder", Module_Status = "ERROR" },
		new SystemStatus { Module_Name = "Pick and Place", Module_Status = "WARNING" },
		new SystemStatus { Module_Name = "Sample Pipettor", Module_Status = "WARNING" }
	};


	var result2 = modules.GroupJoin(
		dataset,
		module => module,
		record => record.Module_Name,
		(module, record) => new { Key = module, Statuses = record })
	.Select(item => item.Statuses.FirstOrDefault() == null ? new SystemStatus { Module_Name = item.Key, Module_Status = "PASS" } : new SystemStatus { Module_Name = item.Key, Module_Status = item.Statuses.First().Module_Status } );
					
	result2.Dump();
	
}

public class SystemStatus
{
	public string Module_Status { get; set; }
	public string Module_Name { get; set; }
}

class Language
{
	public int Id { get; set; }
	public string Name { get; set; }
}

class Person
{
	public int LanguageId { get; set; }
	public string FirstName { get; set; }
}