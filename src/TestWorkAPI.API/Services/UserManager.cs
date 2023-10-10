using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using TestWorkAPI.API.Interfaces;
using TestWorkAPI.API.Models;
using TestWorkAPI.API.Requests;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.API.Services
{
    public class UserManager : IUserManager
    {
        private readonly IRepository<User> _repositoryUsers;
        private readonly IRepository<UserRole> _repositoryUsersRole;

        public UserManager(IRepository<User> repositoryUsers, IRepository<UserRole> repositoryUsersRole)
        {
            _repositoryUsers = repositoryUsers ?? throw new ArgumentNullException(nameof(repositoryUsers));
            _repositoryUsersRole = repositoryUsersRole ?? throw new ArgumentNullException(nameof(repositoryUsersRole));
        }

        public async Task AddUserRole(int userid, int roleid)
        {
            var userRole = await _repositoryUsersRole.GetEntityAsync(userrole =>
                    userrole.UserId == userid && userrole.RoleId == roleid);

            if (userRole is null)
            {
                var newUserRole = new UserRole
                {
                    UserId = userid,
                    RoleId = roleid
                };
                await _repositoryUsersRole.AddAsync(newUserRole);
                await _repositoryUsersRole.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _repositoryUsers.GetEntityWithoutTrackingAsync(q => q.Email.Equals(email));
            if (user is null)
            {
                return false;
            }
            return true;
        }

        public async Task CreateUserAsync(UserRequest model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                Age = model.Age
            };

            await _repositoryUsers.AddAsync(newUser);
            await _repositoryUsers.SaveChangesAsync();

        }

        public async Task DeleteUserAsync(int userid)
        {
            var user = await _repositoryUsers
                .GetEntityAsync(user =>
                    user.Id == userid);

            if (user != null)
            {
                var userRoles = await _repositoryUsersRole
                .GetAll()
                .AsNoTracking()
                .Where(userkey => userkey.UserId == userid)
                .ToListAsync();

                if (userRoles.Any())
                {
                    foreach (var userRole in userRoles)
                    {
                        _repositoryUsersRole.Delete(userRole);
                        await _repositoryUsersRole.SaveChangesAsync();
                    }
                }

                _repositoryUsers.Delete(user);
                await _repositoryUsers.SaveChangesAsync();
            }
        }

        public async Task DeleteUserRole(int userid, int roleid)
        {
            var userRole = await _repositoryUsersRole
                .GetEntityAsync(userrole =>
                    userrole.UserId == userid && userrole.RoleId == roleid);

            if (userRole is null)
            {
                throw new KeyNotFoundException("User role not found");
            }

            _repositoryUsersRole.Delete(userRole);
            await _repositoryUsersRole.SaveChangesAsync();
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync(ListParameters listParameters)
        {
            var request = _repositoryUsers.GetAll();

            if (listParameters.searchTeam != null)
            {
                request = request.Where(
                u => u.Name.Contains(listParameters.searchTeam) ||
                u.Age.ToString().Contains(listParameters.searchTeam) ||
                u.Email.Contains(listParameters.searchTeam)
                );
            }

            if (listParameters.sortOrder?.ToLower() == "desc")
            {
                request = request.OrderByDescending(GetSortProperty(listParameters));
            }
            else
            {
                request = request.OrderBy(GetSortProperty(listParameters));
            }

            var usersList = new List<UserViewModel>();
            var users = await request
                .Skip((listParameters.PageNumber - 1) * listParameters.PageSize)
                .Take(listParameters.PageSize)
                .AsNoTracking()
                .ToListAsync();

            foreach (var user in users)
            {
                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Age = user.Age,
                    Email = user.Email
                };
                usersList.Add(userModel);
            }
            return usersList;
        }

        private static Expression<Func<User, string>> GetSortProperty(ListParameters listParameters)=>
        listParameters.sortColumn?.ToLower() switch
            {
                "name"=> user => user.Name,
                "age" => user => user.Age.ToString(),
                "email" => user => user.Email,
                _ => user => user.Id.ToString()
            };


        public async Task<UserViewModel> GetUserByIdAsync(int userid)
        {
            var user = await _repositoryUsers
                .GetEntityAsync(user =>
                    user.Id == userid);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var userModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Age = user.Age,
                    Email = user.Email
                };
                return userModel;
        }

        public async Task UpdateUserAsync(UserViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var editUser = await _repositoryUsers.GetEntityAsync(user =>
                    user.Id == model.Id);

            if (model.Email != editUser.Email)
            {
                var checkEmail = await _repositoryUsers.GetAll()
                 .AsNoTracking()
                 .Where(userEmail => userEmail.Email == model.Email)
                 .ToListAsync();
                if (checkEmail.Any())
                {
                    throw new Exception($"Email {model.Email} already exists.");
                }
            }

            editUser.Name = model.Name;
            editUser.Age = model.Age;
            editUser.Email = model.Email;

            _repositoryUsers.Update(editUser);
            await _repositoryUsers.SaveChangesAsync();
        }
    }
}
