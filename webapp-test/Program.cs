using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

string settings_file = app.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json";


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

var messages = app.MapGroup("/messages");
messages.MapGet("/", () => {



});

messages.MapPost("/submit", async (Message message) => {

    Console.WriteLine(JsonSerializer.Serialize(message));
    await message.Submit();

});



app.Run();

record UserMessage(string user_name, string message, string sent_time);

