using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.AppointmentDTO;
using eFurnitureProject.Application.ViewModels.ProductDTO;
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
            CreateMap<Product,ProductDTO>().ReverseMap();
            CreateMap<ProductDTO,CreateProductDTO>().ReverseMap();
            CreateMap<Product,CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<CreateAppointmentDTO,Appointment>().ReverseMap();
            CreateMap<Appointment,AppointmentDTO>().ReverseMap();
            CreateMap<AddStaffDTO, Appointment>();
            CreateMap<AddStaffDTO, AppointmentDetail>();
            CreateMap<Appointment, AppointmentDetail>();
        }
    }
}
