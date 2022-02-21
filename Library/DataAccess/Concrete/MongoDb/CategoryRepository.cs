using Microsoft.Extensions.Caching.Memory;

namespace Library.DataAccess.Concrete.MongoDb;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _categories;
    private readonly IMemoryCache _cache;
    private const string _cacheName = "CategoryData";

    public CategoryRepository(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _categories = db.CategoryCollection;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var output = _cache.Get<List<Category>>(_cacheName);

        if (output is null)
        {
            var result = await _categories.FindAsync(_ => true);
            output = result.ToList();

            _cache.Set(_cacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }

    public Task CreateCategory(Category category) => _categories.InsertOneAsync(category);
}
