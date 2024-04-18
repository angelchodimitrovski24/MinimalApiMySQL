using Microsoft.EntityFrameworkCore;
using MinimalOrderProcessingAPI;
using MinimalOrderProcessingAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
              options.UseMySQL("Server=localhost;Database=myDataBase;Uid=root;Pwd=krakadak-ula2011;"));

// Register Services
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<ContactHandler>();
builder.Services.AddScoped<OrderHandler>();

// dotnet ef migrations add InitialMigration
// dotnet ef database update

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Endpoint for getting all contacts
app.MapGet("/contacts", async (DataContext context, ContactHandler contactHandler) =>
{
    return await contactHandler.GetAllContactsAsync(context);
});

// Endpoint for getting a specific contact by ID
app.MapGet("/contacts/{id}", async (DataContext context, ContactHandler contactHandler, int id) =>
{
    return await contactHandler.GetContactByIdAsync(context, id);
});

// Endpoint for creating a new contact id
app.MapPost("/contacts", async (DataContext context, ContactHandler contactHandler, Contact contact) =>
{
    return await contactHandler.CreateContactAsync(context, contact);
});

// Endpoint for updating an existing contact
app.MapPut("/contacts/{id}", async (DataContext context, ContactHandler contactHandler, int id, Contact updatedContact) =>
{
    return await contactHandler.UpdateContactAsync(context,id, updatedContact);
});

// Endpoint for deleting a contact by ID
app.MapDelete("/contacts/{id}", async (DataContext context, ContactHandler contactHandler, int id) =>
{
    return await contactHandler.DeleteContactAsync(context, id);
});




// Endpoint for getting all orders
app.MapGet("/orders", async (DataContext context) =>
{
    var orders = await context.Orders.ToListAsync();
    return Results.Ok(orders);
});

// Endpoint for getting a specific order by ID
app.MapGet("/orders/{id}", async (DataContext context, int id) =>
{
    var order = await context.Orders.FindAsync(id);
    if (order == null)
        return Results.NotFound("Order not found.");

    return Results.Ok(order);
});

// Endpoint for creating a new order
app.MapPost("/orders", async (DataContext context, Order order) =>
{
    context.Orders.Add(order);
    await context.SaveChangesAsync();
    return Results.Created($"/orders/{order.OrderId}", order);
});

// Endpoint for updating an existing order
app.MapPut("/orders/{id}", async (DataContext context, int id, Order updatedOrder) =>
{
    var existingOrder = await context.Orders.FindAsync(id);
    if (existingOrder == null)
        return Results.NotFound("Order not found.");

    existingOrder.ContactId = updatedOrder.ContactId;
    existingOrder.TotalAmount = updatedOrder.TotalAmount;

    await context.SaveChangesAsync();
    return Results.Ok(existingOrder);
});

// Endpoint for deleting an order by ID
app.MapDelete("/orders/{id}", async (DataContext context, int id) =>
{
    var order = await context.Orders.FindAsync(id);
    if (order == null)
        return Results.NotFound("Order not found.");

    context.Orders.Remove(order);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
