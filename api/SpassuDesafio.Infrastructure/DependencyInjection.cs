using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SpassuDesafio.Domain.Interfaces;
using SpassuDesafio.Infrastructure.Data;
using SpassuDesafio.Infrastructure.Repositories;

namespace SpassuDesafio.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(op => op.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPedidoRepository, PedidoRepository>();

            return services;
        }
    }
}
