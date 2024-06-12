namespace Various;

public class DelegateExample
{
    public static void Run()
    {
        Console.WriteLine("Delegate Example");
        Console.WriteLine("----------------------");
        Console.WriteLine();
        Transformer t = x => x + 1;
        Transformer t2 = int.Abs;
        t(3);
    }
}

// Delegates seems to be better than Javas functional interfaces
file delegate int Transformer(int x);
