using PaySmartly.Legislation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<LegislationService>();

app.Run();
