using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ResponseViewModel
{
    public  class ResponseDTO
    {
        public Guid FeedbackId { get; set; }
        public string StaffId { get; set; }
        public string Details { get; set; }
     
     }
}
