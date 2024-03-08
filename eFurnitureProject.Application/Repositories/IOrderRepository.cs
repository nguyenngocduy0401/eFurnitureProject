using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> Get(int pageIndex, int pageSize);
        Task<IEnumerable<Order>> GetOrderByFilter(int pageIndex, int pageSize, FilterOrderDTO filter);
    }
}
