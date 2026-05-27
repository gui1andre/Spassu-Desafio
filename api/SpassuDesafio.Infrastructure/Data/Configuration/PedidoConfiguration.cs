using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpassuDesafio.Infrastructure.Data.Configuration
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(f => f.Id)
               .ValueGeneratedNever();

            builder.Property(f => f.ClienteNome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.CriadoEm)
                .IsRequired();

            builder.Property(f => f.Status)
                .IsRequired();

            builder.Property(f => f.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(f => f.Itens)
                .WithOne()
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(f => f.Itens)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
