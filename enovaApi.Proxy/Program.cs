using enovaApi.Proxy;

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
Cryptography.SetIV("WfEwqyJ4Pm3b0F6nnrLLKQ==");
Cryptography.SetKey("BJaVbPFXNBN59bZCn1ORpKTH7b7UrJ4zj7KFBrHMaSk=");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
