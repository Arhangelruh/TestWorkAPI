using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using TestWorkAPI.API.Helpers;
using TestWorkAPI.API.Interfaces;
using TestWorkAPI.API.Models;
using TestWorkAPI.API.Requests;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.API.Services
{
    /// <inheritdoc cref="IRoleManager"/>
    public class RoleManager : IRoleManager
    {
        private readonly IRepository<Role> _repositoryRoles;
        private readonly IRepository<UserRole> _repositoryUsersRole;

        public RoleManager(IRepository<Role> repositoryRoles, IRepository<UserRole> repositoryUsersRole)
        {
            _repositoryRoles = repositoryRoles ?? throw new ArgumentNullException(nameof(repositoryRoles));
            _repositoryUsersRole = repositoryUsersRole ?? throw new ArgumentNullException(nameof(repositoryUsersRole));
        }

        public async Task CreateRoleAsync(RoleRequest model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var newRole = new Role
            {
                RoleName = model.RoleName
            };

            await _repositoryRoles.AddAsync(newRole);
            await _repositoryRoles.SaveChangesAsync();

        }

        public async Task DeleteRoleAsync(int roleid)
        {
            var role = await _repositoryRoles
     .GetEntityAsync(role =>
         role.Id == roleid);

            if (role != null)
            {
                var userRoles = await _repositoryUsersRole
                .GetAll()
                .AsNoTracking()
                .Where(rolekey => rolekey.RoleId == role.Id)
                .ToListAsync();

                if (userRoles.Any())
                {
                    foreach (var userRole in userRoles)
                    {
                        _repositoryUsersRole.Delete(userRole);
                        await _repositoryUsersRole.SaveChangesAsync();
                    }
                }

                _repositoryRoles.Delete(role);
                await _repositoryRoles.SaveChangesAsync();
            }
        }

        public async Task<PageList<Role>> GetAllRolesAsync(ListParameters listParameters)
        {
            var request = _repositoryRoles.GetAll();

            if (listParameters.searchTeam != null)
            {
                request = request.Where(u => u.RoleName.Contains(listParameters.searchTeam));
            }

            if (listParameters.sortOrder?.ToLower() == "desc")
            {
                request = request.OrderByDescending(GetSortProperty(listParameters));
            }
            else
            {
                request = request.OrderBy(GetSortProperty(listParameters));
            }

            var roles = await PageList<Role>.CreatePageAsync(request, listParameters.PageNumber, listParameters.PageSize);
            return roles;
        }

        private static Expression<Func<Role, object>> GetSortProperty(ListParameters listParameters) =>
              listParameters.sortColumn?.ToLower() switch
                  {
                     "rolename" => role => role.RoleName,
                      _ => role => role.Id
                  };

        public async Task<RoleViewModel> GetRoleByIdAsync(int roleid)
        {
            var role = await _repositoryRoles
                .GetEntityAsync(role =>
                    role.Id == roleid);

            if (role is null)
            {
                throw new KeyNotFoundException("User role not found");
            }

            var roleModel = new RoleViewModel
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
            return roleModel;
        }

        public async Task<List<RoleViewModel>> GetUserRolesAsync(int userid)
        {
            var roleList = new List<RoleViewModel>();

            var userRoles = await _repositoryUsersRole
                .GetAll()
                .AsNoTracking()
                .Where(userrole => userrole.UserId == userid)
                .ToListAsync();

            if (userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    var request = await _repositoryRoles.GetEntityWithoutTrackingAsync(rolerequest => rolerequest.Id.Equals(role.RoleId));
                    var roleToList = new RoleViewModel
                    {
                        Id = request.Id,
                        RoleName = request.RoleName
                    };
                    roleList.Add(roleToList);
                }
            }

            return roleList;
        }

        public async Task UpdateRoleAsync(RoleViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var editRole = await _repositoryRoles.GetEntityAsync(role =>
                    role.Id == model.Id);

            editRole.RoleName = model.RoleName;

            _repositoryRoles.Update(editRole);
            await _repositoryRoles.SaveChangesAsync();
        }
    }
}
