using VotingApp.Api.Extensions;

namespace VotingApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.RegisterServices();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(builder.Configuration.GetValue<string>("AllowedCorsPolicy")!);
        app.UseHttpsRedirection();
        app.RegisterEndpointDefinitions();
        app.Run();
    }
}