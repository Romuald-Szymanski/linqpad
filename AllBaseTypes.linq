<Query Kind="Program" />

void Main()
{
	var current = new FinalClass();
	GetAllBaseTypeNames(current.GetType()).Dump();
}

static IEnumerable<string> GetAllBaseTypeNames(Type type)
{
	return GetAllBaseTypes(type)
		.Select (current => current.Name)
		.Distinct();
}

static IEnumerable<Type> GetAllBaseTypes(Type type)
{
	yield return type;
	
	foreach(var @interface in type.GetInterfaces())
	{
		yield return @interface;
	}
	
	if(type.BaseType != null)
	{
		foreach (var baseType in GetAllBaseTypes(type.BaseType))
		{
			yield return baseType;
		}
	}
}

public interface IFirst
{
}

public interface ISecond
{
}

public class MyBaseClass : IFirst
{
}

public class Intermediate : MyBaseClass, ISecond
{
}

public sealed class FinalClass : Intermediate
{
}