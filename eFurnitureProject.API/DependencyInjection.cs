﻿using eFurnitureProject.API.Services;
using eFurnitureProject.API.Validator.AuthenticationValidator;
using eFurnitureProject.API.Validator.ContractValidator;
using eFurnitureProject.Application;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.Services;
using eFurnitureProject.Application.ViewModels.ContractViewModels;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using eFurnitureProject.Domain.Entities;
using eFurnitureProject.Infrastructures;
using eFurnitureProject.Infrastructures.DataInitializer;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

namespace eFurnitureProject.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "eFurniture API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddHealthChecks();
            services.AddSingleton<Stopwatch>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<RoleInitializer>();
            services.AddHttpContextAccessor();
            services.AddHostedService<SetupIdentityDataSeeder>();
            #region Validator
            services.AddTransient<IValidator<UserRegisterDTO>, UserRegisterValidation>();
            services.AddTransient<IValidator<CreateContractViewModel>, CreateContractViewModelValidation>();
            services.AddTransient<IValidator<UpdateContractDTO>, UpdateContractValidation>();
            #endregion

            return services;
        }
    }
}
