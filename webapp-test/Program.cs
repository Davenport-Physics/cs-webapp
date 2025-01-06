using System.Text.Json;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


var messages = app.MapGroup("/messages");

messages.MapGet("/", () => {

    return "TODO!";

});

messages.MapGet("/sse", static async (HttpContext ctx, CancellationToken ct) => {

    ctx.Response.Headers.TryAdd("Content-Type", "test/event-stream");
    Subscription sub = MessagePassing.Subscribe();

    while (!ct.IsCancellationRequested) {

        var message = await sub.Read();
        if (message != null) {

            await ctx.Response.WriteAsJsonAsync(message);
            await ctx.Response.Body.FlushAsync();
            
        }

    }

    sub.Unsubscribe();

});

messages.MapPost("/submit", async (Message message) => {

    Console.WriteLine(JsonSerializer.Serialize(message));
    await message.Submit();

});



app.Run();

record UserMessage(string user_name, string message, string sent_time);

