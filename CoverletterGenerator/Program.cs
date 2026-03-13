using CoverletterGenerator.Features.CreateUser;
using CoverletterGenerator.Features.GenerateCoverLetter;
using CoverletterGenerator.Features.LoginUser;
using CoverletterGenerator.Features.UploadCV;
using CoverletterGenerator.Infrastructure.DependencyInjection;
using CoverletterGenerator.Infrastructure.ExceptionHandling;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddPostgres(builder.Configuration);
builder.Services.AddAnthropicService(builder.Configuration);
builder.Services.AddPlaywrightServices();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapCreateUserEndpoint();
app.MapLoginUserEndpoint();
app.MapUploadCVEndpoint();
app.MapGenerateCoverLetterEndpoint();

app.Run();
