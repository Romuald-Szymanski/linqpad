<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

void Main()
{
	DateTime? dateDebut = new DateTime(2014, 02, 01);

	int month = 0;
	var saisies = new[] { 
		new Saisie((new DateTime(2014, 02, 01)).AddMonths(month), "date #1"),
		new Saisie((new DateTime(2014, 02, 10)).AddMonths(month), "date #2"),
		new Saisie((new DateTime(2014, 02, 17)).AddMonths(month), "date #3"),
		new Saisie((new DateTime(2014, 02, 21)).AddMonths(month), "date #4"),
		new Saisie((new DateTime(2014, 02, 25)).AddMonths(month), "date #5")
		};
		
		
	DateTime debut = dateDebut ?? saisies.Min(item => item.Date);
	var resultat = Enumerable.Range(0, Math.Max(0, DateTime.Today.Subtract(debut).Days + 1))
		.Select(item => debut.AddDays(item))
		.GroupJoin(saisies, date => date, saisie => saisie.Date,
			(date, saisieDate) => saisieDate.FirstOrDefault() ?? new Saisie(date, string.Empty))
		.Union(saisies.Where(item => item.Date > DateTime.Today))
		.Dump();
}

public class Saisie
{
	public Saisie(DateTime date, string commentaires)
	{
		Date = date;
		Commentaires = commentaires;
	}
	public DateTime Date {get; set;}
	public string Commentaires {get; set;}
}