using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.FeedbackDTO
{
    public class FeedBackDTO
    {
        public string? UserId { get; set; }
       
        public string? Details { get; set; }
        public string Title { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

   
        public DateTime? DeletionDate { get; set; }

        public Guid? DeleteBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}

