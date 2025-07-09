using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app = builder.Build();
app.UseCors();

var products = new List<Product>
{
    new Product(1, "Apple"),
    new Product(2, "Banana"),
    new Product(3, "Carrot")
};

app.MapGet("/products", () => products);

app.MapGet("/products/{id:int}", ([FromRoute] int id) =>
    products.FirstOrDefault(p => p.Id == id) is { } p ? Results.Ok(p) : Results.NotFound());

app.MapPost("/products", ([FromBody] Product product) =>
{
    product = product with { Id = products.Count + 1 };
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();

record Product(int Id, string Name);
