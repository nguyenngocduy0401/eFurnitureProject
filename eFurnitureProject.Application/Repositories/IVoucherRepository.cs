using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Repositories
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<IEnumerable<Voucher>> Get(int pageIndex, int pageSize);
        Task<Pagination<Voucher>> GetVoucherByDateAsync(int pageIndex, int pageSize, DateTime date);
    }
}
