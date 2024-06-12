namespace EntityFramework;

//An example for a 1:N-Relation. You could map the relation at the parent as well but this makes stuff more complex
public class SubExample
{
    public required Guid Id { get; set; }
    public required string Note { get; set; }
    public required int Value { get; set; }
}
