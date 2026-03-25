using SysTickets.Core.Interfaces;
using SysTickets.Data;
using SysTickets.Services;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;  

//Inyección de dependecias (reposotorios y servicios)
builder.Services.AddScoped<ITicketRepository>(_ => new TicketRepository(connectionString));
builder.Services.AddScoped<ICatalogoRepository>(_ => new CatalogoRepository(connectionString));
builder.Services.AddScoped<ITicketService, TicketService>();

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();
app.Run();