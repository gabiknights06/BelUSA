using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaxCalculation.Core.Strategy;
using TaxCalculation.Persistent.Calculator;
using TaxCalculation.Persistent.Strategy;
using TaxCalculation.Persistent.Utilities;
using TaxCalculation.Services;
using TaxCalculation.Services.Interfaces;
using TaxCalculation.Versioning;
using TaxCalculation.Middlewares;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using TaxCalculation.Utilities;

namespace TaxCalculation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Swagger
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                   // options.OperationFilter<SwaggerDefaultValues>();
                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                    options.ExampleFilters(); // for Swagger documentation default value example - REQUEST

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Please enter your Bearer Token.",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                            },
                            new string[]{ }
                        }
                    });
                });

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblyOf<SwaggerRequestExampleValue>(); // for Swagger documentation default value example - REQUEST
            #endregion

            //Get API Connection Details in environment variables and store it in static variable
            APIConnectionDetails.TaxJarBaseURL = Environment.GetEnvironmentVariable("TAX_JAR_BASE_URL");
            APIConnectionDetails.TaxJarAPIKey = Environment.GetEnvironmentVariable("TAX_JAR_API_KEY");

            //Dependency Injection
            services.AddScoped<ITaxService, TaxService>();

            //DI for Strategy Pattern
            services.AddScoped<ITaxCalculatorStrategyFactory, TaxCalculatorStrategyFactory>();
            services.AddScoped<ITaxCalculatorStrategy, TaxJar>();

            #region Naming Policy
            services.AddControllers()
               .AddNewtonsoftJson(setup =>
               {
                   setup.SerializerSettings.ContractResolver = new DefaultContractResolver()
                   {
                       NamingStrategy = new SnakeCaseNamingStrategy()
                   };
               });
            #endregion

           // services.AddControllers();
    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
   
            var pathBase = "/bel-usa";
            app.UsePathBase(pathBase);

            if (env.IsDevelopment())
            {
                #region swagger

                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"{pathBase}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                        options.RoutePrefix = "";
                    });

                #endregion
            }

            app.UseRouting();

            app.UseAuthorization();

            #region Middleware

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseMiddleware<BasicAuthMiddleware>();

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
