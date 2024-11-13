using EventShopApp.Data;
using EventShopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Services
{
    public class FlowerService:IFlowerService
    {
        private readonly ApplicationDbContext _context;

        public FlowerService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Flower>> GetAllFlowerImagesAsync() 
        {
            return await _context.Flowers
                .Where(f => f.FlowerQuantity>0)
                .Select(f => new Flower
                {
                    Id = f.Id,
                    FlowerType = f.FlowerType,
                    FlowerImageUrl = f.FlowerImageUrl
                })
                .ToListAsync();
        }
    }
}
