﻿using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IClaimsService
    {
        public Guid GetCurrentUserId { get; }
    }
}
