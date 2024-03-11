using Azure;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Response = eFurnitureProject.Domain.Entities.Response;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class ResponseRepository : GenericRepository<Response>, IResponseRepository
    {
        private readonly AppDbContext _dbContext;
       
        public ResponseRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService,claimsService)
        {
            _dbContext = context;
            
        }
    }
}
