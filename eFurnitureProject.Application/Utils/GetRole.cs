using eFurnitureProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Utils
{
    public static class GetRole
    {
        public static string? GetRoleName(int? role)
        {
            switch (role)
            {
                case (int)Roles.Administrator:
                    return "Administrator";
                case (int)Roles.Customer:
                    return "Customer";
                case (int)Roles.Staff:
                    return "Staff";
                case (int)Roles.DeliveryStaff:
                    return "DeliveryStaff";
                default:
                    return null;
            }
        }
    }
}
