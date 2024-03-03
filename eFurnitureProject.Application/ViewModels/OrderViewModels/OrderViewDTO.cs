using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewDTO
{
    public class OrderViewDTO
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Price { get; set; }
        public int StatusCode { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int Paid { get; set; }
    }
}
