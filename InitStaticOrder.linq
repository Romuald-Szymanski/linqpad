<Query Kind="Program" />

static class B
{
    public static int X = 7;
    static B() {
        Console.WriteLine("B.X = " + X);
        X = A.X;
        Console.WriteLine("B.X = " + X);
    }
}

static class A
{
    public static int X = B.X + 1;
    static A() {
        Console.WriteLine("A.X = " + X);
    }
}

void Main()
{
	Console.WriteLine("A = {0}, B = {1}", A.X, B.X);	
}

/*
A.X used, so static A() called.
A.X needs to be initialized, but it uses B.X, so static B() called.
B.X needs to be initialized, and it is initialized to 7. B.X = 7
All static fields of B are initialized, so static B() is called. X is printed ("7"), then it is set to A.X. 
A has already started being initialized, so we get the value of A.X, which is the default value 
	("when a class is initialized, all static fields in that class are first initialized to their default value"); B.X = 0, and is printed ("0").
Done initializing B, and the value of A.X is set to B.X+1. A.X = 1.
All static fields of A are initialized, so static A() is called. A.X is printed ("1").
Back in Main, the values of A.X and B.X are printed ("1", "0").
*/
