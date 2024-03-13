using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public int BalanceRemain { get; set; }
        public string? Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime? DateTime { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
        public string TypeOrder { get; set; }
        public Guid? OrderId{ get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        public Guid? OrderProcessingId { get; set; }
        [ForeignKey("OrderProcessingId")]
        public OrderProcessing? OrderProcessing { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
