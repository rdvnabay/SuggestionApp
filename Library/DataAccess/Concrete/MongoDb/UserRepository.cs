namespace Library.DataAccess.Concrete.MongoDb;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    public UserRepository(IDbConnection db)
    {
        _users = db.UserCollection;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var result = await _users.FindAsync(_ => true);
        return result.ToList();
    }

    public async Task<User> GetUserAsync(string id)
    {
        var result = await _users.FindAsync(x => x.Id == id);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<User> GetUserFromAuthentication(string objectId)
    {
        var result = await _users.FindAsync(x => x.ObjectIdentifier == objectId);
        return result.FirstOrDefault();
    }

    public Task CreateUser(User user) => _users.InsertOneAsync(user);

    public Task UpdateUser(User user)
    {
        var filter = Builders<User>.Filter.Eq("Id", user.Id);
        return _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true }); ;
    }
}
