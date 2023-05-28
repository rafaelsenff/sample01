using BaseMongoDb.Models;
using BaseMongoDb.Services;
using BaseMongoDb.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BaseMongoDb
{
    public static class Extensions
    {
        public static void UseMongoDatabase(this WebApplicationBuilder builder, string section)
        {
            builder.Services.Configure<MongoDbDatabaseSettings>(
            builder.Configuration.GetSection(section));
            builder.Services.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));

            /*
             *  appsettings.json da api deve conter as properties
             * 
                "MongoDbConfig": {
                  "ConnectionString": "mongodb://root:****:27017/?authSource=admin",
                  "DatabaseName": "DevLabsDB"
                }
            */
        }
    }
}
