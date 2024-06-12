namespace Various;

public class ObjectsExample
{
    public static void Run()
    {
        Console.WriteLine("Objects Example");
        Console.WriteLine("----------------------");
        Console.WriteLine();

        var dec = new Deconstructable() { X = 1, Y = 2 };
        // You can decompose the object into its parts
        var (_, y) = dec;
        // You can even have multiple deconstructors, depending on the number of parameters
        var (x, _, z) = dec;

        var d = new Initializable() { X = 1, Y = 2 };
        //You can even use this without a class (anonymous types)
        var o = new { X = 1, Y = 2 };

        var c = new Constructable(1, 2);

        var r1 = new Record(1, 2);
        var r2 = new Record(1, 2);
        var r3 = new Record(1, 3);
        Console.WriteLine(r1 == r2); // This is true
        Console.WriteLine(r1.I);
        Console.WriteLine(r1 == r3); // This is false
        //constructor parameters are readonly, rest is mutable
        r1.Additional = "Hello";
        //Deconstructable by default
        var (i, l) = r1;

        var s1 = new Struct() { X = 1, Y = 2, Z = 3 };

        var i1 = new InterfaceImplementation();
        i1.Method1();
        //This is not possible, because Method2 is not part of the class, but the interface
        //i1.Method2();
        (i1 as IInterface).Method2();
        var i2 = new ExplicitInterfaceImplementation();
        //This is not possible, because Method1 is explicitly implemented
        //i2.Method1();
        (i2 as IInterface).Method1();
        var i3 = new OverwriteDefault();
        i3.Method2();
    }
}

// Java does not have deconstructors, only for records there is a "default" deconstructor
file class Deconstructable
{
    public required int X { get; set; }
    public required int Y { get; set; }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = X;
        y = Y;
        z = 1;
    }

}

// Required Properties must be set by the caller after the constructor. This does not exist in Java
file class Initializable
{
    //These properties are required to be set just after the object is created
    public required int X { get; init; }
    public required int Y { get; init; }
}

// You can also have a constructor, then you do not need the required or init keyword. This is the same as in Java
file class Constructable(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}

// Unlike Java-Records, C#-Records can have additional mutable properties. Properties in constructor are threaded similar to Java
file record Record(int I, int L)
{
    public string? Additional { get; set; }
}

// C#-Structs are similar to Classes, but they are value types. This does not exist in Java
file struct Struct
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}

file interface IInterface
{
    public void Method1();

    //With default implementation!
    public void Method2()
    {
        Console.WriteLine("Method2");
    }
}

// Interface implementations are similar to Java
file class InterfaceImplementation : IInterface
{
    public void Method1()
    {
        Console.WriteLine("Method1");
    }
}

file class ExplicitInterfaceImplementation : IInterface
{
    void IInterface.Method1()
    {
        Console.WriteLine("Method1");
    }
}

file class OverwriteDefault : IInterface
{
    void IInterface.Method1()
    {
        Console.WriteLine("Method1");
    }

    public void Method2()
    {
        Console.WriteLine("Method2overwrite");
    }
}
