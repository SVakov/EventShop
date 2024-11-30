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
        Task UpdateFlower(Flower flower); 
        Task SoftDeleteFlower(int id); 
        Task BringBackFlower(int id); 
        Task AddFlower(Flower flower); 
        Task<EmployeeRole?> GetEmployeeRoleByEmail(string email);
    }
}
