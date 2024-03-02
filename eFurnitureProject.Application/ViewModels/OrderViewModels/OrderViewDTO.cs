using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewDTO
{
    public class OrderViewDTO: BaseEntity
    {
        public DateTime DateTime { get; set; }
        public int Price { get; set; }
        public StatusOrder? StatusOrder { get; set; }
        public Transaction? Transaction { get; set; }
        public User? User { get; set; }
    }
}
