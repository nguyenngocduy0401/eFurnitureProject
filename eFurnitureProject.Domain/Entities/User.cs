using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime? DateOfBird { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public int? Wallet { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<AppointmentDetail>? AppointmentDetail { get; set; }
        public virtual ICollection<VoucherDetail>? VoucherDetail { get; set; }
    }
}
