using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ContractViewModels
{
    public class ContractViewModel
    {
        public Guid _Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
