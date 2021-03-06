﻿using Autofac;
using AutoMapper;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Services;
using System.Collections.Generic;
using Module = Autofac.Module;

namespace Sindikat.Identity.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LoadAutomapper(builder);

            builder.RegisterType<CacheService>().As<ICacheService>();

            builder.RegisterType<JwtService>().As<IJwtService>();

            builder.RegisterType<RefreshTokenService>().As<IRefreshTokenService>();

            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<AuthValidatorService>().As<IAuthValidatorService>();

            builder.RegisterType<ClaimService>().As<IClaimService>();
            builder.RegisterType<ClaimValidatorService>().As<IClaimValidatorService>();

            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserValidatorService>().As<IUserValidatorService>();

            builder.RegisterType<RoleService>().As<IRoleService>();
        }

        private void LoadAutomapper(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IAuthService).Assembly).As<Profile>();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }

    }
}
