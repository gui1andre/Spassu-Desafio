using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpassuDesafio.Infrastructure.Data.Configuration
{
    public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {


            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ProdutoNome)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(i => i.PedidoId)
                .IsRequired();

            builder.Property(i => i.Quantidade)
                .IsRequired();

            builder.Property(i => i.PrecoUnitario)
                .HasColumnType("decimal(18,2)");            
        }
    }
}
