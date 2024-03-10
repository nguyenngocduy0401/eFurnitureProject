using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.CartViewModels
{
    public class CartDetailViewDTO
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
    }
}
