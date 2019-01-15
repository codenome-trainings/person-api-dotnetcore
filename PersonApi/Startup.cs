using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PersonApi.Business;
using PersonApi.Business.Implementations;
using PersonApi.Hypermedia;
using PersonApi.Models.Context;
using PersonApi.Repository;
using PersonApi.Repository.Generics;
using PersonApi.Repository.Implementations;
using PersonApi.Security.Configuration;
using RestWithASPNETUdemy.Business.Implementattions;
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

            ExecuteMigration(connectionString);
            
            //Inicio da execução de login
            
            var signigConfiguration = new SigningConfigurations();
            services.AddSingleton(signigConfiguration);

            var tokenConfiguration = new TokenConfiguration();
            
            new ConfigureFromConfigurationOptions<TokenConfiguration>(_configuration.GetSection("TokenConfiguration"))
                .Configure(tokenConfiguration);

            services.AddSingleton(tokenConfiguration);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signigConfiguration.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidAudience = tokenConfiguration.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;   
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build()
                );
            });
            
            //Fim da execução de login
            

            services.AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml",
                        MediaTypeHeaderValue.Parse("text/xml"));
                    options.FormatterMappings.SetMediaTypeMappingForFormat("json",
                        MediaTypeHeaderValue.Parse("application/json"));
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
                    "v1",
                    new Info
                    {
                        Title = "RESTful API With ASP.NET Core 2.0",
                        Version = "v1"
                    });
            });


            services.AddScoped<IPersonBusiness, PersonBusinessImpl>();
            services.AddScoped<IBookBusiness, BookBusinessImpl>();
            services.AddScoped<ILoginBusiness, LoginBusinessImpl>();
            services.AddScoped<IUserRepository, UserRepositoryImpl>();


            services.AddScoped<IPersonRepository, PersonRepositoryImpl>();
            services.AddScoped<IBookRepository, BookRepositoryImpl>();

            services.AddScoped(
                typeof(IRepository<>),
                typeof(GenericRepository<>)
            );
        }

        private void ExecuteMigration(string connectionString)
        {
            if (_environment.IsDevelopment())
            {
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
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "DefaultApi",
                    "{controller=Values}/{id?}");
            });
        }
    }
}