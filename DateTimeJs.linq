<Query Kind="Program" />

void Main()
{
	var origin = new DateTime(1970, 1, 1);

	DateTime date = new DateTime(2015, 3, 15);
	date.Subtract(origin.ToLocalTime()).TotalMilliseconds.Dump();
	
	double d = 1426374000000;
	origin.AddMilliseconds(d).ToLocalTime().Dump();	
}
