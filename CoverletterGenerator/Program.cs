using CoverletterGenerator.Features.CreateUser;
using CoverletterGenerator.Features.GenerateCoverLetter;
using CoverletterGenerator.Features.LoginUser;
using CoverletterGenerator.Features.UploadCV;
using CoverletterGenerator.Infrastructure.DependencyInjection;
using CoverletterGenerator.Infrastructure.ExceptionHandling;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(
        (doc, _, _) =>
        {
            doc.Components ??= new OpenApiComponents();
            doc.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            doc.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Please enter token",
                Name = "Authorization",
                In = ParameterLocation.Header,
            };
            doc.Security ??= new List<OpenApiSecurityRequirement>();
            doc.Security.Add(
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", doc)] = new List<string>(),
                }
            );
            return Task.CompletedTask;
        }
    );
});
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
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
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
