using eFurnitureProject.Infrastructures.DataInitializer;

namespace eFurnitureProject.API.Services
{
    public class SetupIdentityDataSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public SetupIdentityDataSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<RoleInitializer>();

                await seeder.RoleInitializeAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    
    }
}
