using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;

namespace eFurnitureProject.Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<UserRegisterDTO, User>();
            CreateMap<CreateContractViewModel, Contract>();
            CreateMap<Contract, ContractViewModel>()
                 .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
            CreateMap<UpdateContractDTO, Contract>();
            CreateMap<CreateCategoryViewModel, Category>();
            CreateMap<Category, CategoryViewModel>()
                 .ForMember(dest => dest._Id, src => src.MapFrom(x => x.Id));
        }
    }
}
