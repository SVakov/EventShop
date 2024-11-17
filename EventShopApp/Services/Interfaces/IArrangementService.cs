using EventShopApp.ViewModels;

namespace EventShopApp.Services
{
    public interface IArrangementService
    {
        Task<IEnumerable<ArrangementViewModel>> GetAllArrangementImagesAsync();
        Task<IEnumerable<ArrangementViewModel>> GetAllAvailableArrangementsAsync();
        Task<ArrangementViewModel?> GetArrangementDetailsByIdAsync(int id);
    }
}
