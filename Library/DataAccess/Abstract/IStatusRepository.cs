namespace Library.DataAccess.Abstract;

public interface IStatusRepository
{
    Task<List<Status>> GetAllStatusAsync();
    Task CreateStatus(Status status);
}
