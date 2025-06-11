using Microsoft.Azure.Cosmos;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetSection("CosmosDb");
var cosmosClient = new CosmosClient(config["Account"], config["Key"]);
builder.Services.AddSingleton(new CosmosDbService(cosmosClient, config["DatabaseName"], config["ContainerName"]));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
