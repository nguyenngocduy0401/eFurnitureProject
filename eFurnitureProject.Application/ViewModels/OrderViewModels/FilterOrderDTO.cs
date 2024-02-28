using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewDTO
{
    public class FilterOrderDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? TransactionId { get; set; }
        public string? UserId { get; set; }
    }
}
