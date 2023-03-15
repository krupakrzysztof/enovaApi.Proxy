using enovaApi.Proxy;
using Microsoft.Extensions.Configuration;

Cryptography.Configure();
if (args.Contains("--generateCrypto"))
{
    Console.WriteLine($"IV: {Cryptography.GetIV()}");
    Console.WriteLine($"Key: {Cryptography.GetKey()}");
    Environment.Exit(0);
}

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.WebHost.UseKestrel();
if (args.Any(x => x.StartsWith("--urls")))
{
    builder.WebHost.UseUrls(args.First(x => x.StartsWith("--urls")).Replace("--urls=", "").Split(";", StringSplitOptions.RemoveEmptyEntries));
}
builder.Configuration.AddJsonFile("keys.json", true, true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// key and iv can be changed by Cryptography.GetIV() and Cryptography.GetKey() then replaced below
Cryptography.SetIV(builder.Configuration.GetValue<string>("IV") ?? throw new Exception("IV value cannot be null."));
Cryptography.SetKey(builder.Configuration.GetValue<string>("Key") ?? throw new Exception("Key value cannot be null"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
