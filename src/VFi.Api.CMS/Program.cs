using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using VFi.Infra.CMS.FileConfig;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.AddOTelLogs();
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
//    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);
//});
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

var appConfig = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();
if (appConfig != null)
{
    builder.Services.AddSingleton(appConfig);
}
else
{
    appConfig = new AppConfig();
    builder.Services.AddSingleton(appConfig);
}

var codeSyntaxConfig = builder.Configuration.GetSection("CodeSyntaxConfig").Get<CodeSyntaxConfig>();
if (codeSyntaxConfig != null)
{
    builder.Services.AddSingleton(codeSyntaxConfig);
}
else
{
    var codeSyntaxInit = new CodeSyntaxConfig();
    builder.Services.AddSingleton(codeSyntaxInit);
}
builder.Services.Configure<FileUploadConfig>(builder.Configuration.GetSection("FileUpload"));
// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RoutePrefix(new RouteAttribute(appConfig.UrlPrefix ?? "")));
});

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VFiCo CMS Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });

});
builder.Services.AddMediatR(_ =>
{
    _.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    _.NotificationPublisher = new TaskWhenAllPublisher();
    _.NotificationPublisherType = typeof(TaskWhenAllPublisher);
});
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(x =>
           {
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = !string.IsNullOrEmpty(builder.Configuration["Jwt:Audience"]),
                   ValidateLifetime = false,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = builder.Configuration["Jwt:Issuer"],
                   ValidAudience = builder.Configuration["Jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
               };
               x.SecurityTokenValidators.Clear();
               x.SecurityTokenValidators.Add(new JwtSecurityTokenHandler
               {
                   MapInboundClaims = false
               });

               x.Events = new JwtBearerEvents()
               {
                   OnTokenValidated = async ctx =>
                   {
                       var temp = ctx.Principal.Claims;

                       //Add claim if yes
                       var claims = new List<Claim>
                    {
                        new Claim("ConfidentialAccess", "true")
                    };
                       var appIdentity = new ClaimsIdentity(claims);

                       ctx.Principal.AddIdentity(appIdentity);

                   },

                   OnAuthenticationFailed = c =>
                   {
                       c.NoResult();
                       c.Response.StatusCode = 500;
                       c.Response.ContentType = "text/plain";

                       if (builder.Environment.IsDevelopment())
                       {
                           return c.Response.WriteAsync(c.Exception.ToString());
                       }

                       return c.Response.WriteAsync("An error occured processing your authentication.");
                   }


               };


           });

//config log
var logConfig = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Debug()
    .WriteTo.Console();
if (builder.Configuration["Serilog:Elastic"] != null)
{
    logConfig = logConfig.WriteTo.Elasticsearch(ConfigureElasticSink(builder.Configuration, builder.Environment.EnvironmentName));
}
logConfig = logConfig.Enrich.WithProperty("Environment", builder.Environment.EnvironmentName).ReadFrom.Configuration(builder.Configuration);

Log.Logger = logConfig.CreateLogger();

builder.Host.UseSerilog();
//------

//builder.Services.AddOTelTracing(builder.Configuration);
//builder.Services.AddOTelMetrics(builder.Configuration);

builder.Services.AddAspNetUserConfiguration();

VFi.NetDevPack.StartupBase.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

VFi.NetDevPack.StartupBase.ConfigureRequestPipeline(app, builder.Environment);



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger(c =>
{
    c.RouteTemplate = string.IsNullOrEmpty(appConfig.UrlPrefix)
        ? "swagger/{documentName}/swagger.json"
        : appConfig.UrlPrefix + "/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = appConfig.UrlPrefix ?? "";
    c.SwaggerEndpoint(string.IsNullOrEmpty(appConfig.UrlPrefix) ? "/swagger/v1/swagger.json" : $"/{appConfig.UrlPrefix}/swagger/v1/swagger.json", "cms-api");
});

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();




ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["Serilog:Elastic"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}
