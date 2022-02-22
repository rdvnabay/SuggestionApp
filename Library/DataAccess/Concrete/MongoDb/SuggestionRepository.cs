using Microsoft.Extensions.Caching.Memory;

namespace Library.DataAccess.Concrete.MongoDb;

public class SuggestionRepository : ISuggestionRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<Suggestion> _suggestions;
    private const string _cacheKey = "SuggestionRepository";

    public SuggestionRepository(IDbConnection dbConnection, IUserRepository userRepository, IMemoryCache cache)
    {
        _dbConnection = dbConnection;
        _userRepository = userRepository;
        _cache = cache;
        _suggestions = _dbConnection.SuggestionCollection;
    }

    public async Task<List<Suggestion>> GetAllSuggestions()
    {
        var output = _cache.Get<List<Suggestion>>(_cacheKey);
        if (output is null)
        {
            var results = await _suggestions.FindAsync(s => s.Archived == false);
            output = results.ToList();

            _cache.Set(_cacheKey, output, TimeSpan.FromMinutes(1));
        }
        return output;
    }

    public async Task<List<Suggestion>> GetAllApprovedSuggestions()
    {
        var output = await GetAllSuggestions();
        return output.Where(x => x.ApprovedForRelease).ToList();
    }

    public async Task<Suggestion> GetSuggestion(string id)
    {
        var result = await _suggestions.FindAsync(s => s.Id == id);
        return result.FirstOrDefault();
    }

    public async Task<List<Suggestion>> GetAllSuggestionsWaitingForApproval()
    {
        var output = await GetAllSuggestions();
        return output.Where(x => x.ApprovedForRelease == false && x.Rejected == false).ToList();
    }

    public async Task UpdateSuggestion(Suggestion suggestion)
    {
        await _suggestions.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
        _cache.Remove(_cacheKey);
    }

    public async Task UpvoteSuggestion(string suggestionId, string userId)
    {
        var client = _dbConnection.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_dbConnection.SuggestionCollectionName);
            var suggestionInTransaction = db.GetCollection<Suggestion>(_dbConnection.SuggestionCollectionName);
            var suggestion = (await suggestionInTransaction.FindAsync(s => s.Id == suggestionId)).First();

            bool isUpvote = suggestion.UserVotes.Add(userId);
            if (!isUpvote)
                suggestion.UserVotes.Remove(userId);

            await suggestionInTransaction.ReplaceOneAsync(s => s.Id == suggestionId, suggestion);
            var usersInTransaction = db.GetCollection<User>(_dbConnection.UserCollectionName);
            var user = await _userRepository.GetUserAsync(suggestion.Author.Id);

            if (isUpvote)
                user.VotedOnSuggestions.Add(new BasicSuggestion(suggestion));
            else
            {
                var suggestionToRemove = user.VotedOnSuggestions.Where(s => s.Id == suggestionId).First();
                user.VotedOnSuggestions.Remove(suggestionToRemove);
            }
            await usersInTransaction.ReplaceOneAsync(u => u.Id == userId, user);
            await session.CommitTransactionAsync();
            _cache.Remove(_cacheKey);

        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task CreateSuggestion(Suggestion suggestion)
    {
        var client = _dbConnection.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_dbConnection.SuggestionCollectionName);
            var suggestionInTransaction = db.GetCollection<Suggestion>(_dbConnection.SuggestionCollectionName);
            await suggestionInTransaction.InsertOneAsync(suggestion);

            var usersInTransaction = db.GetCollection<User>(_dbConnection.UserCollectionName);
            var user = await _userRepository.GetUserAsync(suggestion.Author.Id);
            user.AuthoredSuggestions.Add(new BasicSuggestion(suggestion));
            await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);
            await session.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}


