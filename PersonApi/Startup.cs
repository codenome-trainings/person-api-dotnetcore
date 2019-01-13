using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using PersonApi.Business;
using PersonApi.Business.Implementations;
using PersonApi.Hypermedia;
using PersonApi.Models.Context;
using PersonApi.Repository;
using PersonApi.Repository.Generics;
using PersonApi.Repository.Implementations;
using Swashbuckle.AspNetCore.Swagger;
using Tapioca.HATEOAS;

namespace PersonApi
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IHostingEnvironment _environment;


        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration["MySqlConnection:MySqlConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connectionString));

            if (_environment.IsDevelopment())
                try
                {
                    var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    var evolve = new Evolve.Evolve(
                        "evolve.json",
                        evolveConnection,
                        msg => _logger.LogInformation(msg))
                    {
                        Locations = new List<string> {"db/migrations"},
                        IsEraseDisabled = true
                    };

                    evolve.Migrate();
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Database Migration Failed: ", e);
                    throw e;
                }

            services.AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("text/xml"));
                    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

                })
                .AddXmlSerializerFormatters();

            var filtertOptions = new HyperMediaFilterOptions();
            filtertOptions.ObjectContentResponseEnricherList.Add(new BookEnricher());
            filtertOptions.ObjectContentResponseEnricherList.Add(new PersonEnricher());
            services.AddSingleton(filtertOptions);
            
            
            services.AddApiVersioning(option => option.ReportApiVersions = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1.0",
                    new Info
                    {
                        Title = "RESTful API With ASP.NET Core 2.0",
                        Version = "v1.0"
                    });
            });
            
            
            
            services.AddScoped<IPersonBusiness, PersonBusinessImpl>();
            services.AddScoped<IBookBusiness, BookBusinessImpl>();


            services.AddScoped<IPersonRepository, PersonRepositoryImpl>();

            services.AddScoped(
                typeof(IRepository<>),
                typeof(GenericRepository<>)
            );
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "My API V1");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "DefaultApi",
                    template: "{controller=Values}/{id?}");
            });
        }
    }
}