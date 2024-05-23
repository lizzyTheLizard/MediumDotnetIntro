using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Various
{
    public class ExtensionsExample
    {
        public static void Run()
        {
            Console.WriteLine("Extensions Example");
            Console.WriteLine("----------------------");
            Console.WriteLine();

            var str = "Hello World";
            Console.WriteLine(str.CustomMethod());
        }
    }

    file static class MyExtensions
    {
        private static readonly char[] separator = [' ', '.', '?'];

        public static int CustomMethod(this string str) => str.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}