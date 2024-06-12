
namespace Async;

public class EventsExample
{
    private readonly ManualResetEventSlim _mres = new(false);
    private readonly CancellationTokenSource _cts = new();
    private readonly HttpClient _client = new() { MaxResponseContentBufferSize = 1_000_000 };
    private readonly IEnumerable<string> _urlList = [
            "https://learn.microsoft.com",
        "https://learn.microsoft.com/aspnet/core",
        "https://learn.microsoft.com/azure",
        "https://learn.microsoft.com/azure/devops",
        "https://learn.microsoft.com/dotnet",
        "https://learn.microsoft.com/dynamics365",
    ];

    public async Task Run()
    {
        var uiTask = Task.Run(UserInterface, _cts.Token);
        var downloadTask = DownloadAsync();
        try
        {
            await downloadTask;
            Console.WriteLine("Download task completed before cancel request was processed.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Download task has been cancelled.");
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Download task has time out.");
        }
    }

    private void UserInterface()
    {
        Console.Write("Press 's' to start, 'p' to pause or any other key to cancel: ");
        while (true)
        {
            var key = Console.ReadKey().Key;
            Console.WriteLine();
            switch (key)
            {
                case ConsoleKey.S:
                    _mres.Set();
                    Console.WriteLine("START key pressed: resuming downloads.");
                    continue;
                case ConsoleKey.P:
                    Console.WriteLine("PAUSE key pressed: pausing downloads.");
                    _mres.Reset();
                    continue;
                default:
                    Console.WriteLine($"{key} pressed: cancelling downloads.");
                    _cts.Cancel();
                    break;
            }
        }
    }

    private async Task DownloadAsync()
    {
        foreach (string url in _urlList)
        {
            //This will wait blocking this thread. If you want non-blocking, wrap this in a TaskCompletionSource
            if (!_mres.Wait(5000, _cts.Token))
            {
                throw new TimeoutException();
            }
            var response = await _client.GetAsync(url, _cts.Token);
            byte[] content = await response.Content.ReadAsByteArrayAsync(_cts.Token);
            Console.WriteLine($"{url,-60} {content.Length,10:#,#}");
        }
    }
}
