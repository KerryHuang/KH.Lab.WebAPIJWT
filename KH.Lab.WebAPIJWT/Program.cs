using KH.Lab.WebAPIJWT.Cache;
using KH.Lab.WebAPIJWT.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presco.Utility.Caching.Redis;
using Presco.Utility.Cookie;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "JWT WebAPI Lab"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

#region Cache
// Custom Cache
builder.Services.AddScoped<ICacheService, CacheService>();
// Presco Cache in memory
//builder.Services.AddPrescoCachingInMemory(new MemoryConfiguration
//{
//    AllMethodsEnableCache = false,
//    Expire = TimeSpan.FromSeconds(30)
//});
// Presco Cache in Redis
builder.Services.AddPrescoCachingInRedis(new RedisConfiguration
{
    AllMethodsEnableCache = false,
    Expire = TimeSpan.FromMinutes(double.Parse(builder.Configuration["Redis:Expire"])),
    KeyPrefix = builder.Configuration["Redis:KeyPrefix"],
    ConnectionString = builder.Configuration["Redis:UrlConnection"]
});
#endregion

#region Cookie
builder.Services.AddPrescoCookie(new CookieConfiguration()
{
    Expire = TimeSpan.FromMinutes(double.Parse(builder.Configuration["Cookie:Expire"])),
    KeyPrefix = builder.Configuration["Cookie:KeyPrefix"]
});
#endregion

builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = KH.Lab.WebAPIJWT.ConfigurationManager.AppSetting["JWT:ValidIssuer"],
            ValidAudience = KH.Lab.WebAPIJWT.ConfigurationManager.AppSetting["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KH.Lab.WebAPIJWT.ConfigurationManager.AppSetting["JWT:Secret"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "JWT WebAPI Lab");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.Run();
