using PharmManager.Orders.API.Extensions;
using PharmManager.Orders.API.Utils.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register dependencies
var massTransitConfig = builder.Configuration.GetSection("MassTransitConfig").Get<MassTransitConfig>();
builder.Services.RegisterMasstransit(massTransitConfig);
builder.Services.RegisterDatabaseProvider(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.RegisterDependencies();
builder.Services.RegisterSwagger("Orders.API");
builder.Services.RegisterMapper();
builder.Services.RegisterSettings(builder.Configuration);

//Register Authentication and Authorization
builder.Services.RegisterAuthenticationSettings();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
