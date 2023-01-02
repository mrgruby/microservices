using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. In startup.cs, this would be in the ConfigureServices() method

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//if (app.Environment.IsProduction())
//{
Console.WriteLine("--> Using Sql Server Database");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
//}
//else
//{
//Console.WriteLine("--> Using InMem Database");
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
//}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddScoped<IMessageBusClient, MessageBusClient>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddGrpc();

Console.WriteLine($"--> CommandService endpoint {builder.Configuration["CommandService"]}");


var app = builder.Build();
// Configure the HTTP request pipeline. In startup.cs, this would be in the Configure() method
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcPlatformService>();
});

//Add test data to the InMem database. We send over the environment check, to determine if we want to add migrations to production db,
//or seed the InMem dev database
PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
