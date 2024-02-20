using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Commons
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
        public string Error { get; set; } = null;
        public List<string> ErrorMessages { get; set; } = null;

    }
}
