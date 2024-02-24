using eFurnitureProject.API.Services;
using eFurnitureProject.API.Validator.AuthenticationValidator;
using eFurnitureProject.Application;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Infrastructures;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace eFurnitureProject.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddSingleton<Stopwatch>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();

            #region Validator
            services.AddTransient<IValidator<UserRegisterDTO>, UserRegisterValidation>();
            #endregion

            return services;
        }
    }
}
