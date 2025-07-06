using ServiceOrder.Infra.Extensions;
using ServiceOrder.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapOrderEndpoints();
app.MapServiceEndpoints();

app.Run();
