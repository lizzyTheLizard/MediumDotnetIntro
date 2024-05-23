
namespace Iterator
{
    public class CustomAsyncCollection : IAsyncEnumerable<int>
    {
        public async IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(100, cancellationToken);
                yield return i;
            }
        }
    }
}