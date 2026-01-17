using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HeroesApi.Extensions;

public static class SwaggerExtensions
{
    /// <summary>
    /// Adiciona a documentação do Swagger na API
    /// </summary>
    /// <param name="services"></param>
    /// <param name="title">Título da documentação</param>
    /// <param name="description">Descrição da documentação</param>
    /// <param name="version">Versão da documentação. (Padrão: v1)</param>
    /// <param name="addBearerToken">Indicador se adiciona a entrada de token</param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, string title, string description,
        string version = "v1", bool addBearerToken = false)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.EnableAnnotations();

            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Description = description,
                Version = version,
            });

            if (addBearerToken)
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer'[space] and then your token in the text input below. Example: \"Bearer {{TOKEN}}\""
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        });

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env">Interface para identificação do ambiente de execução</param>
    /// <param name="version">Versão da documentação. (Padrão: v1)</param>
    /// <returns></returns>
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IHostEnvironment env, string version = "v1")
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{version}/swagger.json", version));
        }

        return app;
    }
}
