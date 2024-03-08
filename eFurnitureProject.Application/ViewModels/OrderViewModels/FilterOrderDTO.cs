using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewModels
{
    public class FilterOrderDTO
    {
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Status { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
