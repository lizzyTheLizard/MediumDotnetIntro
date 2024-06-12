namespace ASP;

public class ExampleDatabase
{
    private readonly List<Example> _examples = [];

    public async Task<ICollection<Example>> GetAll()
    {
        await Task.Delay(10);
        return _examples.AsReadOnly();
    }

    public async Task<ICollection<Example>> GetForDate(DateOnly date)
    {
        await Task.Delay(10);
        return _examples.Where(e => e.Date == date).ToList();
    }

    public async Task<Example?> GetForId(Guid id)
    {
        await Task.Delay(10);
        return _examples.FirstOrDefault(e => e.Id == id);
    }

    public async Task<Example> Create(Example input)
    {
        await Task.Delay(10);
        _examples.Add(input);
        return input;
    }

    public async Task<Example> Update(Example input)
    {
        await Task.Delay(10);
        _examples.RemoveAll(e => e.Id == input.Id);
        _examples.Add(input);
        return input;
    }

    public async Task Delete(Guid id)
    {
        await Task.Delay(10);
        _examples.RemoveAll(e => e.Id == id);
    }

    public async Task Clear()
    {
        await Task.Delay(10);
        _examples.Clear();
    }
}
