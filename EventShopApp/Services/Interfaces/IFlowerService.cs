using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.ViewModels;

namespace EventShopApp.Services
{
    public interface IFlowerService
    {
        Task<IEnumerable<FlowerViewModel>> GetAllFlowerImagesAsync();
        Task<IEnumerable<FlowerViewModel>> GetAllAvailableFlowersAsync();
        Task<IEnumerable<Flower>> GetAllFlowers();
        Task<FlowerViewModel?> GetFlowerDetailsByIdAsync(int id);

        Task<Flower?> GetFlowerById(int id);
        Task UpdateFlower(Flower flower); // Update a flower
        Task SoftDeleteFlower(int id); // Soft-delete a flower
        Task BringBackFlower(int id); // Bring back a soft-deleted flower
        Task AddFlower(Flower flower); // Add a new flower
        Task<EmployeeRole?> GetEmployeeRoleByEmail(string email);
    }
}
