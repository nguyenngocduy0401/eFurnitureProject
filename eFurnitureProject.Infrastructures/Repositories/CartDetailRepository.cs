using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class CartDetailRepository : ICartDetailRepository
    {
        private readonly AppDbContext _dbContext;
    }
}
