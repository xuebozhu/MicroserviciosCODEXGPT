using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5001");
builder.Services.AddHttpClient("products", c =>
{
    c.BaseAddress = new Uri("http://localhost:5000"); // ProductService default
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();

var orders = new List<Order>
{
    new Order(1, 1, 2)
};

app.MapGet("/orders", async ([FromServices] IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("products");
    var result = new List<object>();
    foreach (var order in orders)
    {
        try
        {
            var product = await client.GetFromJsonAsync<Product>($"/products/{order.ProductId}");
            result.Add(new { order.Id, Product = product?.Name ?? "unknown", order.Quantity });
        }
        catch
        {
            result.Add(new { order.Id, Product = "unavailable", order.Quantity });
        }
    }
    return result;
});

app.MapPost("/orders", ([FromBody] OrderDto dto) =>
{
    var order = new Order(orders.Count + 1, dto.ProductId, dto.Quantity);
    orders.Add(order);
    return Results.Created($"/orders/{order.Id}", order);
});

app.Run();

record Order(int Id, int ProductId, int Quantity);
record OrderDto(int ProductId, int Quantity);
record Product(int Id, string Name);
