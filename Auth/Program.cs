using Auth.Filters;
using Infrastructure.Auth;
using Infrastructure.Contracts;
using Infrastructure.Common.Impl;
using Infrastructure.Dal;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contracts.Repositories;
using Core.Domain;
using Infrastructure.Dal.Repositories;
using MediatR;
using Application.UseCases;
using Application.UseCaseHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IJsonWebTokenService, JsonWebTokenService>();

builder.Services.AddScoped<IRepository<User>, UserRepository>();

builder.Services.AddMediatR(typeof(UseCase<>), typeof(UseCaseHandler<,>));
builder.Services.AddDbContext<AppDbContext>(o
    => o.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AppDbContext))));
//builder.Services.AddTransient<AppDbContext>();

builder.Services.Configure<AuthOptions>(
    builder.Configuration.GetSection(nameof(AuthOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
