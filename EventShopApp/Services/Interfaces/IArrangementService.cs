using EventShopApp.Models;
using EventShopApp.ViewModels;

namespace EventShopApp.Services
{
    public interface IArrangementService
    {
        Task<IEnumerable<ArrangementViewModel>> GetAllArrangementImagesAsync();
        Task<IEnumerable<ArrangementViewModel>> GetAllAvailableArrangementsAsync();
        Task<ArrangementViewModel?> GetArrangementDetailsByIdAsync(int id);

        Task<IEnumerable<ArrangementItem>> GetAllArrangements();
        Task<ArrangementItem?> GetArrangementById(int id);
        Task AddArrangement(ArrangementItem arrangement);
        Task UpdateArrangement(ArrangementItem arrangement);
        Task SoftDeleteArrangement(int id);
        Task BringBackArrangement(int id);
    }
}
