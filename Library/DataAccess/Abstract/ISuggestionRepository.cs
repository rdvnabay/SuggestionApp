
namespace Library.DataAccess.Abstract;

public interface ISuggestionRepository
{
    Task CreateSuggestion(Suggestion suggestion);
    Task<List<Suggestion>> GetAllApprovedSuggestions();
    Task<List<Suggestion>> GetAllSuggestions();
    Task<List<Suggestion>> GetAllSuggestionsWaitingForApproval();
    Task<Suggestion> GetSuggestion(string id);
    Task UpdateSuggestion(Suggestion suggestion);
    Task UpvoteSuggestion(string suggestionId, string userId);
}