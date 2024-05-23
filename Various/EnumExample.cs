namespace Various
{
    public static class EnumExample
    {
        public static void Run()
        {
            Console.WriteLine("Enum Example");
            Console.WriteLine("----------------------");
            Console.WriteLine();

            var exampleEnum = ExampleEnum.A;
            Console.WriteLine(exampleEnum.GetDescription());
        }
    }

    // An enum is a simple name for an integer value. Unklike Java enums, C# enums are not objects.
    file enum ExampleEnum { A, B, C }

    //You cannot have methods in an enum, but you can define extension methods for it
    file static class NewEnumExtensions
    {
        public static string GetDescription(this ExampleEnum grade) => grade switch
        {
            ExampleEnum.A => "Excellent",
            ExampleEnum.B => "Good",
            ExampleEnum.C => "Average",
            _ => throw new Exception("Invalid grade"),
        };

        public static int NumericalValue(this ExampleEnum grade) => grade switch
        {
            ExampleEnum.A => 1,
            ExampleEnum.B => 2,
            ExampleEnum.C => 3,
            _ => throw new Exception("Invalid grade"),
        };
    }
}