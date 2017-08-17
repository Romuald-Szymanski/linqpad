<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

void Main()
{
	var assembly = Assembly.GetExecutingAssembly();
	var typeEnum = assembly.GetTypes().FirstOrDefault(type => type.Name.ToLower() == "genre");
	typeEnum.IsEnum.Dump();
//	var typeEnum = Type.GetType(enumeration);

	Enum.GetNames(typeEnum).ToDictionary(item => item, 
		item => new {
			Sequence = (int)Enum.Parse(typeEnum, item),
			Traduction = GetDescription(typeEnum, item)
		}
	).Dump();
}

string GetDescription(Type typeEnum, string valeur)
{
	string traduction = valeur;
	if(typeEnum.IsEnum)
	{
		var field = typeEnum.GetField(valeur);
		var attributes = (TraductionAttribute[])field.GetCustomAttributes(typeof(TraductionAttribute), false);
		traduction = attributes != null && attributes.Any() ? attributes.First().Traduction : string.Format("{0}.{1}", typeEnum.Name, valeur);
		// Rechercher traduction en BdD
	}
	return traduction;
}

enum Test
{
	Valeur1,
	[Traduction("Valeur #2")]
	Valeur2,
	[Traduction("Valeur #3")]
	Valeur3
}

enum Genre
{
	[Traduction("Homme")]
	Male, 
	[Traduction("Femme")]
	Female,
	[Traduction("Transgenre")]
	Other
}

class TraductionAttribute : Attribute
{
	public TraductionAttribute(string traduction)
	{
		Traduction = traduction;
	}
	
	public string Traduction {get; set;}
}
