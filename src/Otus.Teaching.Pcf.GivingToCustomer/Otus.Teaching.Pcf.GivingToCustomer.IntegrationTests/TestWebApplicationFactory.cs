using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using Otus.Teaching.Pcf.GivingToCustomer.Core.Domain;
using Otus.Teaching.Pcf.GivingToCustomer.DataAccess;
using Otus.Teaching.Pcf.GivingToCustomer.Integration;
using Otus.Teaching.Pcf.GivingToCustomer.IntegrationTests.Data;

namespace Otus.Teaching.Pcf.GivingToCustomer.IntegrationTests
{
    public class TestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // remove MongoDb elements
                RemoveElementFromServices(services, typeof(IMongoCollection<Customer>));
                RemoveElementFromServices(services, typeof(IMongoCollection<PromoCode>));
                RemoveElementFromServices(services, typeof(IMongoCollection<Preference>));
                RemoveElementFromServices(services, typeof(IMongoDatabase));
                RemoveElementFromServices(services, typeof(IMongoClient));

                // Init test database
                IMongoDatabase dbContext = InitMongoDb(services);

                services.AddScoped<INotificationGateway, NotificationGateway>();
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var logger = scopedServices
                    .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                try
                {
                    new EfTestDbInitializer(dbContext).InitializeDb();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Проблема во время заполнения тестовой базы. " +
                                        "Ошибка: {Message}", ex.Message);
                }
            });
        }

        private void RemoveElementFromServices(IServiceCollection services, System.Type type)
        {
            ServiceDescriptor descriptor = services.SingleOrDefault(
                d => d.ServiceType == type);
            if (descriptor != null)
                services.Remove(descriptor);
        }

        private IMongoDatabase InitMongoDb(IServiceCollection services)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27018");
            var database = mongoClient.GetDatabase("TestDb");
            var customerCollection = database.GetCollection<Customer>("Customer");
            var preferenceCollection = database.GetCollection<Preference>("Preference");
            var promocodeCollection = database.GetCollection<PromoCode>("PromoCode");

            services.AddSingleton<IMongoClient>(_ => mongoClient);
            services.AddSingleton(serviceProvider => database);
            services.AddSingleton(serviceProvider => customerCollection);
            services.AddSingleton(serviceProvider => preferenceCollection);
            services.AddSingleton(serviceProvider => promocodeCollection);

            services.AddScoped(serviceProvider =>
                                serviceProvider.GetRequiredService<IMongoClient>()
                .StartSession());

            return database;
        }

    }
}