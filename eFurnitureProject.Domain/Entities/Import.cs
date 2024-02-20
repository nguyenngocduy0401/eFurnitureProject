using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Domain.Entities
{
    public class Import : BaseEntity
    {
        public double Price { get; set; }
        public int Status { get; set; }

        public virtual ICollection<ImportDetail>? ImportDetail { get; set; }
    }
}
