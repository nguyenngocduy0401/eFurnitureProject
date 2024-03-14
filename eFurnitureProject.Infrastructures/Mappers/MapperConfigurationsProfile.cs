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
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;

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
                 .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));
            CreateMap<OrderViewDTO, Order>();
            CreateMap<Order, OrderViewDTO>();
            CreateMap<CreateImportDTO, Import>();
            CreateMap<Import, ImportViewDTO>()
                 .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));
            CreateMap<CreateImportDetailDTO, ImportDetail>();
            CreateMap<UpdateImportDTO, Import>();
            CreateMap<AddProductToCartDTO, CartDetail>();
            CreateMap<CartDetail, CartDetailViewDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString()))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : ""))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : ""))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0));
            CreateMap<StatusOrder, StatusDetailOrderViewDTO>();
            CreateMap<ImportDetail, ImportDetailViewDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : ""))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : ""));
            CreateMap<Cart, CartDetailViewDTO>();
            CreateMap<StatusOrder,StatusDetailOrderViewDTO>();
            CreateMap<Import, ImportViewFullDTO>()
                .ForMember(dest => dest.importDetailViewDTOs, opt => opt.MapFrom(src => src.ImportDetail));
            CreateMap<Feedback, FeedBackDTO>();
            CreateMap<FeedBackDTO, Feedback>();
            CreateMap<CreateFeedBackDTO, Feedback>();
            CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest => dest.VoucherId,opt => opt
            .MapFrom(src => string.IsNullOrEmpty(src.VoucherId) ? (Guid?)null : Guid.Parse(src.VoucherId)));

            CreateMap<Transaction, TransactionViewDTO>();
        }
    }
}
