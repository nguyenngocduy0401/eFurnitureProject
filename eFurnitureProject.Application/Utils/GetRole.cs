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
                case (int)RoleEnum.Administrator:
                    return "Administrator";
                case (int)RoleEnum.Customer:
                    return "Customer";
                case (int)RoleEnum.Staff:
                    return "Staff";
                case (int)RoleEnum.DeliveryStaff:
                    return "DeliveryStaff";
                default:
                    return null;
            }
        }
    }
}
