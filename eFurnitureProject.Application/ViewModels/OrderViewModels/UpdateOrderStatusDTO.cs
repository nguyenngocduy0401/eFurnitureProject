using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewDTO
{
    public class UpdateOrderStatusDTO
    {
        public Guid Id { get; set; }
        //public Guid StatusId { get; set; }
        public int StatusCode { get; set; }
    }
}
