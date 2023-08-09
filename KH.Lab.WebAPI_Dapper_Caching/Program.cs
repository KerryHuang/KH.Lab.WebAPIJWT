using Autofac.Core;
using Dapper.Extensions.Caching.Memory;
using Dapper.Extensions.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDapperForSQLite();

#region ResponseCache
builder.Services.AddResponseCaching();
#endregion

#if DAPPER_EXTENSIONS_CACHE_MEMORY

#endif

builder.Services.AddDapperCachingInMemory(new MemoryConfiguration
{
    AllMethodsEnableCache = false
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region ResponseCache
app.UseResponseCaching();
#endregion

app.Run();
