using EventShopApp.Data;
using EventShopApp.Models;
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

        public async Task<IEnumerable<ArrangementItem>> GetAllArrangementImagesAsync()
        {
            return await _context.ArrangementItems
                .Where(a => a.ArrangementItemsQuantity>0)
                .Select(a => new ArrangementItem
                {
                    Id = a.Id,
                    ArrangementItemType = a.ArrangementItemType,
                    ArrangementItemImageUrl = a.ArrangementItemImageUrl
                })
                .ToListAsync();
            }
    }
}
