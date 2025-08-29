using Shared.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHTASwagger();

var app = builder.Build();

app.UseCors(cor =>
{
    cor.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.UseHTASwagger();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();