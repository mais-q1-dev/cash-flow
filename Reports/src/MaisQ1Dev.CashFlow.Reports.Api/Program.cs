using MaisQ1Dev.CashFlow.Reports.Api;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger();
    app.ApplyMigrations();
}

app.ConfigureApp();
app.Run();