using System.Text;
using System.Text.Json.Serialization;
using DinkToPdf;
using DinkToPdf.Contracts;
using InsurancePoliciesSystem.Api.BackOffice.Agreements;
using InsurancePoliciesSystem.Api.SellPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;
using InsurancePoliciesSystem.Api.Shared;
using InsurancePoliciesSystem.Api.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});

builder.Services.AddEndpointsApiExplorer();

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement()
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
};
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", securityScheme);
    o.AddSecurityRequirement(securityReq);
});
builder.Services.AddCors();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IWorkInsuranceRepository, InMemoryWorkInsuranceRepository>();
builder.Services.AddSingleton<IAgreementsRepository, InMemoryAgreementsRepository>();
builder.Services.AddSingleton<ISearchPolicyStorage, InMemorySearchPolicyStorage>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<PolicyPdfGenerator, Test>();
builder.Services.AddTransient<PolicyPdfGenerator, WorkInsurancePdfGenerator>();
builder.Services.AddTransient<CancelPolicyService>();
builder.Services.AddTransient<PolicyCanceller, WorkInsurancePolicyCanceller>();
builder.Services.AddTransient<WorkInsurancePdfGenerator>();
builder.Services.AddTransient<WorkInsurancePolicyPdfModelProvider>();
builder.Services.AddTransient<PdfProvider>();
builder.Services.AddTransient<IClock, Clock>();
builder.Services.AddTransient<IPriceConfigurationService, InMemoryPriceConfigurationService>();



var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.Run();