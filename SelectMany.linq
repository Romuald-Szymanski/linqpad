<Query Kind="Program" />

void Main()
{
	IEnumerable<Test> tests = new[] {
		new Test(1) { Inners = new[] { new Inner(1, 1), new Inner(1, 2), new Inner(1, 3) }},
		new Test(2) { Inners = new[] { new Inner(2, 4), new Inner(2, 5)}},
		new Test(3) { Inners = new[] { new Inner(3, 6), new Inner(3, 7), new Inner(3, 8), new Inner(3, 9) }},
		new Test(4) { Inners = new[] { new Inner(4, 10)}}
	};
	
	tests.SelectMany(test => test.Inners)
		.Select(inner => new { inner.TestId, inner.Id })
		.Dump();
}

public class Test
{
	public Test(int id)
	{
		Id= id;
	}

	public int Id {get; set;}
	public IEnumerable<Inner> Inners {get; set;}
}

public class Inner
{
	public Inner(int testId, int id)
	{
		Id = id;
		TestId = testId;
	}

	public int Id {get; set;}
	public int TestId {get; set;}
}
