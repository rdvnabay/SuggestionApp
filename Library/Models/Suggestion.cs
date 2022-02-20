namespace Library.Models;

public class Suggestion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string OwnerNotes { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public bool ApprovedForRelease { get; set; } = false;
    public bool Archived { get; set; } = false;
    public bool Rejected { get; set; } = false;
    HashSet<string> UserVotes { get; set; } = new();

    public BasicUser Author { get; set; }
    public Category Category { get; set; }
    public Status Status { get; set; }
}
