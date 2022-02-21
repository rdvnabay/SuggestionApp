namespace Library.DataAccess.Abstract;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task CreateCategory(Category category);
}
