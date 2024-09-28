using ApiCrud.Data;
using ApiCrud.Estudantes;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDBContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.AddRotasEstudantes();

app.Run();