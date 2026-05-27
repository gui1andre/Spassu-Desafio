using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SpassuDesafio.Application.Mappings;
using SpassuDesafio.Application.Pedido.Interfaces;
using SpassuDesafio.Application.Pedidos;
using SpassuDesafio.Application.Validators;

namespace SpassuDesafio.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<PedidoDTOValidator>();

            services.AddAutoMapper(c => c.AddProfile<PedidoMappingProfile>(), typeof(DependencyInjection));

            services.AddScoped<IPedidoService, PedidoService>();

            return services;
        }
    }
}
