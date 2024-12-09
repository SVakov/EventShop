using EventShopApp.Areas.Management.Controllers;
using EventShopApp.Areas.Management.ViewModels;
using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class EmployeesControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
       
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
        _context = new ApplicationDbContext(options);

        
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _userManager = new UserManager<IdentityUser>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
        _roleManager = new RoleManager<IdentityRole>(
            roleStoreMock.Object, null, null, null, null
        );

        _controller = new EmployeesController(_context, _userManager, _roleManager);
    }

    [Fact]
    public void Index_ReturnsViewWithEmployees()
    {
        // Arrange
        _context.Employees.AddRange(new List<Employee>
        {
            new Employee { Id = 1, Name = "John", Email = "john@example.com", PhoneNumber = "+359897598648", HireDate = DateTime.UtcNow, Role = EmployeeRole.Manager },
            new Employee { Id = 2, Name = "Jane", Email = "jane@example.com", PhoneNumber = "+359897598458", HireDate = DateTime.UtcNow, Role = EmployeeRole.Seller }
        });
        _context.SaveChanges();

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void Add_Get_ReturnsView()
    {
        // Act
        var result = _controller.Add();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.Model);
    }

    [Fact]
    public async Task Add_Post_InvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var employee = new Employee { Name = "", Email = "john@example.com", PhoneNumber = "+359897598648", HireDate = DateTime.UtcNow, Role = EmployeeRole.Manager };
        _controller.ModelState.AddModelError("Name", "The Name field is required.");

        // Act
        var result = await _controller.Add(employee, "TemporaryPassword123!");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Employee>(viewResult.Model);
        Assert.Equal(employee, model);
    }
}
