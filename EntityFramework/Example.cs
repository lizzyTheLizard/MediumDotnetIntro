namespace EntityFramework;

public class Example
{
    public required Guid Id { get; set; }

    public required DateOnly Date { get; set; }

    public required int Value { get; set; }

    public string? Note { get; set; }

    //Can be null if not loaded
    public ICollection<SubExample>? SubExamples { get; set; }

    public long Timestamp { get; set; }

    public override bool Equals(Object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        var other = (Example)obj;
        if (!Id.Equals(other.Id) || !Date.Equals(other.Date) || !Value.Equals(other.Value) || !(Note?.Equals(other.Note) ?? true))
        {
            return false;
        }
        //If not loaded, we can't compare
        if (SubExamples == null || other.SubExamples == null)
        {
            return true;
        }
        return SubExamples.SequenceEqual(other.SubExamples);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}