using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ICurrentTime _currentTime;
        private readonly RoleManager<Role> _roleManager;
        private readonly IClaimsService _claimsService;

        public UserRepository(AppDbContext context, ICurrentTime currentTime,
            IClaimsService claimsService, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _dbContext = context;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<Pagination<User>> GetProductsByFilter
            (string search, string role, DateTime setLockoutEndDate, int pageIndex = 1, int pageSize = 10) 
        {
            var userList = _dbContext.Users
            .Where(u => (string.IsNullOrEmpty(search) ||
                   u.Name.Contains(search) ||
                   u.PhoneNumber.Contains(search) ||
                   u.Email.Contains(search)) && _roleManager.Roles.Any(r => r.Name == role) &&
                  (u.LockoutEnd == null || u.LockoutEnd <= setLockoutEndDate));

            var itemCount = await userList.CountAsync();
            var items = await userList
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

            var result = new Pagination<User>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result; 
        }
        
        public async Task AddAsync(User user)
        {
           await _dbContext.AddAsync(user);
        }
        public async Task<bool> CheckEmailNameExisted(string emailName) => 
            await _dbContext.Users.AnyAsync(u => u.Email == emailName);

        public async Task<bool> CheckPhoneNumberExisted(string phoneNumber) => 
            await _dbContext.Users.AnyAsync(u =>u.PhoneNumber == phoneNumber);

        public async Task<bool> CheckUserNameExisted(string userName) => 
            await _dbContext.Users.AnyAsync(u => u.UserName == userName);

        public async Task<User> GetUserByUserNameAndPassword(string username, string password)
        {
            var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            bool invalid = await _userManager.CheckPasswordAsync(user, password);
            if (user is null && invalid is false) 
            {
                throw new Exception("Username or password is not correct!");
            }
            return user;
        }
    }
}
