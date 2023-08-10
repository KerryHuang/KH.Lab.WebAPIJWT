using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dapper.Extensions.Caching.Memory;
using Dapper.Extensions.Caching.Redis;
using Dapper.Extensions.SQLite;
using KH.Lab.WebAPI_Dapper_Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Add Dapper
builder.Services.AddDapperForSQLite(monitorBuilder =>
{
    monitorBuilder.Threshold = 10;
    monitorBuilder.EnableLog = true;
    monitorBuilder.AddMonitorHandler<MyMonitorHandler>();
});
#endregion

#region Enable Caching
// Response Cache
builder.Services.AddResponseCaching();
// Memory Cache
builder.Services.AddMemoryCache();
// Dapper Memory Cache
//builder.Services.AddDapperCachingInMemory(new MemoryConfiguration
//{
//    AllMethodsEnableCache = false,
//    Expire = TimeSpan.FromSeconds(30)
//});
// Dapper Redis Cache 
builder.Services.AddDapperCachingInRedis(new RedisConfiguration()
{
    AllMethodsEnableCache = false,
    Expire = TimeSpan.FromSeconds(30),
    ConnectionString = builder.Configuration["RedisURL"]
});
#endregion

#region Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister()));
#endregion

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
