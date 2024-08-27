using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace VotingApp.Api.UnitTests.Helpers
{
    public class InMemoryDbApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(collection =>
            {
                collection.RemoveDbContext().AddInMemorySql();
            });

            return base.CreateHost(builder);
        }
    }
}
