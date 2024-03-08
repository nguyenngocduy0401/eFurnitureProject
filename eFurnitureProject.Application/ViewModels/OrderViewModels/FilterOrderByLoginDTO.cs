using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.OrderViewModels
{
    public class FilterOrderByLoginDTO
    {
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int? StatusCode{ get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
