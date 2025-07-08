using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5002");

builder.Services.AddHttpClient("orders", c =>
{
    c.BaseAddress = new Uri("http://localhost:5001");
});

var app = builder.Build();

var shipments = new List<Shipment>();

app.MapGet("/shipments", async ([FromServices] IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("orders");
    var orders = await client.GetFromJsonAsync<List<OrderDto>>("/orders") ?? new List<OrderDto>();

    return shipments.Select(s => new
    {
        s.Id,
        s.OrderId,
        s.ShipDate,
        Order = orders.FirstOrDefault(o => o.Id == s.OrderId)
    });
});

app.MapPost("/shipments", ([FromBody] ShipmentCreateDto dto) =>
{
    var shipment = new Shipment(shipments.Count + 1, dto.OrderId, DateTime.UtcNow);
    shipments.Add(shipment);
    return Results.Created($"/shipments/{shipment.Id}", shipment);
});

app.Run();

record Shipment(int Id, int OrderId, DateTime ShipDate);
record ShipmentCreateDto(int OrderId);
record OrderDto(int Id, string Product, int Quantity);
