namespace Library.DataAccess.Abstract;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task<User> GetUserAsync(string id);
    Task<User> GetUserFromAuthentication(string objectId);
    Task<List<User>> GetUsersAsync();
    Task UpdateUser(User user);
}