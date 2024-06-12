namespace Async;

public class AsyncExample
{
    public async Task Run()
    {
        // You can simply await tasks, this makes the function Async
        await ReadFile("Program.cs");

        // You can also use ContinueWith to avoid direct await
        var fileReadTask = ReadFile("Program.cs").ContinueWith(s => Console.WriteLine("Read File Completed"));

        //You can use tasks do so some work in parallel
        var parallelTask = Task.Run(HeavyWork);

        //Exception Handling With Tasks
        var fileProcess = ReadFile("Program.cs");
        try
        {
            var fileContent = await fileProcess;
            Console.WriteLine(fileContent[..10]);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }

        await Task.WhenAll(fileReadTask, parallelTask);
    }

    private static async Task<string> ReadFile(string filename)
    {
        using StreamReader reader = new(filename);
        return await reader.ReadToEndAsync();
    }

    private static void HeavyWork()
    {
        for (var i = 0; i < 5; i++)
        {
            Console.WriteLine($"Child Thread {i}");
            Thread.Sleep(500);
        }
    }
}
