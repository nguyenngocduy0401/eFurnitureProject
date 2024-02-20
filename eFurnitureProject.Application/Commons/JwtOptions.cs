using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Commons
{
    public class JwtOptions
    {
        public String Issuer { get; set; } = string.Empty;
        public String Audience { get; set; } = string.Empty;
        public String Secret { get; set; } = string.Empty;
    }
}
