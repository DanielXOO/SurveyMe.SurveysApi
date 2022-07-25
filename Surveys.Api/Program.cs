using System.Text.Json.Serialization;
using IdentityServer4.AccessTokenValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Refit;
using SurveyMe.Common.Logging;
using SurveyMe.Common.Time;
using Surveys.Api.Extensions;
using Surveys.Api.Handlers;
using Surveys.Data;
using Surveys.Data.Abstracts;
using Surveys.Data.Refit;
using Surveys.Services;
using Surveys.Services.Abstracts;
using Surveys.Services.Automapper.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logBuilder =>
{
    logBuilder.AddLogger();
    logBuilder.AddFile(builder.Configuration.GetSection("Serilog:FileLogging"));
});

var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

builder.Services.AddDbContext<SurveysDbContext>(options
    => options.UseSqlServer(connectionString));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "Surveys.Api.xml");
    options.IncludeXmlComments(filePath);
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddMaps(typeof(Program).Assembly);
    configuration.AddProfile(new QueueModelsProfile());
    configuration.AddProfile(new SurveyOptionsProfile());
    configuration.AddProfile(new SurveyProfile());
});

builder.Services.AddScoped<ISurveysService, SurveysService>();
builder.Services.AddScoped<ISurveysUnitOfWork, SurveysUnitOfWork>();
builder.Services.AddScoped<ISurveyPersonService, SurveyPersonService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthorizeHandler>();

builder.Services.AddSingleton<ISystemClock, SystemClock>();

builder.Services.AddControllers();

builder.Services.AddRefitClient<ISurveyPersonOptionsApi>().ConfigureHttpClient(config =>
{
    var stringUrl = builder.Configuration.GetConnectionString("GatewayURL");
    config.BaseAddress = new Uri(stringUrl);
}).AddHttpMessageHandler<AuthorizeHandler>();

builder.Services.AddRefitClient<IAuthenticationApi>().ConfigureHttpClient(config =>
{
    var stringUrl = builder.Configuration.GetConnectionString("GatewayURL");
    config.BaseAddress = new Uri(stringUrl);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "survey-api-cache";
});

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = "http://authentication-api";
        options.RequireHttpsMetadata = false;
        options.ApiName = "Survey.Api";
        options.ApiSecret = "survey_secret";
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.CreateDbIfNotExists();

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();