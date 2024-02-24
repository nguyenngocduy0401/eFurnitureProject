﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eFurnitureProject.Application.Repositories;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Infrastructures.Repositories;
using eFurnitureProject.Infrastructures.Mappers;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using eFurnitureProject.Application;

namespace eFurnitureProject.Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            #region Service DI
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            #endregion

            #region Repository DI
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentDetailRepository, AppointmentDetailRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartDetailRepository, CartDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IImportDetailRepository, ImportDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderProcessingRepository, OrderProcessingRepository>();
            services.AddScoped<IOrderProcessingDetailRepository, OrderProcessingDetailRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IStatusOrderProcessingRepository, StatusOrderProcessingRepository>();
            services.AddScoped<IStatusOrderRepository, StatusOrderRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherDetailRepository, VoucherDetailRepository>();
            services.AddScoped<IProductService, ProductService>();
            #endregion
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

           services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);


            return services;
        }
    }
}
