using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
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

        public UserRepository(AppDbContext context, ICurrentTime timeService,
            IClaimsService claimsService)
        {
            _dbContext = context;
        }

        public Task<User> AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckEmailNameExisted(string emailName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPhoneNumberExisted(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckUserNameExisted(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUserNameAndPasswordHash(string username, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
