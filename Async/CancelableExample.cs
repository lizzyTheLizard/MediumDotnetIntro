namespace Async;

public class CancelableExample
{
    private readonly CancellationTokenSource _cts = new();
    private readonly HttpClient _client = new() { MaxResponseContentBufferSize = 1_000_000 };
    private readonly IEnumerable<string> _urlList = [
            "https://learn.microsoft.com",
        "https://learn.microsoft.com/aspnet/core",
        "https://learn.microsoft.com/azure",
        "https://learn.microsoft.com/azure/devops",
        "https://learn.microsoft.com/dotnet",
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
    }

    private void UserInterface()
    {
        Console.WriteLine("Press the ENTER key to cancel...\n");
        while (Console.ReadKey().Key != ConsoleKey.Enter) continue;
        Console.WriteLine("\nENTER key pressed: cancelling downloads.\n");
        _cts.Cancel();
    }

    private async Task DownloadAsync()
    {
        foreach (string url in _urlList)
        {
            var response = await _client.GetAsync(url, _cts.Token);
            byte[] content = await response.Content.ReadAsByteArrayAsync(_cts.Token);
            Console.WriteLine($"{url,-60} {content.Length,10:#,#}");
        }
    }
}
