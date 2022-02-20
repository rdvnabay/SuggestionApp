namespace Library.Models;

public class BasicSuggestion
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }

    public BasicSuggestion()
    {

    }

    public BasicSuggestion(Suggestion suggestion)
    {
        Id = suggestion.Id;
        Title = suggestion.Title;
    }
}
