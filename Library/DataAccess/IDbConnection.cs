using MongoDB.Driver;

namespace Library.DataAccess;

public interface IDbConnection
{
    MongoClient Client { get; }
    string CategoryCollectionName { get; }
    string SuggestionCollectionName { get; }
    string StatusCollectionName { get; }
    string UserCollectionName { get; }
    IMongoCollection<Suggestion> SuggestionCollection { get; }
    IMongoCollection<Status> StatusCollection { get; }
    IMongoCollection<Category> CategoryCollection { get; }
    IMongoCollection<User> UserCollection { get; }

}