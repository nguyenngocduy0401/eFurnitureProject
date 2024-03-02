using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.UserViewModels
{
    public class UserViewDTO : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime? DateOfBird { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public int? Wallet { get; set; }
        public bool Status { get; set; } = true;
    }
}
