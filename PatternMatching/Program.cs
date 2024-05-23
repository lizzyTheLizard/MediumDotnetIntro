
//Part 1: Pattern matching with types => Exists in Java as well, boring
static string CheckObject(object? obj)
{
    if (obj is int i)
    {
        return CheckInteger(i);
    }
    else if (obj is Record record)
    {
        return CheckRecord(record);
    }
    else if (obj is string s)
    {
        return CheckString(s);
    }
    else if (obj is string[] array)
    {
        return CheckArray(array);
    }
    else if (obj is not null)
    {
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


//Part 3: Pattern matching on conditionals, exists in Java as well => boring 
static string CheckInteger(int integer) => integer switch
{
    (> 100) => "Input is a huge integer",
    (< 0) => "Input is a negative integer",
    _ => $"Input is integer {integer}"
};

//Part 4: Pattern matching on records, exists in Java as well => boring
static string CheckRecord(Record record) => record switch
{
    { Int: > 100 } => $"Input is a Record with a huge integer and {record.Str}",
    { Int: < 0 } => "Input is a Recor with a negative integer",
    { Str: "TEST" } => "Input is a Record with the defined value \"TEST\"",
    { Str: "DONE" } => "Input is a Record with the defined value \"DONE\"",
    _ => $"Input is Record {record.Int} {record.Str}",
};

// Part 5: Pattern matching on arrays, does not exist in Java
static string CheckArray(string[] array) => array switch
{
    ["TEST", var type] => $"Input is an array with the defined value \"TEST\" and {type}",
    ["DONE", var type] => $"Input is an array with the defined value \"DONE\" and {type}",
    _ => "Input is an array"
};


// Other stuff

while (true)
{
    var unknownVariable = GetUnknownVariable();
    Console.WriteLine(CheckObject(unknownVariable));
}

static object? GetUnknownVariable()
{
    Console.Write("Please Enter input:");
    var input = Console.ReadLine();
    if (input == "" || input == null)
    {
        return null;
    }
    if (input.Contains(' '))
    {
        var split = input.Split(' ');
        if (int.TryParse(split[0], out int integer2))
        {
            return new Record(integer2, split[1]);
        }
        return split;
    }
    return int.TryParse(input, out int integer) ? integer : input;
}

record class Record(int Int, string Str);