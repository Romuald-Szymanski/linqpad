<Query Kind="Program" />

void Main()
{
	connections = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

	Subscribe("SID01", "Luc");
	Subscribe("SID02", "Jean");
	Subscribe("SID01", "Kevin");
	Subscribe("SID01", "George");
	Unsubscribe("SID02", "Jean");
	Unsubscribe("SID01", "Kevin");
	Subscribe("SID02", "Alain");
	Subscribe("SID02", "Dylan");
}

public static IDictionary<string, List<string>> connections;

static void Subscribe(string sampleId, string username)
{
	if (connections.Keys.Contains(sampleId))
	{
		if (!connections[sampleId].Contains(username, StringComparer.InvariantCultureIgnoreCase))
		{
			connections[sampleId].Add(username);
		}
	}
	else
	{
		connections.Add(sampleId, new List<string>(new[] { username }));
	}
	
	Notify(sampleId);
}

static void Unsubscribe(string sampleId, string username)
{
	if (connections.Keys.Contains(sampleId))
	{
		connections[sampleId].Remove(username);
	}
}

static void Notify(string sampleId)
{
	if (connections.Keys.Contains(sampleId) && connections[sampleId].Count > 1)
	{
		string message = sampleId + " => " + string.Join(",", connections[sampleId]);
		message.Dump();
	}
}
