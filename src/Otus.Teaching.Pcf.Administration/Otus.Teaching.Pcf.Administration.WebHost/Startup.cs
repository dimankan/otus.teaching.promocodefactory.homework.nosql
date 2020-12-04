using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Otus.Teaching.Pcf.Administration.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.Administration.DataAccess;
using Otus.Teaching.Pcf.Administration.DataAccess.Data;
using Otus.Teaching.Pcf.Administration.DataAccess.Repositories;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Otus.Teaching.Pcf.Administration.DataAccess.MongoAdmin;

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
            services.AddControllers().AddMvcOptions(x=> 
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IDbInitializer, MongoDbInitializer>();

            services.Configure<MongoAdministrationDatabaseSettings>(
                    Configuration.GetSection("MongoDatabaseSettings"));

            services.AddSingleton<IMongoAdministrationDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoAdministrationDatabaseSettings>>().Value);

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