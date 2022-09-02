using PharmManager.Products.API.Extensions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

// Register dependencies
builder.Services.RegisterMasstransit();
builder.Services.RegisterDatabaseProvider(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.RegisterDependencies();
builder.Services.RegisterSwagger("Products.API");
builder.Services.RegisterMapper();

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
