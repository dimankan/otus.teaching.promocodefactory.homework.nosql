using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.DataAccess.Data;
using Otus.Teaching.Pcf.Administration.DataAccess.Repositories;
using System;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Otus.Teaching.Pcf.Administration.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionStringKey = "MongoAdministrationDb:ConnectionString";
            string dbNameKey = "MongoAdministrationDb:Database";

            string connectionString = Environment.GetEnvironmentVariable(connectionStringKey);
            if (connectionString == null)
            {
                connectionString = Configuration[connectionStringKey];
            }

            string dbName = Environment.GetEnvironmentVariable(dbNameKey);
            if (dbName == null)
            {
                dbName = Configuration[dbNameKey];
            }

            services
                .AddSingleton<IMongoClient>(_ => new MongoClient(connectionString))
                .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IMongoClient>().GetDatabase(dbName))
                .AddScoped(serviceProvider => serviceProvider.GetRequiredService<IMongoClient>().StartSession());

            services.AddControllers().AddMvcOptions(x =>
                x.SuppressAsyncSuffixInActionNames = false);

            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IDbInitializer, MongoDbInitializer>();

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory Administration API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbInitializer.InitializeDb();
        }
    }
}