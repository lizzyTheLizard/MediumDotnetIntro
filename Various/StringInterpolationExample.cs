namespace Various;

public class StringInterpolationExample
{
    // String interpolation, Java has similar features nowadys. C#s are more powerful
    public static void Run()
    {
        Console.WriteLine("String Interpolation Example");
        Console.WriteLine("----------------------");
        Console.WriteLine();

        var name = "World";
        var s = $" Hello {name}";
        Console.WriteLine($"PI = {Math.PI,20:F3}");
    }
}
