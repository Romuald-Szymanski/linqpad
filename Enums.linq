<Query Kind="Program" />

void Main()
{
	int intLevel = 1;
	AlarmLevel alarmLevel = (AlarmLevel)intLevel ;
	string alarmLevelValue = alarmLevel.ToString();
	
	alarmLevel.Dump("int to enum");
	alarmLevelValue.Dump("cast enum to string");
	
	string stringLevel = "danger";
	Enum.TryParse<AlarmLevel>(stringLevel, true, out alarmLevel);
	
	alarmLevel.Dump("string to enum");
	int value = (int)alarmLevel;
	value.Dump("cast enum to int");
}

public enum AlarmLevel{
	None = 0,
	Warning = 1,
	Danger = 2
}
