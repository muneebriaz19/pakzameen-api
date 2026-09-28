using Application.Context;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Domain;
using Domain.Common;
using InfraStructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using Domain.Interfaces;
using Domain.Services;
using Org.BouncyCastle.Asn1.Cms;
using Domain.Utility;
using Application.Entites;
using Domain.Interfaces.Authentication;

namespace InfraStructure
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configurationManager)
        {
            var connectionString = configurationManager.GetConnectionString("Database");

            //var connectionString = Encrypter.DecryptString(configurationManager.GetSection("Secrets")["masterkey"], configurationManager.GetConnectionString("Database"));
            services.AddDbContext<SubcrybeContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            //services.Configure<Settings>(configurationManager.GetSection("file"));
            services.AddIdentity<AppUsers, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                
                
            })
                .AddEntityFrameworkStores<SubcrybeContext>().AddDefaultTokenProviders();

             services.AddTransient<IUser, UserService>();
            services.AddSingleton<IEmailSender,EmailSender>();
            services.AddSingleton<IJwtTokenService, JwtService>();
           // services.AddTransient<IAdmin, AdminService>();
            services.AddTransient<IProperty, PropertyService>();
            services.AddTransient<ICommon, CommonServices>();



            return services;
        }

    }
}
