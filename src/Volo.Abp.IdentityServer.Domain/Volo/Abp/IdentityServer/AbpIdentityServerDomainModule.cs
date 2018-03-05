﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerDomainSharedModule))]
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpIdentityDomainModule))]
    [DependsOn(typeof(AbpSecurityModule))]
    public class AbpIdentityServerDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ClientAutoMapperProfile>(validate: true);
            });

            AddIdentityServer(services);

            services.AddAssemblyOf<AbpIdentityServerDomainModule>();
        }

        private static void AddIdentityServer(IServiceCollection services)
        {
            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            });

            identityServerBuilder
                .AddDeveloperSigningCredential() //TODO: Should be able to change this!
                .AddAbpIdentityServer();

            services.ExecutePreConfiguredActions(identityServerBuilder);
        }
    }
}
