using EventShopApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class CartCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CartCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();

                // Call the cleanup logic with the desired timeout
                cartService.CleanupStaleCartItems(TimeSpan.FromMinutes(20));
            }

            // Wait for 5 minutes before running again
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
