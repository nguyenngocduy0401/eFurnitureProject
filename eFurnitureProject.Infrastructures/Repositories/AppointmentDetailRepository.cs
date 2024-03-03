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
    public class AppointmentDetailRepository : IAppointmentDetailRepository
    {
        private readonly AppDbContext _dbContext;
        public AppointmentDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(AppointmentDetail appointmentDetail)
        {
            await _dbContext.AppointmentDetails.AddAsync(appointmentDetail);
        }
    }
}
