using eFurnitureProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Utils
{
    public class GetStatusOrder
    {
        public static string? GetStatusName(int status)
        {
            switch (status)
            {
                case (int)Order.Pending:
                    return "Pending";
                case (int)Order.Delivering:
                    return "Delivering";
                case (int)Order.Cancelled:
                    return "Cancelled";
                case (int)Order.Delivered:
                    return "Delivered";
                case (int)Order.Rejected:
                    return "Rejected";
                default:
                    return null;
            }
        }
    }
}
