using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Repositories
{
    public class VoucherDetailRepository:IVoucherDetailRepository

    {
        private readonly AppDbContext _dbContext;
        public VoucherDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(VoucherDetail voucherDetail)
        {
            await _dbContext.VouchersDetails.AddAsync(voucherDetail);
        }
    }
}//
