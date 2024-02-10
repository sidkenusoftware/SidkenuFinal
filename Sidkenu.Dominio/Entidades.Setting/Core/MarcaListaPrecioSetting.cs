using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Setting.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidkenu.Dominio.Entidades.Setting.Core
{
    public class MarcaListaPrecioSetting : EntidadBaseSetting,
        Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<MarcaListaPrecio>
    {
        public void Configure(EntityTypeBuilder<MarcaListaPrecio> builder)
        {
            // Propiedades
            builder.Property(x => x.MarcaId)
                .IsRequired();

            builder.Property(x => x.ListaPrecioId)
                .IsRequired();

            builder.Property(x => x.TipoValor)
                .IsRequired();

            builder.Property(x => x.Valor).HasPrecision(18, 6)
                .IsRequired();

            // Propiedades de Navegacion

            builder.HasOne(x => x.Marca)
                .WithMany(x => x.MarcaListaPrecios)
                .HasForeignKey(x => x.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ListaPrecio)
                .WithMany(x => x.MarcaListaPrecios)
                .HasForeignKey(x => x.ListaPrecioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Empresa)
                .WithMany(x => x.MarcaListaPrecios)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}