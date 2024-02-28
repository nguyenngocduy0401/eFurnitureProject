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
            IClaimsService claimsService,UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _dbContext = context;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _userManager = userManager;
            _roleManager = roleManager;
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
