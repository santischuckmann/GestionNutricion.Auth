using GestionNutricionAuth.Infraestructura.Extensiones;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;

// Add services to the container.

var _policyName = "configuracionCors";

builder.Services.AddCors(x =>
{
    x.AddPolicy(_policyName, builder =>
    {
        builder.WithOrigins(Configuration["AllowedHosts"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetPreflightMaxAge(TimeSpan.FromDays(365));
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContexts(Configuration);
builder.Services.AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(_policyName);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
