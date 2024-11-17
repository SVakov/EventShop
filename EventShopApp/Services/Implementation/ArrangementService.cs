using EventShopApp.Data;
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
    }
}
