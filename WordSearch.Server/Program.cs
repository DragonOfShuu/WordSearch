
using WordSearch.Server.Controllers.Hubs;
using WordSearch.Server.Services;
using WordSearch.Server.Services.WordGenerator;
using WordSearch.Server.Services.WordSelector;

namespace WordSearch.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add logging providers
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            builder.Services.AddTransient<ISingleplayerGame, SingleplayerGameService>();
            builder.Services.AddSingleton<IWordGenerator, WordGeneratorService>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            // HUBS
            app.MapHub<MultiplayerHub>("/hubs/multiplayer");
            app.MapHub<SingleplayerHub>("/hubs/singleplayer");
            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
