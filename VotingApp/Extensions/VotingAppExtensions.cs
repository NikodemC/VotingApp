using Application.Abstractions;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using VotingApp.Api.Abstractions;

namespace VotingApp.Api.Extensions
{
    public static class VotingAppExtensions
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {

            builder.AddDbContext();
            builder.AddSwagger();
            builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
            builder.Services.AddScoped<IVoterRepository, VoterRepository>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }

        public static void RegisterEndpointDefinitions(this WebApplication app)
        {
            var endpointDefinitions = typeof(Program).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition)) && t is { IsAbstract: false, IsInterface: false })
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>();

            foreach (var endpoint in endpointDefinitions)
            {
                endpoint.RegisterEndpoints(app);
            }
        }

        private static void AddDbContext(this WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("DeviceServiceDb");
            var majorVersion = builder.Configuration.GetValue<int>("DatabaseVersion:Major");
            var minorVersion = builder.Configuration.GetValue<int>("DatabaseVersion:Minor");
            var patchVersion = builder.Configuration.GetValue<int>("DatabaseVersion:Patch");
            var serverVersion = new MySqlServerVersion(new Version(majorVersion, minorVersion, patchVersion));
            builder.Services.AddDbContext<VotingDbContext>(opt => opt.UseMySql(cs, serverVersion));
        }

        private static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}
