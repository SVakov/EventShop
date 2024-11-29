using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Services
{
    public class ArrangementService : IArrangementService
    {
        private readonly ApplicationDbContext _context;

        public ArrangementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArrangementViewModel>> GetAllArrangementImagesAsync()
        {
            return await _context.ArrangementItems
                .Where(a => a.ArrangementItemsQuantity > 0)
                .Select(a => new ArrangementViewModel
                {
                    Id = a.Id,
                    ArrangementItemType = a.ArrangementItemType,
                    ArrangementItemImageUrl = a.ArrangementItemImageUrl
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ArrangementViewModel>> GetAllAvailableArrangementsAsync()
        {
            return await _context.ArrangementItems
                .Where(a => a.ArrangementItemsQuantity > 0)
                .Select(a => new ArrangementViewModel
                {
                    Id = a.Id,
                    ArrangementItemType = a.ArrangementItemType,
                    Price = a.Price,
                    Description = a.Description,
                    ArrangementItemImageUrl = a.ArrangementItemImageUrl,
                    ArrangementItemsQuantity = a.ArrangementItemsQuantity
                })
                .ToListAsync();
        }

        public async Task<ArrangementViewModel?> GetArrangementDetailsByIdAsync(int id)
        {
            return await _context.ArrangementItems
                .Where(a => a.Id == id)
                .Select(a => new ArrangementViewModel
                {
                    Id = a.Id,
                    ArrangementItemType = a.ArrangementItemType,
                    Price = a.Price,
                    Description = a.Description,
                    ArrangementItemImageUrl = a.ArrangementItemImageUrl,
                    ArrangementItemsQuantity = a.ArrangementItemsQuantity
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ArrangementItem>> GetAllArrangements()
        {
            return await _context.ArrangementItems.ToListAsync();
        }

        public async Task<ArrangementItem?> GetArrangementById(int id)
        {
            return await _context.ArrangementItems.FindAsync(id);
        }

        public async Task AddArrangement(ArrangementItem arrangement)
        {
            await _context.ArrangementItems.AddAsync(arrangement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArrangement(ArrangementItem arrangement)
        {
            _context.ArrangementItems.Update(arrangement);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteArrangement(int id)
        {
            var arrangement = await _context.ArrangementItems.FindAsync(id);
            if (arrangement != null)
            {
                arrangement.IsAvailable = false;
                _context.ArrangementItems.Update(arrangement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task BringBackArrangement(int id)
        {
            var arrangement = await _context.ArrangementItems.FindAsync(id);
            if (arrangement != null)
            {
                arrangement.IsAvailable = true;
                _context.ArrangementItems.Update(arrangement);
                await _context.SaveChangesAsync();
            }
        }
    }
}
