
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonApi.Business;
using PersonApi.Business.Implementations;
using PersonApi.Models.Context;
using PersonApi.Repository;
using PersonApi.Repository.Implementations;

namespace PersonApi
{
    public class Startup
    {

        private readonly ILogger _logger;
        public IConfiguration _configuration { get; }
        public IHostingEnvironment _environment;

        
        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration["MySqlConnection:MySqlConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connectionString));

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
                        Locations = new List<string>() {"db/migrations"},
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
            
            services.AddMvc();
            services.AddApiVersioning();
            services.AddScoped<IPersonBusiness, PersonBusinessImpl>();
            services.AddScoped<IPersonRepository, PersonRepositoryImpl>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
