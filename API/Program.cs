using Microsoft.EntityFrameworkCore;
using BankTransferApi.Mappings;

using BankTransferApi.Data;
using BankTransferApi.Repositories;
using BankTransferApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BankContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BankDatabase")));

// Add services to the container.
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

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
