using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ProductService
{
    public static partial class ServiceCollectionExtentions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbOptions>(config.GetSection("MongoDb"));

            services.AddSingleton(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbOptions>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);

                return database;
            });

            return services;
        }
    }
}
