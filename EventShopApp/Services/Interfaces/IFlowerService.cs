using EventShopApp.Models;
using EventShopApp.ViewModels;

namespace EventShopApp.Services
{
    public interface IFlowerService
    {
        Task<IEnumerable<FlowerViewModel>> GetAllFlowerImagesAsync();
        Task<IEnumerable<FlowerViewModel>> GetAllAvailableFlowersAsync();
        Task<FlowerViewModel?> GetFlowerDetailsByIdAsync(int id);
    }
}
