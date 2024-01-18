using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthAPI.Business.Modules.Roles.Interfaces;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolesB _rolesService;

    public RolesController(IRolesB rolesService)
    {
        _rolesService = rolesService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole(int userId, int roleId)
    {
        await _rolesService.AssignRoleAsync(userId, roleId);
        return Ok();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole(string roleName, string description)
    {
        await _rolesService.CreateRoleAsync(roleName, description);
        return Ok();
    }
}
