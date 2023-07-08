using InsurancePoliciesSystem.Api.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IUserRepository, InMemoryUserRepository>();


var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();