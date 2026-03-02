using Microsoft.EntityFrameworkCore;
using Thermus.Api;
using Thermus.Api.Infrastructure;
using Thermus.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IReadingServices, ReadingServices>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailService, MailServices>();
builder.Services.AddDbContext<ThermusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Aplico migraciones automáticamente al arrancar (crea/actualiza tablas si hace falta)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ThermusDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (ctx, next) =>
{
    Console.WriteLine($"REQ {ctx.TraceIdentifier} {ctx.Connection.RemoteIpAddress} {ctx.Request.Method} {ctx.Request.Path}");
    await next();
});
//app.UseHttpsRedirection();
app.MapControllers();


Console.WriteLine("ASPNETCORE_URLS = " + Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
Console.WriteLine("Urls config = " + builder.Configuration["Urls"]);
Console.WriteLine("Environment = " + app.Environment.EnvironmentName);
Console.WriteLine("app.Urls = " + string.Join(", ", app.Urls));

app.Run();

