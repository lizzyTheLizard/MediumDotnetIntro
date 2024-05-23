var enumerable = GetEnumerable();
//Can be used in foreach loops
foreach (var e in enumerable)
{
    Console.WriteLine($" Sync: {e}");
}
//Can be used in LINQ queries
var evenElements = from e in enumerable where e % 2 == 0 select e;
var evenElements2 = enumerable.Where(e => e % 2 == 0);
//Can be used in while loops
var enumerator = enumerable.GetEnumerator();
while (enumerator.MoveNext())
{
    Console.WriteLine($" Sync Enumerator: {enumerator.Current}");
}

var asyncEnumerable = GetAsyncEnumerable();
//Can be used in foreach loops
await foreach (var e in asyncEnumerable)
{
    Console.WriteLine($"Async: {e}");
}

//Cann´t be used in LINQ queries directly
var asyncEnumerator = asyncEnumerable.GetAsyncEnumerator();
//Can be used in while loops
while (await asyncEnumerator.MoveNextAsync())
{
    Console.WriteLine($"While {asyncEnumerator.Current}");
}
//Can be run in parallel
var task = Task.Run(async () => { await foreach (var e in asyncEnumerable) { Console.WriteLine($"Parallel {e}"); } });
Thread.Sleep(400);
Console.WriteLine("Waiting for task to finish");
await task;


static IEnumerable<int> GetEnumerable()
{
    for (int i = 0; i < 10; i++)
    {
        yield return i;
    }
}

static async IAsyncEnumerable<int> GetAsyncEnumerable()
{
    for (int i = 0; i < 10; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}

static IEnumerator<int> GetEnumerator()
{
    for (int i = 0; i < 10; i++)
    {
        yield return i;
    }
}

static async IAsyncEnumerator<int> GetAsyncEnumerator()
{
    for (int i = 0; i < 10; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}