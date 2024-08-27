using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VotingApp.Api.UnitTests.Helpers
{
    public static class DbContextHelpers
    {
        public static IServiceCollection RemoveDbContext(this IServiceCollection serviceCollection)
        {
            var descriptor = serviceCollection.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<VotingDbContext>));
            if (descriptor != null)
            {
                serviceCollection.Remove(descriptor);
            }

            return serviceCollection;
        }

        public static IServiceCollection AddInMemorySql(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddDbContext<VotingDbContext>(
                options => { options.UseInMemoryDatabase(Guid.NewGuid().ToString()); },
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);
        }
    }
}
