﻿using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <param name="listParameters"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] ListParameters listParameters)
        {            
            return Ok(await _roleManager.GetAllRolesAsync(listParameters));        }

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

        /// <summary>
        /// Create role.
        /// </summary>
        /// <param name="request">Role model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Update role.
        /// </summary>
        /// <param name="id">Role Id.</param>
        /// <param name="request">New role model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Delete role by id.
        /// </summary>
        /// <param name="id">Role id.</param>
        /// <returns></returns>
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
