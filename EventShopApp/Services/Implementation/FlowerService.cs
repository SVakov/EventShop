using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
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

        public async Task<IEnumerable<Flower>> GetAllFlowers()
        {
            return await _context.Flowers.ToListAsync();
        }

        public async Task<IEnumerable<FlowerViewModel>> GetAllAvailableFlowersAsync()
        {
            return await _context.Flowers
                .Where(f => f.FlowerQuantity > 0 && f.IsAvailable)
                .Select(f => new FlowerViewModel
                {
                    Id = f.Id,
                    FlowerType = f.FlowerType,
                    Price = f.Price,
                    Description = f.Description.Length>20? f.Description.Substring(0,20)+"...":f.Description,
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

        public async Task<Flower?> GetFlowerById(int id)
        {
            return await _context.Flowers.FindAsync(id);
        }

        public async Task UpdateFlower(Flower flower)
        {
            _context.Flowers.Update(flower);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteFlower(int id)
        {
            var flower = await _context.Flowers.FindAsync(id);
            if (flower != null)
            {
                flower.IsAvailable = false;
                _context.Flowers.Update(flower);
                await _context.SaveChangesAsync();
            }
        }

        public async Task BringBackFlower(int id)
        {
            var flower = await _context.Flowers.FindAsync(id);
            if (flower != null)
            {
                flower.IsAvailable = true;
                _context.Flowers.Update(flower);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddFlower(Flower flower)
        {
            await _context.Flowers.AddAsync(flower);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeRole?> GetEmployeeRoleByEmail(string email)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
            return employee?.Role;
        }

    }
}
