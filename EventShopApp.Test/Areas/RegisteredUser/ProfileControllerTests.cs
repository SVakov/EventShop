using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.Areas.RegisteredUser.Controllers;
using EventShopApp.Areas.RegisteredUser.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;

public class ProfileControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ProfileController _controller;

    public ProfileControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _userManager = new UserManager<IdentityUser>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        _controller = new ProfileController(_userManager, _context);
    }

    [Fact]
    public async Task Index_ReturnsRedirectToAction_IfUserNotFound()
    {
        // Arrange
        
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity());
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = userClaims };

        // Act
        var result = await _controller.Index();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName); 
        Assert.Equal("Account", redirectResult.ControllerName);
        Assert.Equal("Identity", redirectResult.RouteValues["area"]);
    }
}
