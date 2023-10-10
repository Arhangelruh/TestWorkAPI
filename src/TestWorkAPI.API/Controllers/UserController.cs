using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TestWorkAPI.API.Interfaces;
using TestWorkAPI.API.Models;
using TestWorkAPI.API.Requests;

namespace TestWorkAPI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public UserController(
            IUserManager userManager,
            IRoleManager roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = new List<UserViewModel>();
            var alluser = await _userManager.GetAllUsersAsync();

            foreach (var user in alluser)
            {
                var roles = await _roleManager.GetUserRolesAsync(user.Id);

                var tempUser = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    Roles = roles
                };
                users.Add(tempUser);
            }

            return Ok(users);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var userRequest = await _userManager.GetUserByIdAsync(id);
                if (userRequest != null)
                {
                    var userRoles = await _roleManager.GetUserRolesAsync(userRequest.Id);

                    var user = new UserViewModel
                    {
                        Id = userRequest.Id,
                        Name = userRequest.Name,
                        Age = userRequest.Age,
                        Email = userRequest.Email,
                        Roles = userRoles
                    };
                    return Ok(user);
                }
            }catch (KeyNotFoundException ex)
            {                
                return Ok(ex.Message);
            }
            return BadRequest();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserRequest request)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            if (ModelState.IsValid)
            {
                var checkEmail = await _userManager.CheckEmailAsync(request.Email);
                if (checkEmail)
                {
                    return BadRequest($"email {request.Email} exists in DB");
                }

                await _userManager.CreateUserAsync(request);
                return Ok();
            }

            return BadRequest();
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserRequest request)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            if (ModelState.IsValid)
            {
                var user = new UserViewModel
                {
                    Id = id,
                    Name = request.Name,
                    Age = request.Age,
                    Email = request.Email
                };
                try
                {
                    await _userManager.UpdateUserAsync(user);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return Conflict();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _userManager.DeleteUserAsync(id);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddUserRoleAsync(int userid, int roleid)
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(userid);
                var role = await _roleManager.GetRoleByIdAsync(roleid);
            }
            catch (KeyNotFoundException e)
            {
                return Ok(e.Message);
            }

            await _userManager.AddUserRole(userid, roleid);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("deleterole")]
        public async Task<IActionResult> DeleteUserRoleAsync(int userid, int roleid)
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(userid);
                await _userManager.DeleteUserRole(userid, roleid);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Ok(e.Message);
            }
        }
    }
}
