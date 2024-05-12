while (true)
{
    var unknownVariable = GetUnknownVariable();
    Console.WriteLine(CheckObject(unknownVariable));
}

//Part 1: Pattern matching with types => Exists in Java as well, boring
static string CheckObject(object? obj) {
    if(obj is int i)
    {
         return CheckInteger(i);
    }
    else if (obj is string s)
    {
        return CheckString(s);
    }
    else if(obj is not null) {
        return $"Input \"{obj}\" has an unknown type";
    }
    else
    {
        return "No input given";
    }
}

//Part 2: Pattern matching on values, exists in Java as well => boring 
static string CheckString(string input) => input switch
{
    "TEST" => $"Input is defined value \"TEST\"",
    "DONE" => $"Input is defined value \"DONE\"",
    _ => $"Input is string \"{input}\"",
};


//Part 2: Pattern matching on conditionals, exists in Java as well => boring 
static string CheckInteger(int integer) => integer switch
{
    (> 100) => "Input is a huge integer",
    (< 0) => "Input is a negative integer",
    _ => $"Input is integer {integer}"
};

//Part 2: Pattern matching on records, exists in Java as well => boring
static string CheckRecord(Record record) => record switch
{
    { Int: > 100 } => "Input is a huge integer",
    { Int: < 0 } => "Input is a negative integer",
    { Str: "TEST" } => "Input is defined value \"TEST\"",
    { Str: "DONE" } => "Input is defined value \"DONE\"",
    _ => $"Input is Record {record.Int} {record.Str}",
};

// Other stuff


static object? GetUnknownVariable()
{
    Console.Write("Please Enter input:");
    var input = Console.ReadLine();
    if (input == "")
    {
        return null;
    }
    if (int.TryParse(input, out int integer))
    {
        return integer;
    }
    return input;
}

record class Record(int Int, string Str);
