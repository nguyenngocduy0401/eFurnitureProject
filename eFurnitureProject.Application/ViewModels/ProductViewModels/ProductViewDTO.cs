﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.ProductViewModels
{
    public class ProductViewDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int InventoryQuantity { get; set; }
        public int Status { get; set; }
        public Guid? CategoryId { get; set; }
    }
}