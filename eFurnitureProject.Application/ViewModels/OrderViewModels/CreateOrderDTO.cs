using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace eFurnitureProject.Application.ViewModels.OrderViewModels
{
    public class CreateOrderDTO
    {
        public Guid UserId { get; set; }
        public Guid TransactionId { get; set; }
        //public Transaction Transaction { get; set; }
        //public List<Guid> ProductsId { get; set; }
        public List<OrderDetail>? orderDetails { get; set; }
    }
}
