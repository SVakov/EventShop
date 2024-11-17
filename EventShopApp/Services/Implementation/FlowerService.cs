using EventShopApp.Data;
using EventShopApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Services
{
    public class FlowerService : IFlowerService
    {
        private readonly ApplicationDbContext _context;

        public FlowerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlowerViewModel>> GetAllFlowerImagesAsync()
        {
            return await _context.Flowers
                .Where(f => f.FlowerQuantity > 0)
                .Select(f => new FlowerViewModel
                {
                    Id = f.Id,
                    FlowerType = f.FlowerType,
                    FlowerImageUrl = f.FlowerImageUrl
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<FlowerViewModel>> GetAllAvailableFlowersAsync()
        {
            return await _context.Flowers
                .Where(f => f.FlowerQuantity > 0)
                .Select(f => new FlowerViewModel
                {
                    Id = f.Id,
                    FlowerType = f.FlowerType,
                    Price = f.Price,
                    Description = f.Description,
                    FlowerImageUrl = f.FlowerImageUrl,
                    FlowerQuantity = f.FlowerQuantity
                })
                .ToListAsync();
        }

        public async Task<FlowerViewModel?> GetFlowerDetailsByIdAsync(int id)
        {
            return await _context.Flowers
                .Where(f => f.Id == id)
                .Select(f => new FlowerViewModel
                {
                    Id = f.Id,
                    FlowerType = f.FlowerType,
                    Price = f.Price,
                    Description = f.Description,
                    FlowerImageUrl = f.FlowerImageUrl,
                    FlowerQuantity = f.FlowerQuantity
                })
                .FirstOrDefaultAsync();
        }
    }
}
