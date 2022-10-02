using Microsoft.EntityFrameworkCore;
using VideoJatekBackend.Dal.Seed;

namespace VideoJatekBackend.Extensions
{
    public static class HostDataExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                PublisherJsonFileProcessor.Deserialize(serviceProvider);
                VideogameJsonFileProcessor.Deserialize(serviceProvider);
            }

            return host;
        }
    }
}
