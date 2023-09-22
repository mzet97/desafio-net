using Desafio.Domain.Interfaces.Services;
using Desafio.Domain.Interfaces;
using Desafio.Domain.Models;
using Desafio.Infra.CrossCutting.Notifications;
using Desafio.Infra.Data.Context;
using Desafio.Service;
using Desafio.WebAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Desafio.Domain.Interfaces.Repository;
using Desafio.Infra.Data.Repository;

namespace Desafio.WebAPI.Configuration
{
    public static class DependencyInjectionConfig
    {

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<DataIdentityDbContext>();


            services.AddScoped<INotifier, Notifier>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddScoped<RoleManager<IdentityRole<Guid>>>();
            services.AddScoped<UserManager<User>>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            #region Repository
            services.AddScoped<IPessoaRepository, PessoaRepository>();

            #endregion

            #region Service
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<IAuthService, AuthService>();

            #endregion

            return services;
        }

    }
}
