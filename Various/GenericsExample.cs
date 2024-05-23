using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Various
{
    // Java has generics, but they are not as powerful as C#s as Java is limited by type erasure
    public class GenericsExample
    {
        public static void Run()
        {
            Console.WriteLine("Generics Example");
            Console.WriteLine("----------------------");
            Console.WriteLine();

            var g = new Generic<int>(3);
            var isInt = g.IsOfType(3);
            List<string> strings = ["test"];
            Console.WriteLine(g);

            //This is easily possble
            IEnumerable<object> objects = strings;
            //This is dangerous as it can cause a CastException. In Java however this is not even detectable...
            IEnumerable<string> strings2 = (IEnumerable<string>)objects;
            try
            {
                IEnumerable<int> ints = (IEnumerable<int>)objects;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Expected cast exception was throw");
            }
        }
    }

    file class Generic<T>(T value)
    {
        public T Value { get; private init; } = value;

        //This would not be possible in Java
        public bool IsOfType(Object value) => value is T;

        //This would not be possible in Java
        public override string ToString() => $"Generic of Type {typeof(T).Name} with value {Value}";
    }
}