using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class ManagementAccessHandler : AuthorizationHandler<ManagementAccessRequirement>
{
    private readonly ApplicationDbContext _context;

    public ManagementAccessHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ManagementAccessRequirement requirement)
    {
        var email = context.User.Identity?.Name;

        if (email == null)
        {
            return;
        }

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

        if (employee != null && employee.Role == EmployeeRole.Owner)
        {
            context.Succeed(requirement);
        }
    }
}
