
using ExchangePulse.Application.Interfaces;
using ExchangePulse.Application.Services;
using ExchangePulse.Application.Validators;
using ExchangePulse.Infrastructure.BackgroundServices;
using ExchangePulse.Infrastructure.ExternalServices;
using ExchangePulse.Infrastructure.Persistence;
using ExchangePulse.Infrastructure.Persistence.Seed;
using ExchangePulse.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona FluentValidation
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CurrencyDTOValidator>();
builder.Services.AddDbContext<ExchangePulseDbContext>(options => options.UseSqlServer(connectionString));

// Serviços e repositórios de domínio
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

builder.Services.AddScoped<IExchangeMetricService, ExchangeMetricService>();
builder.Services.AddScoped<IExchangeMetricRepository, ExchangeMetricRepository>();

// Serviços externos
builder.Services.AddHttpClient<IExchangeRateFetcher, BcbExchangeRateFetcher>();
builder.Services.AddHttpClient<IBcbDataFetcher, BcbDataFetcher>();

// Serviços de aplicação
builder.Services.AddScoped<ExchangeRateUpdater>();

// Hosted Service em background
builder.Services.AddHostedService<ExchangeRateBackgroundService>();






#region --|Autenticacao e Autorizacao|--
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangePulse API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {

    //         new OpenApiSecurityScheme
    //         {

    //             //Reference = new OpenApiReference
    //             //{
    //             //    Type = ReferenceType.SecurityScheme,
    //             //    Id = "Bearer"
    //             //}
    //         },
    //         new string[] {}

    // });
});
#endregion

builder.Services.AddSwaggerGen(c => {
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Executa o seeder após migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ExchangePulseDbContext>();
        db.Database.EnsureCreated(); // garante que o banco existe
        CurrencySeeder.Seed(db);
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



