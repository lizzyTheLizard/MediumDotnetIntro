# MediumDotnetIntro
# Java Developer Discovers C#: The Surprising Features That Will Make You Want to Switch!

## 

I started off my coding journey as a .NET engineer, dipping my toes into C# and the .NET framework. Then, life happened, and I ended up spending the last ten years of my career in the Java world. I got pretty fond at this techstack. However, I recently switched teams and found myself back in the .NET neighborhood. I won't lie, it was a bit repulsed at first; PascalCasing for method names and newlines for curly brackets... Who want to progam like that? 

But as I started exploring C# again, I stumbled upon some seriously cool features that not only made coding fun but also super powerful. So, stick around as I spill the beans on my journey back to C#, the good, bad and weird features I discovered and why, even after being a Java professional, I don't want to go back!

## Similarities

## The Good
TODO
Cool things C# has to offer
* LINQ: Similar to stream, but provider can also create direct responses (e.g. Entity-Frameworks)
* async - await
* Gererics: No type erasure



Smaller Improvements:
* Mutliple Return Values: Java does not support this, you have to define classes for it
* Extension-Methods
* Nullable Types
* Delegates instead of functinal interfaces
* Object initializer and anonymous types
* Operators and Indexers
* Better iterators (yield)
* Method by default non virtual
* Named Arguments and default Parameters    




## The Bad
I honestly didn't find a lot that C# was lacking. However, if I had to pick one thing that I missed from Java, it would be the power of Java enums. In Java an enum can behave as a regular class. You can have methods, fields, and constructors in them. In C# however, enums are basically just a list of named constants. You can mimic the behavior of Java enums by using extension methods, but it's not the same.

```csharp
var grade = ExampleEnum.A;
Console.WriteLine(grade.GetDescription());

enum ExampleEnum { A, B, C }

// You can mimic the behavior of Java enums by using extension methods
static class NewEnumExtensions
{
    public static string GetDescription(this GradeEnum grade) => grade switch
    {
        GradeEnum.A => "Excellent",
        GradeEnum.B => "Good",
        GradeEnum.C => "Average",
        _ => throw new Exception("Invalid grade"),
    };

    public static int NumericalValue(this GradeEnum grade) => grade switch
    {
        GradeEnum.A => 1,
        GradeEnum.B => 2,
        GradeEnum.C => 3,
        _ => throw new Exception("Invalid grade"),
    };        
}
```

Additionally, there are few things that I found in C# that I didn't like:
* You can overwrite non-virtual methods using the __'new'-Keyword__. Then, depending on the typ of the variable eihter the parent or the overwritten method is called. This can lead to unexpected behavior and and do not really see the use case for it...
* Java forces you to handle checked exceptions, which can be a pain, but it also makes your code more robust. In C#, there are __only unchecked exception__. So it is easy to miss an exception that should be handled.
* In C#, you can pass __parameters by reference__ using the `in`, `ref` or `out` keyword. This is rarely usefull (maybe some high performance apps?), esp. since C# allowes multiple return values. The usage can be confusing and I would recommend to avoid it. Howerver, the standard library uses it in some places, so you have to deal with it.
* __Overwriting the == Operator__: In C#, you can override the `==` operator. In nearly all programing languages including Java and C#, '==' means reference comparization and .Equals() means value comparization. In C#, this default behavior can be overwritten which can be confusing and I cannot think of a single use case where this would be a good idea. 

Aditionally, there are some things I do not think should be used in Web-Applications, but might make a some sense in other contexts:
* Beside Classes, C# also allows to define __structs__. These objects are allocated on the stack and are passed as value objets. Java does not have such a feature and I never missed it. However, there might be cases where this allowes you to improve performace and they are needed when accessing C++-Librarires. In all other contexts, I would recommend to stick with classes. C# futhermore allowes you to use __unchecked__ code and __fixed__ pointers for the same reasons.
* C# allowes you to use __Partial Classes__, e.g. a class definied in multiple files. I do not know what this can be used for...
* C# supports the traditional __goto__, however you should stick with the more modern control structures like for, while, etc.





