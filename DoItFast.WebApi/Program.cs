using DoItFast.Application;
using DoItFast.Application.Helpers;
using DoItFast.Infrastructure.Persistence;
using DoItFast.Infrastructure.Shared;
using DoItFast.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSharedInfrastructureServices();
builder.Services.AddPersistenceInfrastructureServices(builder.Configuration);
//builder.Services.AddIdentityInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationLayerServices(builder.Configuration);
builder.AddWebApiServices();

// Add middlewares.

await using var app = builder.Build();

Task.Run(async () => await app.Services.ApplyPendingChanges(default)).Wait();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerExtension();
}

app.UseErrorHandlingMiddleware();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSpa();

await app.RunAsync();
