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
        private readonly RoleManager<Role> _roleManager;
        public UserRepository(AppDbContext context, ICurrentTime timeService,
            IClaimsService claimsService)
        {
            _dbContext = context;
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

        public async Task<User> GetUserByUserNameAndPasswordHash(string username, string passwordHash)
        {
            var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            bool invalid = await _userManager.CheckPasswordAsync(user, user.PasswordHash);
            if (user is null && invalid is false) 
            {
                throw new Exception("Username or password is not correct!");
            }
            return user;
        }
    }
}
