﻿using eFurnitureProject.Infrastructures.DataInitializer;

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
                var statusOrderProcessingInitialize = scope.ServiceProvider.GetRequiredService<StatusOrderProcessingInitializer>();
                var statusOrderInitializer = scope.ServiceProvider.GetRequiredService<StatusOrderInitializer>();
                var seeder = scope.ServiceProvider.GetRequiredService<RoleInitializer>();
                await seeder.RoleInitializeAsync();
                await statusOrderInitializer.StatusOrderInitializerAsync();
                await statusOrderProcessingInitialize.StatusOrderProcessingInitializerAsync();

                var accountInitializer = scope.ServiceProvider.GetRequiredService<AccountInitializer>();
                await accountInitializer.AccountInitializeAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    
    }
}
