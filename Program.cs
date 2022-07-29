using HarbintonApi.Interfaces;
using HarbintonApi.Services;
using HarbintonApi.Sql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISql, SqlConnect>();
builder.Services.AddScoped<IBillProcessing, BillProcessing>();
builder.Services.AddScoped<IFundTransfer, FundsTransfer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
