using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Application.ViewModels.OrderViewModels;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.AppointmentViewModel.AppointmentDetailViewModel;
using Microsoft.AspNetCore.Identity;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using eFurnitureProject.Application.ViewModels.CartViewModels;
using eFurnitureProject.Application.ViewModels.StatusOrderViewModels;

namespace eFurnitureProject.Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password));
            CreateMap<ProductViewDTO, Product>();
            CreateMap<CreateVoucherDTO, Voucher>();
            CreateMap<VoucherViewDTO, Voucher>();
            CreateMap<Voucher, VoucherViewDTO>();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductDTO,CreateProductDTO>().ReverseMap();
            CreateMap<Product,CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<Appointment, AppointmentDetail>();
            CreateMap<CreateContractDTO, Contract>();
            CreateMap<Contract, ContractViewDTO>()
                 .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
            CreateMap<UpdateContractDTO, Contract>();
            CreateMap<OrderViewGetDTO, Order>();
            CreateMap<Order, OrderViewGetDTO>();
            CreateMap<CreateAppointmentDTO, Appointment>();
            CreateMap<AppointmentDetailDTO, AppointmentDetail>();
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<AppointmentDetail, AppointmentDetailDTO>();
            CreateMap<AppointmentDetailDTO, AppointmentDetail>();
            CreateMap<AppointmentDetail, AppointmentDetailDTO>();
            CreateMap<AppointmentDetailDTO, AppointmentDetail>().ReverseMap();
            CreateMap<CreateAppointmentDTO, AppointmentDetail>();
            CreateMap<User, AppointmentDetail>();
            CreateMap<AppointmentDetailDTO, User>();
            CreateMap<ProductViewDTO , Product>();
            CreateMap<Product,ProductViewDTO>();
            CreateMap<AppointmentDetail,CreateAppointmentDetailDTO>();
            CreateMap<Pagination<ProductDTO>, IEnumerable<ProductDTO>>();
            CreateMap<User, UserDetailViewDTO>();
            CreateMap<User, UserViewDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<OrderDetail, OrderDetailViewDTO>();
            CreateMap<Order, OrderViewDTO>();
            CreateMap<Order, OrderViewForCustomerDTO>()
     .ForMember(dest => dest.StatusOrderViewDTO, opt => opt.MapFrom(src => new StatusOrderViewDTO
     {
         StatusCode = src.StatusOrder.StatusCode,
         Name = src.StatusOrder.Name
     }));
            CreateMap<CreateCategoryViewModel, Category>();
            CreateMap<Category, CategoryViewModel>()
                 .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
            CreateMap<OrderViewDTO, Order>();
            CreateMap<Order, OrderViewDTO>();
            CreateMap<CreateImportDTO, Import>();
            CreateMap<Import, ImportViewDTO>()
                 .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
            CreateMap<CreateImportDetailDTO, ImportDetail>();
            CreateMap<UpdateImportDTO, Import>();
            CreateMap<AddProductToCartDTO, CartDetail>();
            CreateMap<Cart, CartDetailViewDTO>();
        }
    }
}
