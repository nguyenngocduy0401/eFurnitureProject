using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class ContractGetViewDTO
    {
        public Guid id { get; set; } 
        public string nameCustomer { get; set; }
        public string title { get; set; }
        public string? description { get; set; }
        public int value { get; set; }
        public int pay { get; set; }
        public int status { get; set; }
    }
}
