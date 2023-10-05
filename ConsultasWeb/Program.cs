

using ConsultasWeb.config;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la cadena de conexión desde appsettings.json
var connectionString = configuration.GetConnectionString("OracleConnection");

//builder.Services.AddDbContext<Contexto>
	//(options => options.UseOracle(connectionString));

// Configurar la conexión Oracle
OracleConnection oracleConnection = new OracleConnection(connectionString);
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", builder =>
	{
		builder.WithOrigins("http://localhost:4200") 
			   .AllowAnyHeader()
			   .AllowAnyMethod();
	});
});


builder.Services.AddSingleton(oracleConnection);

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
