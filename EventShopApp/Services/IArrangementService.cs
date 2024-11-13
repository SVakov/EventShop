using EventShopApp.Models;

namespace EventShopApp.Services
{
    public interface IArrangementService
    {
        Task<IEnumerable<ArrangementItem>> GetAllArrangementImagesAsync();
    }
}
