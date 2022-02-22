using Library.DataAccess.Abstract;
using Library.DataAccess.Concrete.MongoDb;

namespace BlazorServerWebUI;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMemoryCache();

        builder.Services.AddSingleton<IDbConnection, DbConnection>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<ISuggestionRepository, SuggestionRepository>();
        builder.Services.AddSingleton<IStatusRepository, StatusRepository>();
        builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
    }
}

