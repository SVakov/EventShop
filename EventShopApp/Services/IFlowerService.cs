using EventShopApp.Models;

namespace EventShopApp.Services
{
    public interface IFlowerService
    {
        Task<IEnumerable<Flower>> GetAllFlowerImagesAsync();
    }
}
