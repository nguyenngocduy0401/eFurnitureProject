using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ProductDTO;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Application.ViewModels.VoucherDTO;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Application.ViewModels.OrderViewDTO;
using eFurnitureProject.Application.ViewModels.TransactionViewModels;
using eFurnitureProject.Application.ViewModels.OrderDetailViewModels;

namespace eFurnitureProject.Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<UserRegisterDTO, User>().ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.Password));
            CreateMap<ProductViewDTO, Product>();
            CreateMap<CreateVoucherDTO, Voucher>();
            CreateMap<VoucherViewDTO, Voucher>();
            CreateMap<Voucher, VoucherViewDTO>();
            CreateMap<Product,ProductDTO>().ReverseMap();
            CreateMap<ProductDTO,CreateProductDTO>().ReverseMap();
            CreateMap<Product,CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<Appointment, AppointmentDetail>();
            CreateMap<CreateContractDTO, Contract>();
            CreateMap<Contract, ContractViewDTO>()
                 .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
            CreateMap<UpdateContractDTO, Contract>();
            CreateMap<OrderViewDTO, Order>();
            CreateMap<Order, OrderViewDTO>();
            CreateMap <ProductViewDTO , Product>();
            CreateMap<Product,ProductViewDTO>();
            CreateMap<OrderDetail, OrderDetailViewDTO>();
            CreateMap<OrderDetailViewDTO, OrderDetail>();
            CreateMap<Transaction,TransactionViewDTO>();
            CreateMap<TransactionViewDTO, Transaction>();
        }
    }
}
