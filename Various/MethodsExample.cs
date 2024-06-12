namespace Various;

public class MethodsExample
{
    public static void Run()
    {
        Console.WriteLine("Methods Example");
        Console.WriteLine("----------------------");
        Console.WriteLine();

        Console.WriteLine($"SingleReturnValue: {SingleReturnValue()}");
        (int a, int b) = MultipleReturnValues();
        Console.WriteLine($"MutlipleReturnValue: First value: {a}, Second value: {b}");
        //You can also ignore some values
        (_, int c) = MultipleReturnValues();

        MutipleArguments("John", "Doe");
        //Arguments with a default value can be omitted
        MutipleArguments("John");
        //Arguments can be named
        MutipleArguments(surname: "Doe", name: "John");

        //Out-Parameters do not need to be assigned before and can be changed by the called function
        int x1, y1;
        OutParams(out x1, out y1);
        //Now they are assigned
        Console.WriteLine($"OutParams: {x1}, {y1}");

        //Ref-Parameters must be assigned before and can be changed by the called function
        RefParams(ref x1, ref y1);
        Console.WriteLine($"OutParams: {x1}, {y1}");

        //Readonly-Ref-Parameters must be assigned before and cannot be changed by the called function
        ReadonlyRefParams(ref x1);

        //In-Parameters are similar to readonly ref parameters but the caller do not need a parameter value
        InParams(x1, y1);

        //Even == can be overridden
        var c1 = new OverwriteEqual(1);
        var c2 = new OverwriteEqual(1);
        Console.WriteLine($"c1 == c2: {c1 == c2}");
        Console.WriteLine($"c1 equals c2: {c1.Equals(c2)}");
        var d1 = new OverwriteEqualAndSame(1);
        var d2 = new OverwriteEqualAndSame(1);
        Console.WriteLine($"d1 == d2: {d1 == d2}");
        Console.WriteLine($"d1 equals d2: {d1.Equals(d2)}");
    }


    private static int SingleReturnValue() => 1;

    private static (int, int) MultipleReturnValues() => (1, 2);

    private static void MutipleArguments(string name, string surname = "") => Console.WriteLine($"Hello, {name} {surname}!");


    private static void OutParams(out int x, out int y)
    {
        //Parameters must be assigned and cannot be accessed before assignment
        x = 1;
        y = 2;
    }

    private static void RefParams(ref int x, ref int y)
    {
        //Parameters can (but do not need) be assigned and can be accessed before assignment
        Console.WriteLine($"RefParams: {x}, {y}");
        x = 3;
    }

    private static void ReadonlyRefParams(ref readonly int x)
    {
        //Like ref but cannot be changed
        Console.WriteLine($"RefParams: {x}");
    }

    private static void InParams(in int x, in int y)
    {
        //Like readonly ref
        Console.WriteLine($"RefParams: {x}, {y}");
    }
}

// You cannot define own operators in Java
file class ExampleWithOperator(int v)
{
    private readonly int _v = v;

    public static ExampleWithOperator operator +(ExampleWithOperator c1, ExampleWithOperator c2)
    {
        return new ExampleWithOperator(c1._v + c2._v);
    }
}

//You cannot define indexers in Java
file class ExampleWithIndexer(int v)
{
    private int _v = v;


    // Define the indexer to allow client code to use [] notation.
    public int this[int i]
    {
        get { return _v + i; }
        set { _v = value - i; }
    }
}

file class OverwriteEqual(int v)
{
    private readonly int _v = v;

    public override bool Equals(object? obj)
    {
        return _v == ((OverwriteEqual?)obj)?._v;
    }

    public override int GetHashCode() => _v;
}

// You can also override "==" => this can lead to weird behavior
file class OverwriteEqualAndSame(int v)
{
    private readonly int _v = v;

    public static bool operator ==(OverwriteEqualAndSame c1, OverwriteEqualAndSame c2) => c1._v == c2._v;
    public static bool operator !=(OverwriteEqualAndSame c1, OverwriteEqualAndSame c2) => c1._v != c2._v;

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj);
    }

    public override int GetHashCode() => _v;
}
