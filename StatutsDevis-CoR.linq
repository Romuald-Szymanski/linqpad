<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

void Main()
{
	var chaineValidation = new DraftHandler()
		.SetSuccessor(new AValiderHandler()
		.SetSuccessor(new ValideHandler()
		.SetSuccessor(new EnvoyeClientHandler()
		.SetSuccessor(new ProdHandler()
		.SetSuccessor(new AnnuleHandler()
		)))));
	
	var info = new StatutDevisInfo
	{
		MontantDevis = 40000m,
		StatutCourant = StatutDevis.Valide,
		Roles = new[] { "ModifierDevis", "ValiderDevis" },
		Fonctions = new [] { "COO" },
		EstResponsableDevis = false,
		EstSuperieurResponsable = true
	};
	
	var statuts = chaineValidation.Valide(info).ToArray();
	statuts.Dump("Statuts possibles");
}

// Define other methods and classes here

public enum StatutDevis
{
	Draft = 0,
	AValider,
	Valide,
	EnvoyeClient,
	Signe,
	AMettreEnProd,
	Prod,
	Refuse,
	Annule,
	Clos
}

public class StatutDevisInfo
{
	public StatutDevisInfo()
	{
		// provient de la BdD
		Validation = new Dictionary<string, decimal>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ "DirCli", 5000 } ,
			{ "Consultant", 5000},
			{ "Commercial", 5000},
			{ "DirCom", 10000},
			{ "DirProjet", 50000},
			{ "DirEntite", 100000}, 
			{ "COO", 200000},
			{ "CEO", decimal.MaxValue}	// correspond à un champ NULL
		};
		
		Production = new Dictionary<string, decimal>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ "DirEntite", 15000}, 
			{ "COO", 40000},
			{ "CEO", decimal.MaxValue}	// correspond à un champ NULL
		};
	}

	public StatutDevis StatutCourant { get; set; }
	public decimal MontantDevis { get; set; }
	public string[] Roles { get; set; }
	public string[] Fonctions {get; set;}
	public bool EstResponsableDevis { get; set; }
	public bool EstSuperieurResponsable { get; set; }
	
	Dictionary<string, decimal> Validation;
	Dictionary<string, decimal> Production;
	
	public bool PeutValider()
	{
		var max = Fonctions.Select (f => Validation.ContainsKey(f) ? Validation[f] : 0).Max();
		return max >= MontantDevis;
	}
	
	public bool PeutMettreEnProd()
	{
		var max = Fonctions.Select (f => Production.ContainsKey(f) ? Validation[f] : 0).Max();
		return max >= MontantDevis;
	}
	
	public bool EstResponsableOuSuperieur()
	{
		return EstResponsableDevis || EstSuperieurResponsable;
	}
}

public abstract class StatutDevisHandler
{
	protected StatutDevisHandler successor;
	
	public abstract IEnumerable<StatutDevis> Valide(StatutDevisInfo info);
	
	public StatutDevisHandler SetSuccessor(StatutDevisHandler successor)
	{
		this.successor = successor;
		return this;
	}
}

public sealed class DraftHandler : StatutDevisHandler
{
	public override IEnumerable<StatutDevis> Valide(StatutDevisInfo info)
	{
		if(info.EstResponsableOuSuperieur())
		{
			if(info.StatutCourant == StatutDevis.AValider && info.Roles.Contains("ModifierDevis"))
			{
				yield return StatutDevis.Draft;
			}
		}
		
		if(successor != null)
		{
			var successorStatuts = successor.Valide(info);
			foreach(var statut in successorStatuts)
			{
				yield return statut;
			}
		}
	}
}

public sealed class EnvoyeClientHandler : StatutDevisHandler
{
	public override IEnumerable<StatutDevis> Valide(StatutDevisInfo info)
	{
		if(info.EstResponsableOuSuperieur())
		{
			if(info.StatutCourant == StatutDevis.Valide)
			{
				yield return StatutDevis.EnvoyeClient;
			}
		}
		
		if(successor != null)
		{
			var successorStatuts = successor.Valide(info);
			foreach(var statut in successorStatuts)
			{
				yield return statut;
			}
		}
	}
}

public sealed class AValiderHandler : StatutDevisHandler
{
	public override IEnumerable<StatutDevis> Valide(StatutDevisInfo info)
	{
		if(info.EstResponsableOuSuperieur())
		{
			if(info.StatutCourant == StatutDevis.Draft)
			{
				yield return StatutDevis.AValider;
			}
		}
		
		if(successor != null)
		{
			var successorStatuts = successor.Valide(info);
			foreach(var statut in successorStatuts)
			{
				yield return statut;
			}
		}
	}
}


public sealed class ValideHandler : StatutDevisHandler
{
	public override IEnumerable<StatutDevis> Valide(StatutDevisInfo info)
	{
		if(info.EstResponsableOuSuperieur() && info.Roles.Contains("ValiderDevis") && info.PeutValider())
		{
			if(info.StatutCourant == StatutDevis.Draft)
			{
				yield return StatutDevis.Valide;
			}
		}
		
		if(successor != null)
		{
			var successorStatuts = successor.Valide(info);
			foreach(var statut in successorStatuts)
			{
				yield return statut;
			}
		}
	}
}

public sealed class ProdHandler : StatutDevisHandler
{
	public override IEnumerable<StatutDevis> Valide(StatutDevisInfo info)
	{
		var statutsCourantsAutorises = new StatutDevis[]
		{
			StatutDevis.Valide,
			StatutDevis.EnvoyeClient,
			StatutDevis.Signe
		};
	
		if(statutsCourantsAutorises.Contains(info.StatutCourant) && info.PeutMettreEnProd())
		{
			yield return StatutDevis.Prod;
		}
		
		if(successor != null)
		{
			var successorStatuts = successor.Valide(info);
			foreach(var statut in successorStatuts)
			{
				yield return statut;
			}
		}
	}
}

public sealed class AnnuleHandler : StatutDevisHandler
{
	public override IEnumerable<StatutDevis> Valide(StatutDevisInfo info)
	{
		var statutsNonCourantsAutorises = new StatutDevis[]
		{
			StatutDevis.EnvoyeClient,
			StatutDevis.Clos
		};
	
		if(info.EstResponsableOuSuperieur() &&  !statutsNonCourantsAutorises.Contains(info.StatutCourant))
		{
			yield return StatutDevis.Annule;
		}
		
		if(successor != null)
		{
			var successorStatuts = successor.Valide(info);
			foreach(var statut in successorStatuts)
			{
				yield return statut;
			}
		}
	}
}