using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ProductViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<UserRegisterDTO, User>();
            CreateMap<ProductViewDTO, Product>();
        }
    }
}
