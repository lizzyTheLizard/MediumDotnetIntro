namespace Various;

public class NullableExample
{
    // Java is missing this feature but it exists in Kotlin
    public static void Run()
    {
        Console.WriteLine("Nullable Example");
        Console.WriteLine("----------------------");
        Console.WriteLine();

        int? nullable = null;
        int nonNullable = nullable ?? 0;
        var obj = new ExampleObject() { X = 1, Y = nonNullable };
        obj.X = nonNullable;
        obj = new ExampleObject() { X = 2 };

        //Those are not allowed
        //nonNullable = nullable;
        //obj = new ExampleObject(){X= nullable, Y = 2};
        //obj = new ExampleObject(){X= 2, Y = nullable};
        //obj = new ExampleObject(){Y= 2};
        //obj.X = nullable;

        // There are also conditional operators
        obj.X = nullable ?? 0;
        string? s = nullable?.ToString();
        string strings2 = nullable?.ToString() ?? "null";
    }
}

file class ExampleObject
{
    public required int X { get; set; }
    public int Y { get; set; }
}
