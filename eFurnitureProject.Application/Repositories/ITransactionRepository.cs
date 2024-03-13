using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<Pagination<Transaction>> FilterTransactionByLogin(string userId, DateTime? fromTime, DateTime? toTime, int pageIndex, int pageSize);
        Task<Pagination<Transaction>> FilterTransaction(DateTime? fromTime, DateTime? toTime, int pageIndex, int pageSize);
    }//11
}
