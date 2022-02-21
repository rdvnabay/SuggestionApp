using Microsoft.Extensions.Caching.Memory;

namespace Library.DataAccess.Concrete.MongoDb;

public class StatusRepository : IStatusRepository
{
    private readonly IMongoCollection<Status> _status;
    private readonly IMemoryCache _cache;
    private const string _cacheName = "StatusData";

    public StatusRepository(IDbConnection db)
    {
        _status = db.StatusCollection;
    }
    public Task CreateStatus(Status status) => _status.InsertOneAsync(status);

    public async Task<List<Status>> GetAllStatusAsync()
    {
        var output = _cache.Get<List<Status>>(_cacheName);

        if (output is null)
        {
            var result = await _status.FindAsync(_ => true);
            output = result.ToList();

            _cache.Set(_cacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }
}
