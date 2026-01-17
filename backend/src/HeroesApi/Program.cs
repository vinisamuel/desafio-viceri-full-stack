var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.AddAppSettings();

builder.Services
    .AddHeroesAppDatabase()
    .AddBusinessServices();

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(["http://localhost:5175"]);
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
    })
    .AddControllers()
    .AddCamelCaseJsonOptions();

builder.Services.AddSwaggerDocumentation("Heroes API", "API para gerenciamento de super-heróis");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseSwaggerDocumentation(app.Environment);

app.UseHealthChecks("/status");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
