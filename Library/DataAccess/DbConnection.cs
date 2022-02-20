using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Library.DataAccess;

public class DbConnection
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;
    private readonly string _connectionString = "MongoDB";
    public MongoClient Client { get; private set; }

    public string SuggestionCollectionName { get; private set; } = "suggestions";
    public string CategoryCollectionName { get; private set; } = "categories";
    public string StatusCollectionName { get; private set; } = "status";
    public string UserCollectionName { get; private set; } = "users";

    public IMongoCollection<Suggestion> SuggestionCollection { get; private set; }
    public IMongoCollection<Category> CategoryCollection { get; private set; }
    public IMongoCollection<Status> StatusCollection { get; private set; }
    public IMongoCollection<User> UserCollection { get; private set; }

    public DbConnection(IConfiguration configuration)
    {
        _configuration = configuration;
        Client = new MongoClient(_configuration.GetConnectionString(_connectionString));
        _database = Client.GetDatabase(_configuration["DatabaseName"]);

        SuggestionCollection = _database.GetCollection<Suggestion>(SuggestionCollectionName);
        CategoryCollection = _database.GetCollection<Category>(CategoryCollectionName);
        StatusCollection = _database.GetCollection<Status>(StatusCollectionName);
        UserCollection = _database.GetCollection<User>(UserCollectionName);
    }
}
