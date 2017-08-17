<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
</Query>

void Main()
{
	string age = AgeHelper.GetAge(new DateTime(2016, 12, 15), DateTime.Today);
	age.Dump();
}

public static class AgeHelper
{
	#region constants
	/// <summary>
	/// How many hours system considers for a day
	/// </summary>
	public const int HoursPerDay = 24;
	/// <summary>
	/// How many days system considers for a week
	/// </summary>
	public const int DaysPerWeek = 7;
	/// <summary>
	/// how many days the system considers for a month
	/// </summary>
	public const double DaysPerMonth = 30.435;
	/// <summary>
	/// how many months the system considers for a year
	/// </summary>
	public const int MonthsPerYear = 12;
	/// <summary>
	/// how many weeks the system considers for a year
	/// </summary>
	public const int WeeksPerYear = 52;
	/// <summary>
	/// how many weeks the system considers for a month
	/// </summary>
	public const int WeeksPerMonth = 4;

	/// <summary>
	/// default nr of hours displayed
	/// </summary>
	public const int DefaultHours = 48;
	/// <summary>
	/// default nr of days displayed
	/// </summary>
	public const int DefaultDays = 15;
	/// <summary>
	/// default nr of weeks displayed
	/// </summary>
	public const int DefaultWeeks = 8;
	/// <summary>
	/// default nr of months displayed
	/// </summary>
	public const int DefaultMonths = 24;
	/// <summary>
	/// default nr of years displayed
	/// </summary>
	public const int DefaultYears = 1;

	private static readonly int[] DaysInMonth = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
	#endregion

	public static int MaxHour { get { return DefaultHours; } }
	public static int MaxDay { get { return DefaultDays * HoursPerDay; } }
	public static int MaxWeek { get { return DefaultWeeks * DaysPerWeek * HoursPerDay; } }
	public static int MaxMonth { get { return Convert.ToInt32(DefaultMonths * DaysPerMonth * HoursPerDay); } }
	public static int HoursPerMonth { get { return Convert.ToInt32(DaysPerMonth * HoursPerDay); } }

	internal static string GetAge(DateTime? birthdate, DateTime referenceDate)
	{
		if (!birthdate.HasValue)
		{
			return null;
		}

		TimeSpan ts = referenceDate - birthdate.Value;

		var ageInHours = Convert.ToInt32(ts.TotalHours);

		// From 0 day to 60 days, the system shall display the age in days
		if (ageInHours <= 60 * HoursPerDay)
		{
			int days = ageInHours / HoursPerDay;
			return string.Format("Days: {0}", days);
		}
		//  From 61 days to 23 months, the system shall display the age in months
		else if (ageInHours <= 23 * HoursPerMonth)
		{
			DateTime fromDate = new DateTime(Math.Max(0, referenceDate.Ticks - (ageInHours) * TimeSpan.TicksPerHour));
			DateDifference dateDiff = ComputeDateDifference(fromDate, referenceDate);

			int months = dateDiff.Years * MonthsPerYear + dateDiff.Months;
			return string.Format("Months: {0}", months);
		}
		// From 24 months, the system shall display the age in years
		else
		{
			DateTime fromDate = new DateTime(Math.Max(0, referenceDate.Ticks - (ageInHours) * TimeSpan.TicksPerHour));
			DateDifference dateDiff = ComputeDateDifference(fromDate, referenceDate);

			int years = dateDiff.Years;

			return string.Format("Years: {0}", years);
		}
	}

	private static DateDifference ComputeDateDifference(DateTime fromDate, DateTime toDate)
	{
		DateDifference dateDiff = new DateDifference();
		int increment;

		/// 
		/// Day Calculation
		/// 
		increment = 0;
		if (fromDate.Day > toDate.Day)
		{
			increment = DaysInMonth[fromDate.Month - 1];
		}
		/// if it is february month
		/// if it's to day is less then from day
		if (increment == -1)
		{
			if (DateTime.IsLeapYear(fromDate.Year))
			{
				// leap year february contain 29 days
				increment = 29;
			}
			else
			{
				increment = 28;
			}
		}
		if (increment != 0)
		{
			dateDiff.Days = (toDate.Day + increment) - fromDate.Day;
			increment = 1;
		}
		else
		{
			dateDiff.Days = toDate.Day - fromDate.Day;
		}

		///
		///month calculation
		///
		if ((fromDate.Month + increment) > toDate.Month)
		{
			dateDiff.Months = (toDate.Month + MonthsPerYear) - (fromDate.Month + increment);
			increment = 1;
		}
		else
		{
			dateDiff.Months = (toDate.Month) - (fromDate.Month + increment);
			increment = 0;
		}

		///
		/// year calculation
		///
		dateDiff.Years = toDate.Year - (fromDate.Year + increment);

		return dateDiff;
	}
}

/// <summary>
/// holds difference between 2 dates in time years, months and seconds
/// </summary>
internal class DateDifference
{
	private int _Years;
	/// <summary>
	/// years
	/// </summary>
	public int Years
	{
		get
		{
			return _Years;
		}
		set
		{
			_Years = value;
		}
	}

	private int _Months;
	/// <summary>
	/// months
	/// </summary>
	public int Months
	{
		get
		{
			return _Months;
		}
		set
		{
			_Months = value;
		}
	}

	private int _Days;
	/// <summary>
	/// days
	/// </summary>
	public int Days
	{
		get
		{
			return _Days;
		}
		set
		{
			_Days = value;
		}
	}
}