using dotenv.net;
using iscaslune.Api.CrossCutting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        DotEnv.Load();
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.InjectDomain();
        builder.Services.InjectInfrastructure();
        builder.Services.InjectApplication();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();
        
        app.UseCors();

        app.MapControllers();

        app.Run();
    }
}