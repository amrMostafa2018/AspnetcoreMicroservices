using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

using System;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrteDateBase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var configuration = service.GetRequiredService<IConfiguration>();
                var logger = service.GetRequiredService<ILogger<TContext>>();

                try
                {
                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection,
                    };
                    command.CommandText = "Drop table If EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"Create Table Coupon (Id SERIAL PRIMARY KEY,
                                                                 ProductName VARCHAR(24) NOT NULL,
                                                                 DESCRIPTION TEXT,
                                                                 Amount INT)";
                    command.ExecuteNonQuery();

                }
                catch(Exception ex)
                {
                    throw;
                }
                return host;
            }
        }
    }
}
