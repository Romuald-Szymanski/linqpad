<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
</Query>

void Main()
{
	AgeHelper.ComputeAge(DateTime.Today.AddHours(-2), DateTime.Today).Dump();
	AgeHelper.ComputeAge(new DateTime(2017, 1, 14), DateTime.Today).Dump();
	AgeHelper.ComputeAge(new DateTime(2016, 12, 14), DateTime.Today).Dump();
	AgeHelper.ComputeAge(new DateTime(2016, 2, 27), DateTime.Today).Dump();
	AgeHelper.ComputeAge(DateTime.Today.AddYears(-2).AddDays(2), DateTime.Today).Dump();
	AgeHelper.ComputeAge(DateTime.Today.AddYears(-2), DateTime.Today).Dump();
	AgeHelper.ComputeAge(DateTime.Today.AddYears(-5), DateTime.Today).Dump();
	AgeHelper.ComputeAge(new DateTime(1983, 5, 29), DateTime.Today).Dump();
	
	Console.WriteLine("*****************");

	AgeHelper.ComputeAge(new DateTime(2016, 12, 14), new DateTime(2017, 2, 13)).Dump();
	AgeHelper.ComputeAge(new DateTime(2015, 2, 14), new DateTime(2017, 2, 13)).Dump();


	AgeHelper.ComputeAge(new DateTime(2017, 02, 17), DateTime.Today);


}

public static class AgeHelper
{
	public const int HoursPerDay = 24;
	public const double DaysPerMonth = 30.435;
	public const int MonthsPerYear = 12;
	
	public static string ComputeAge(DateTime birthdate, DateTime reference)
	{
		string value = string.Empty;
		
		int diffDays = reference.Subtract(birthdate).Days;
		if (diffDays <= 60)
		{
			value = $"{diffDays} days";
		}
		
		double monthLimit = 2 * MonthsPerYear * DaysPerMonth;

		if (diffDays > 60 && diffDays < monthLimit)
		{
			int diffMonths = Math.Max(2, (int)Math.Floor(diffDays / DaysPerMonth));
			value = $"{diffMonths} months";
		}

		if (diffDays >= monthLimit)
		{
			int diffYears = Math.Max(2, (int)(Math.Floor(diffDays/(MonthsPerYear * DaysPerMonth))));
			value = $"{diffYears} years";
		}
		
		return value;
	}
}