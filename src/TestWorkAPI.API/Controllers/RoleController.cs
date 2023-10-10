using Microsoft.AspNetCore.Mvc;
using TestWorkAPI.API.Interfaces;
using TestWorkAPI.API.Models;
using TestWorkAPI.API.Requests;

namespace TestWorkAPI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleManager _roleManager;

        public RoleController(IRoleManager roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {            
            return Ok(await _roleManager.GetAllRolesAsync());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                return Ok(await _roleManager.GetRoleByIdAsync(id));
            }
            catch(KeyNotFoundException ex)
            {
                return Ok(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoleRequest request)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            if (ModelState.IsValid)
            {               
                await _roleManager.CreateRoleAsync(request);
                return Ok();
            }

            return BadRequest();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RoleRequest request)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            if (ModelState.IsValid)
            {
                var role = new RoleViewModel
                {
                   Id = id,
                   RoleName= request.RoleName
                };
                    await _roleManager.UpdateRoleAsync(role);
                    return Ok();
            }
            return Conflict();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _roleManager.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}
