using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Voucher : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Percent { get; set; }
        public int Number { get; set; }

        public virtual ICollection<VoucherDetail>? VoucherDetail { get; set; }
    }
}
