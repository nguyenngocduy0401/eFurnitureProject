using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public class IClaimsService
    {
        public Guid GetCurrentUserId { get; }
    }
}
